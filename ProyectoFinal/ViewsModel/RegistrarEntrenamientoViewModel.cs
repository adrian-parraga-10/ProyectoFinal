using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MongoDB.Bson;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewModels
{
    public class RegistrarEntrenamientoViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();

        public ObservableCollection<EjercicioSesionEditable> EjerciciosRutina { get; set; } = new ObservableCollection<EjercicioSesionEditable>();
        public ICommand GuardarSesionCommand { get; }
        public ICommand ActualizarSeriesCommand { get; }

        // Modificar el constructor para recibir la rutina seleccionada
        public RegistrarEntrenamientoViewModel(Rutina rutina)
        {
            GuardarSesionCommand = new Command(async () => await GuardarSesion());
            CargarEjerciciosRutina(rutina);  // Cargar ejercicios de la rutina seleccionada
            ActualizarSeriesCommand = new Command<EjercicioSesionEditable>(ActualizarSeries);

        }

        private void CargarEjerciciosRutina(Rutina rutina)
        {
            if (rutina != null && rutina.Ejercicios != null)
            {
                foreach (var ejercicio in rutina.Ejercicios)
                {
                    var ejercicioEditable = new EjercicioSesionEditable
                    {
                        EjercicioId = ejercicio.EjercicioId,
                        Nombre = ejercicio.Nombre,
                        Categoria = ejercicio.Categoria
                    };

                    // Establece el número de series predeterminadas (por ejemplo 3)
                    ejercicioEditable.EstablecerSeriesPredeterminadas(3);  // 3 series como ejemplo

                    EjerciciosRutina.Add(ejercicioEditable);
                }
            }
        }

        private async Task GuardarSesion()
        {
            var sesionEntrenamiento = new SesionEntrenamiento
            {
                FechaSesion = DateTime.Now,
                UsuarioId = GlobalData.Instance.UsuarioActual.Id,
                Ejercicios = new List<EjercicioSesion>()
            };

            // Iterar sobre cada ejercicio en EjerciciosRutina
            foreach (var ejercicio in EjerciciosRutina)
            {
                var ejercicioSesion = new EjercicioSesion
                {
                    EjercicioId = ejercicio.EjercicioId,
                    Series = new List<Serie>()
                };

                foreach (var serie in ejercicio.Series)
                {
                    ejercicioSesion.Series.Add(new Serie
                    {
                        SerieNumero = serie.SerieNumero,
                        Repeticiones = serie.Repeticiones,
                        Peso = serie.Peso
                    });
                }

                sesionEntrenamiento.Ejercicios.Add(ejercicioSesion);
            }



            await _bbdd.GuardarSesionEntrenamientoAsync(sesionEntrenamiento);
            await Application.Current.MainPage.DisplayAlert("Éxito", "Sesión guardada correctamente.", "OK");

            // Limpiar campos
            EjerciciosRutina.Clear();
        }

        private void ActualizarSeries(EjercicioSesionEditable ejercicio)
        {
            ejercicio.Series.Clear();

            for (int i = 0; i < ejercicio.NumeroSeriesDeseadas; i++)
            {
                ejercicio.Series.Add(new Serie
                {
                    SerieNumero = i + 1,
                    Repeticiones = 10,
                    Peso = 0
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }

}
