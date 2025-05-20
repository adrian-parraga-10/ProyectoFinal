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
    public ICommand EliminarRutinaCommand { get; }

    public Rutina RutaSeleccionada
    {
        get => _rutaSeleccionada;
        set
        {
            if (_rutaSeleccionada != value)
            {
                _rutaSeleccionada = value;
                OnPropertyChanged();
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
        IrARegistrarRutinaCommand = new Command(IrARegistrarRutina);
        EliminarRutinaCommand = new Command<Rutina>(async (rutina) => await EliminarRutinaAsync(rutina));
    }


    public async Task CargarRutinasAsync()
    {
        Rutinas.Clear();

        try
        {
            var listaRutinas = await _bbdd.ObtenerRutinasUsuarioActualAsync();

            if (listaRutinas != null && listaRutinas.Count > 0)
            {
                foreach (var rutina in listaRutinas)
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
        
        DetallesVisible = RutaSeleccionada != null;
        
        if (RutaSeleccionada != null && RutaSeleccionada.Ejercicios == null)
        {
            
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

    private async Task EliminarRutinaAsync(Rutina rutina)
    {
        if (rutina == null)
            return;

        bool confirmar = await Application.Current.MainPage.DisplayAlert(
            "Confirmar",
            $"¿Seguro que deseas eliminar la rutina \"{rutina.Nombre}\"?",
            "Sí", "No");

        if (!confirmar) return;

        try
        {
            await _bbdd.EliminarRutinaAsync(rutina.Id);
            Rutinas.Remove(rutina);
            RutaSeleccionada = null;
            DetallesVisible = false;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar la rutina: {ex.Message}", "OK");
        }
    }



    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
}
