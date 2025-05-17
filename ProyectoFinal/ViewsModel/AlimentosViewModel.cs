using ProyectoFinal.Modelos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewsModel
{
    public class AlimentosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();

        public ObservableCollection<Alimento> Alimentos { get; set; } = new();

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
            }
        }

        private Alimento _alimentoSeleccionado;
        public Alimento AlimentoSeleccionado
        {
            get => _alimentoSeleccionado;
            set
            {
                _alimentoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        private string _textoBusqueda;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set
            {
                _textoBusqueda = value;
                OnPropertyChanged();
                _ = BuscarDesdeServidorAsync(_textoBusqueda);
            }
        }

        public AlimentosViewModel()
        {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
