using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewsModel
{
    public class EjerciciosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new();

        public ObservableCollection<Ejercicio> Ejercicios { get; set; } = new();

        private Ejercicio _ejercicioSeleccionado;
        public Ejercicio EjercicioSeleccionado
        {
            get => _ejercicioSeleccionado;
            set
            {
                _ejercicioSeleccionado = value;
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

        public EjerciciosViewModel()
        {
            _ = CargarEjerciciosAsync();
        }

        private async Task CargarEjerciciosAsync()
        {
            var lista = await _bbdd.ObtenerEjerciciosLimitadoAsync(30);
            Ejercicios = new ObservableCollection<Ejercicio>(lista);
            OnPropertyChanged(nameof(Ejercicios));
        }

        private async Task BuscarDesdeServidorAsync(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                var pocos = await _bbdd.ObtenerEjerciciosLimitadoAsync(30);
                Ejercicios = new ObservableCollection<Ejercicio>(pocos);
            }
            else
            {
                var resultados = await _bbdd.BuscarEjerciciosAsync(texto);
                Ejercicios = new ObservableCollection<Ejercicio>(resultados);
            }

            OnPropertyChanged(nameof(Ejercicios));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
