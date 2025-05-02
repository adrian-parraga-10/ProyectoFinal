using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ProyectoFinal.Modelos;
using ProyectoFinal.Vista;
public class RutinasViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new BBDD();
    public ObservableCollection<Rutina> Rutinas { get; set; } = new ObservableCollection<Rutina>();
    private Rutina _rutaSeleccionada;
    private bool _detallesVisible;

    public ICommand IrARegistrarRutinaCommand { get; }

    public Rutina RutaSeleccionada
    {
        get => _rutaSeleccionada;
        set
        {
            if (_rutaSeleccionada != value)
            {
                _rutaSeleccionada = value;
                OnPropertyChanged();
                // Actualiza la visibilidad de los detalles si la rutina seleccionada cambia
                OnRutaSeleccionadaChanged();
            }
        }
    }

    public bool DetallesVisible
    {
        get => _detallesVisible;
        set
        {
            if (_detallesVisible != value)
            {
                _detallesVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public RutinasViewModel()
    {
        CargarRutinas();
        IrARegistrarRutinaCommand = new Command(IrARegistrarRutina);
    }

    private async void CargarRutinas()
    {
        try
        {
            var listaRutinas = await _bbdd.ObtenerRutinasUsuarioActualAsync();

            if (listaRutinas != null && listaRutinas.Count > 0)
            {
                foreach (var rutina in listaRutinas.ToList())
                {
                    var rutinaConEjercicios = await _bbdd.ObtenerRutinaConEjercicios(rutina.Id);
                    if (rutinaConEjercicios != null)
                    {
                        Rutinas.Add(rutinaConEjercicios);
                    }
                }
            }
            else
            {
                Console.WriteLine("No se han encontrado rutinas para este usuario.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar las rutinas: {ex.Message}");
        }
    }


    private async void OnRutaSeleccionadaChanged()
    {
        // Cambiar la visibilidad de los detalles según si la rutina seleccionada es válida
        DetallesVisible = RutaSeleccionada != null;

        // Verifica si la rutina seleccionada es válida y si realmente necesita actualizarse
        if (RutaSeleccionada != null && RutaSeleccionada.Ejercicios == null)
        {
            // Aquí solo actualizamos los ejercicios si no están ya cargados
            RutaSeleccionada = await _bbdd.ObtenerRutinaConEjercicios(RutaSeleccionada.Id);
        }
    }

    private async void IrARegistrarRutina()
    {
        if (RutaSeleccionada != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistrarEntrenamientoView(RutaSeleccionada));
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Aviso", "Selecciona una rutina primero.", "OK");
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
}
