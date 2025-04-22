using ProyectoFinal.Modelos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewsModel
{
    public class EjerciciosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();

        public ObservableCollection<Ejercicio> Ejercicios { get; set; } = new ObservableCollection<Ejercicio>();

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

        public EjerciciosViewModel()
        {
            CargarEjercicios();
        }

        private async void CargarEjercicios()
        {
            var lista = await _bbdd.ObtenerEjerciciosAsync();

            if (lista != null && lista.Count > 0)
            {
                foreach (var ejercicio in lista)
                    Ejercicios.Add(ejercicio);
            }
            else
            {
                Mensaje = "No hay ejercicios disponibles.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
