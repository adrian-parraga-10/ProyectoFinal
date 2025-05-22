using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.ViewsModel
{
    public class AdminAlimentosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new();
        private string _busqueda;
        public ObservableCollection<Alimento> Alimentos { get; set; } = new();

        

        private Alimento _alimentoSeleccionado = new() { Id = ObjectId.Empty };
        public Alimento AlimentoSeleccionado
        {
            get => _alimentoSeleccionado;
            set
            {
                _alimentoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged();
                BuscarDesdeServidorAsync(_busqueda);
            }
        }


        public Command GuardarAlimentoCommand { get; }
        public Command EliminarAlimentoCommand { get; }
        public Command NuevoAlimentoCommand { get; }

        public AdminAlimentosViewModel()
        {
            GuardarAlimentoCommand = new Command(async () => await GuardarAlimento());
            EliminarAlimentoCommand = new Command(async () => await EliminarAlimento());
            NuevoAlimentoCommand = new Command(NuevoAlimento);
            CargarAlimentosAsync();
            
        }
        private async Task CargarAlimentosAsync()
        {
            var lista = await _bbdd.ObtenerAlimentosLimitadoAsync(10);
            Alimentos = new ObservableCollection<Alimento>(lista);
            OnPropertyChanged(nameof(Alimentos));
        }

        private async Task BuscarDesdeServidorAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                var pocos = await _bbdd.ObtenerAlimentosLimitadoAsync(30);
                Alimentos = new ObservableCollection<Alimento>(pocos);
            }
            else
            {
                var resultados = await _bbdd.BuscarAlimentosAsync(texto);
                Alimentos = new ObservableCollection<Alimento>(resultados);
            }

            OnPropertyChanged(nameof(Alimentos));
        }
        

        private async Task GuardarAlimento()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AlimentoSeleccionado?.Nombre))
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "El nombre del alimento es obligatorio.", "OK");
                    return;
                }

                if (!ValidarCamposNumericos())
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Los campos numéricos deben ser valores válidos y no negativos.", "OK");
                    return;
                }

                if (AlimentoSeleccionado.Id == ObjectId.Empty)
                    await _bbdd.InsertarAlimentoAsync(AlimentoSeleccionado);
                else
                    await _bbdd.ActualizarAlimentoAsync(AlimentoSeleccionado);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Alimento guardado correctamente.", "OK");
                await BuscarDesdeServidorAsync("");
                NuevoAlimento();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo guardar el alimento: {ex.Message}", "OK");
            }
        }

        private bool ValidarCamposNumericos()
        {
            bool EsValido(double? valor) => valor == null || valor >= 0;

            return
                EsValido(AlimentoSeleccionado.Calorias) &&
                EsValido(AlimentoSeleccionado.Proteinas) &&
                EsValido(AlimentoSeleccionado.Carbohidratos) &&
                EsValido(AlimentoSeleccionado.Grasas) &&
                EsValido(AlimentoSeleccionado.Fibra);
        }

        private async Task EliminarAlimento()
        {
            try
            {
                if (AlimentoSeleccionado?.Id == ObjectId.Empty)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Selecciona un alimento válido para eliminar.", "OK");
                    return;
                }

                bool confirmar = await Application.Current.MainPage.DisplayAlert(
                    "Confirmar eliminación",
                    $"¿Seguro que deseas eliminar el alimento \"{AlimentoSeleccionado.Nombre}\"?",
                    "Sí", "No");

                if (!confirmar) return;

                await _bbdd.EliminarAlimentoAsync(AlimentoSeleccionado.Id);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Alimento eliminado correctamente.", "OK");
                await BuscarDesdeServidorAsync("");
                NuevoAlimento();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar el alimento: {ex.Message}", "OK");
            }
        }

        private void NuevoAlimento()
        {
            AlimentoSeleccionado = new Alimento { Id = ObjectId.Empty };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
