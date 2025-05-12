using Microcharts;
using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class SeguimientoFisicoViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new();
    public ObservableCollection<ProgresoFisico> HistorialProgreso { get; set; } = new();

    private double _peso;
    public double Peso
    {
        get => _peso;
        set
        {
            if (_peso != value)
            {
                _peso = value;
                OnPropertyChanged();
            }
        }
    }

    private Chart _graficoPeso;
    public Chart GraficoPeso
    {
        get => _graficoPeso;
        set
        {
            _graficoPeso = value;
            OnPropertyChanged();
        }
    }

    public Command GuardarProgresoCommand { get; }

    public SeguimientoFisicoViewModel()
    {
        GuardarProgresoCommand = new Command(async () => await GuardarProgreso());
        InicializarAsync();
    }

    private async Task InicializarAsync()
    {
        await CargarHistorial();
        GenerarGrafico(); // Generamos gráfico al inicio
    }

    private async Task GuardarProgreso()
    {
        var nuevo = new ProgresoFisico
        {
            UsuarioId = GlobalData.Instance.UsuarioId,
            Fecha = DateTime.Today,
            Peso = Peso
        };

        await _bbdd.GuardarProgresoFisicoAsync(nuevo);
        await _bbdd.ActualizarPesoUsuarioAsync(GlobalData.Instance.UsuarioId, Peso);

        HistorialProgreso.Add(nuevo);
        OnPropertyChanged(nameof(HistorialProgreso));

        GenerarGrafico(); // Regenerar gráfico
    }

    private async Task CargarHistorial()
    {
        var lista = await _bbdd.ObtenerProgresosUsuarioAsync(GlobalData.Instance.UsuarioId);

        HistorialProgreso.Clear();
        // Filtrar solo los últimos 7 registros
        var ultimos7Registros = lista.OrderByDescending(x => x.Fecha)
                                      .Take(7)  // Solo tomar los últimos 7 registros
                                      .OrderBy(x => x.Fecha)  // Ordenarlos nuevamente para que estén en orden cronológico
                                      .ToList();

        foreach (var item in ultimos7Registros)
        {
            HistorialProgreso.Add(item);
        }
    }

    private void GenerarGrafico()
    {
        var entries = HistorialProgreso
            .Select(p => new ChartEntry((float)p.Peso)
            {
                Label = p.Fecha.ToString("dd/MM"),
                ValueLabel = p.Peso.ToString("F1"),
                Color = SKColor.Parse("#00FFAA"),
                ValueLabelColor = SKColor.Parse("#FFFFFF")
            })
            .ToList();

        GraficoPeso = new LineChart
        {
            Entries = entries,
            LineMode = LineMode.Straight,  // Usar líneas rectas entre los puntos
            LineSize = 4,  // Grosor de la línea
            PointMode = PointMode.Circle,  // Estilo de los puntos (círculos)
            PointSize = 6,  // Tamaño de los puntos
            BackgroundColor = SKColors.Transparent  // Fondo transparente
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
