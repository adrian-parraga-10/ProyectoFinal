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

        public ObservableCollection<Alimento> AlimentosFiltrados { get; set; } = new();

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
                BuscarEnBBDD(); 
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

            _ = BuscarEnBBDD();
        }

        private async Task BuscarEnBBDD()
        {
            var lista = await _bbdd.BuscarAlimentosAsync(Busqueda); 
            AlimentosFiltrados.Clear();
            foreach (var alimento in lista.Take(50)) 
                AlimentosFiltrados.Add(alimento);
        }

        private async Task GuardarAlimento()
        {
            if (string.IsNullOrWhiteSpace(AlimentoSeleccionado?.Nombre)) return;

            if (AlimentoSeleccionado.Id == ObjectId.Empty)
                await _bbdd.InsertarAlimentoAsync(AlimentoSeleccionado);
            else
                await _bbdd.ActualizarAlimentoAsync(AlimentoSeleccionado);

            await BuscarEnBBDD();
            NuevoAlimento();
        }

        private async Task EliminarAlimento()
        {
            if (AlimentoSeleccionado?.Id == ObjectId.Empty) return;

            await _bbdd.EliminarAlimentoAsync(AlimentoSeleccionado.Id);
            await BuscarEnBBDD();
            NuevoAlimento();
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
