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
        GenerarGrafico(); 
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

        GenerarGrafico(); 
    }

    private async Task CargarHistorial()
    {
        var lista = await _bbdd.ObtenerProgresosUsuarioAsync(GlobalData.Instance.UsuarioId);

        HistorialProgreso.Clear();
        // Filtrar solo los últimos 7 registros
        var ultimos7Registros = lista.OrderByDescending(x => x.Fecha)
                                      .Take(7)  
                                      .OrderBy(x => x.Fecha)  
                                      .ToList();

        foreach (var item in ultimos7Registros)
        {
            HistorialProgreso.Add(item);
        }
    }

    private async void GenerarGrafico()
    {
        if (HistorialProgreso == null || HistorialProgreso.Count == 0)
        {
            GraficoPeso = null;
            return;
        }

        
        GraficoPeso = null;
        await Task.Delay(50); 

        
        var progresoFiltrado = HistorialProgreso
            .OrderBy(p => p.Fecha)
            .GroupBy(p => p.Fecha.Date)
            .Select(g => g.Last()) // Último peso registrado por día
            .ToList();

        var entries = progresoFiltrado
            .Select(p => new ChartEntry((float)p.Peso)
            {
                Label = p.Fecha.ToString("dd/MM"),
                ValueLabel = p.Peso.ToString("F1"),
                Color = SKColor.Parse("#00FFAA"),
                ValueLabelColor = SKColor.Parse("#FFFFFF")
            })
            .ToList();

        // Rango dinámico para mostrar mejor las variaciones pequeñas
        float minPeso = (float)progresoFiltrado.Min(p => p.Peso) - 1f;
        float maxPeso = (float)progresoFiltrado.Max(p => p.Peso) + 1f;

        GraficoPeso = new LineChart
        {
            Entries = entries,
            LineMode = LineMode.Straight,
            LineSize = 4,
            PointMode = PointMode.Circle,
            PointSize = 6,
            MinValue = minPeso,
            MaxValue = maxPeso,
            BackgroundColor = SKColors.Transparent
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
