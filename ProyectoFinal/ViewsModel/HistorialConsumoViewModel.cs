using Microcharts;
using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;


public class HistorialConsumoViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new();

    public ObservableCollection<ConsumoAlimento> AlimentosDeEseDia { get; set; } = new();

    private DateTime _fechaSeleccionada;
    public DateTime FechaSeleccionada
    {
        get => _fechaSeleccionada;
        set
        {
            if (_fechaSeleccionada != value)
            {
                _fechaSeleccionada = value;
                OnPropertyChanged(nameof(FechaSeleccionada));
                CargarAlimentosPorFecha(); 
            }
        }
    }

    private Chart _graficoMacros;
    public Chart GraficoMacros
    {
        get => _graficoMacros;
        set
        {
            _graficoMacros = value;
            OnPropertyChanged();
        }
    }


    public HistorialConsumoViewModel()
    {
        FechaSeleccionada = DateTime.Today;
        CargarAlimentosPorFecha(); 
    }

    private async Task CargarAlimentosPorFecha()
    {
        var usuarioId = GlobalData.Instance.UsuarioId;
        var consumo = await _bbdd.ObtenerConsumoDiarioPorFechaAsync(usuarioId, FechaSeleccionada);

        AlimentosDeEseDia.Clear();
        if (consumo != null)
        {
            foreach (var alimento in consumo.AlimentosConsumidos)
                AlimentosDeEseDia.Add(alimento);
        }

        
        ActualizarGrafico();
    }


    private async void ActualizarGrafico()
    {
        if (AlimentosDeEseDia == null || !AlimentosDeEseDia.Any())
        {
            GraficoMacros = null;
            return;
        }

        // Limpiar el gráfico antes de asignar uno nuevo
        GraficoMacros = null;
        OnPropertyChanged(nameof(GraficoMacros)); 
        await Task.Delay(50);

        float proteinas = (float)AlimentosDeEseDia.Sum(a => a.Proteinas);
        float carbohidratos = (float)AlimentosDeEseDia.Sum(a => a.Carbohidratos);
        float grasas = (float)AlimentosDeEseDia.Sum(a => a.Grasas);

        var entries = new[]
        {
        new ChartEntry(proteinas) { Label = "Proteínas", ValueLabel = $"{proteinas}g", Color = SKColor.Parse("#4CAF50"),ValueLabelColor = SKColors.Green},
        new ChartEntry(carbohidratos) { Label = "Carbohidratos", ValueLabel = $"{carbohidratos}g", Color = SKColor.Parse("#2196F3"),ValueLabelColor = SKColors.Green },
        new ChartEntry(grasas) { Label = "Grasas", ValueLabel = $"{grasas}g", Color = SKColor.Parse("#FF9800"),ValueLabelColor = SKColors.Green }
    };

        GraficoMacros = new PieChart
        {
            Entries = entries,
            LabelTextSize = 18,
            LabelColor = SKColors.White,
            BackgroundColor = SKColors.Transparent,

        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
