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

    private Ejercicio _ejercicioSeleccionado = new();
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

    public Command EliminarMusculoCommand { get; }
    public Command AgregarMusculoCommand { get; }

    public AdminEjerciciosViewModel()
    {
        GuardarCommand = new Command(async () => await Guardar());
        EliminarCommand = new Command(async () => await Eliminar());
        NuevoCommand = new Command(Nuevo);
        AgregarMusculoCommand = new Command(AgregarMusculo);
        EliminarMusculoCommand = new Command<string>(EliminarMusculo);

        Cargar();
    }

    private void EliminarMusculo(string musculo)
    {
        if (!string.IsNullOrWhiteSpace(musculo))
        {
            EjercicioSeleccionado.MusculosTrabajados.Remove(musculo);
            OnPropertyChanged(nameof(EjercicioSeleccionado.MusculosTrabajados)); 
        }
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
        if (string.IsNullOrWhiteSpace(EjercicioSeleccionado?.Nombre)) return;

        // Filtrar los músculos vacíos
        EjercicioSeleccionado.MusculosTrabajados = new ObservableCollection<string>(
            EjercicioSeleccionado.MusculosTrabajados.Where(m => !string.IsNullOrWhiteSpace(m))
        );

        if (EjercicioSeleccionado.Id == ObjectId.Empty)
            await _bbdd.InsertarEjercicioAsync(EjercicioSeleccionado);
        else
            await _bbdd.ActualizarEjercicioAsync(EjercicioSeleccionado);

        await Cargar();
        Nuevo();
    }

    private async Task Eliminar()
    {
        if (EjercicioSeleccionado?.Id == ObjectId.Empty) return;

        await _bbdd.EliminarEjercicioAsync(EjercicioSeleccionado.Id.ToString());
        await Cargar();
        Nuevo();
    }

    private void Nuevo()
    {
        EjercicioSeleccionado = new Ejercicio
        {
            MusculosTrabajados = new ObservableCollection<string> { "" }
        };
    }

    private void AgregarMusculo()
    {
        EjercicioSeleccionado.MusculosTrabajados.Add(""); // Asegúrate de añadir un músculo vacío
        OnPropertyChanged(nameof(EjercicioSeleccionado.MusculosTrabajados)); // Notificar que la colección ha cambiado
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
