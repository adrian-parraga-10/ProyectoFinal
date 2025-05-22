using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class AdminEjerciciosViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new();
    private string _busqueda;

    public ObservableCollection<Ejercicio> Ejercicios { get; set; } = new();
    public ObservableCollection<Ejercicio> EjerciciosFiltrados { get; set; } = new();

    public string Busqueda
    {
        get => _busqueda;
        set
        {
            _busqueda = value;
            OnPropertyChanged();
            FiltrarEjercicios();
        }
    }

    private Ejercicio _ejercicioSeleccionado = new Ejercicio();
    public Ejercicio EjercicioSeleccionado
    {
        get => _ejercicioSeleccionado;
        set
        {
            _ejercicioSeleccionado = value;
            OnPropertyChanged();
        }
    }

    public Command GuardarCommand { get; }
    public Command EliminarCommand { get; }
    public Command NuevoCommand { get; }

    public AdminEjerciciosViewModel()
    {
        GuardarCommand = new Command(async () => await Guardar());
        EliminarCommand = new Command(async () => await Eliminar());
        NuevoCommand = new Command(Nuevo);

        Cargar();
    }

    private async Task Cargar()
    {
        var lista = await _bbdd.ObtenerEjerciciosAsync();
        Ejercicios.Clear();
        foreach (var e in lista)
            Ejercicios.Add(e);

        FiltrarEjercicios();
    }

    private void FiltrarEjercicios()
    {
        EjerciciosFiltrados.Clear();

        var filtrados = string.IsNullOrWhiteSpace(Busqueda)
            ? Ejercicios
            : new ObservableCollection<Ejercicio>(Ejercicios.Where(e =>
                e.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                e.Categoria.Contains(Busqueda, StringComparison.OrdinalIgnoreCase)));

        foreach (var e in filtrados)
            EjerciciosFiltrados.Add(e);
    }

    private async Task Guardar()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EjercicioSeleccionado?.Nombre))
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "El nombre del ejercicio es obligatorio.", "OK");
                return;
            }

            if (EjercicioSeleccionado.Id == ObjectId.Empty)
                await _bbdd.InsertarEjercicioAsync(EjercicioSeleccionado);
            else
                await _bbdd.ActualizarEjercicioAsync(EjercicioSeleccionado);

            await Application.Current.MainPage.DisplayAlert("Éxito", "Ejercicio guardado correctamente.", "OK");
            await Cargar();

            Nuevo();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo guardar el ejercicio: {ex.Message}", "OK");
        }
    }


    private async Task Eliminar()
    {
        if (EjercicioSeleccionado?.Id == ObjectId.Empty) return;

        bool confirmar = await Application.Current.MainPage.DisplayAlert(
            "Confirmar eliminación",
            $"¿Seguro que deseas eliminar el ejercicio \"{EjercicioSeleccionado.Nombre}\"?",
            "Sí", "No");

        if (!confirmar) return;

        await _bbdd.EliminarEjercicioAsync(EjercicioSeleccionado.Id.ToString());
        await Cargar();
        Nuevo();
    }


    private void Nuevo()
    {
        EjercicioSeleccionado = new Ejercicio();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
