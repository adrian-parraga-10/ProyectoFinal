using Microcharts;
using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class EstadisticasViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new();
    private ObservableCollection<SesionEntrenamiento> _sesiones = new();

    public ObservableCollection<SesionEntrenamiento> Sesiones
    {
        get => _sesiones;
        set
        {
            _sesiones = value;
            OnPropertyChanged();
        }
    }

    private Chart _graficoPesoSemanal;
    public Chart GraficoPesoSemanal
    {
        get => _graficoPesoSemanal;
        set { _graficoPesoSemanal = value; OnPropertyChanged(); }
    }

    private Chart _graficoFrecuenciaSemanal;
    public Chart GraficoFrecuenciaSemanal
    {
        get => _graficoFrecuenciaSemanal;
        set { _graficoFrecuenciaSemanal = value; OnPropertyChanged(); }
    }

    private Chart _graficoEjercicios;
    public Chart GraficoEjercicios
    {
        get => _graficoEjercicios;
        set { _graficoEjercicios = value; OnPropertyChanged(); }
    }

    private string _mensaje;
    public string Mensaje
    {
        get => _mensaje;
        set { _mensaje = value; OnPropertyChanged(); }
    }

    private string _totalSesiones;
    public string TotalSesiones
    {
        get => _totalSesiones;
        set { _totalSesiones = value; OnPropertyChanged(); }
    }

    private string _promedioEjerciciosPorSesion;
    public string PromedioEjerciciosPorSesion
    {
        get => _promedioEjerciciosPorSesion;
        set { _promedioEjerciciosPorSesion = value; OnPropertyChanged(); }
    }

    private string _promedioRepeticionesPorEjercicio;
    public string PromedioRepeticionesPorEjercicio
    {
        get => _promedioRepeticionesPorEjercicio;
        set { _promedioRepeticionesPorEjercicio = value; OnPropertyChanged(); }
    }

    private string _promedioPesoLevado;
    public string PromedioPesoLevado
    {
        get => _promedioPesoLevado;
        set { _promedioPesoLevado = value; OnPropertyChanged(); }
    }

    public EstadisticasViewModel()
    {
        CargarEstadisticas();
    }

    private async void CargarEstadisticas()
    {
        try
        {
            var sesiones = await _bbdd.ObtenerSesionesPorUsuario(GlobalData.Instance.UsuarioId);
            Sesiones = new ObservableCollection<SesionEntrenamiento>(sesiones);
            CalcularEstadisticas();
        }
        catch (Exception ex)
        {
            Mensaje = $"Error: {ex.Message}";
        }
    }

    private void CalcularEstadisticas()
    {
        if (Sesiones.Count == 0)
        {
            Mensaje = "No hay sesiones registradas.";
            TotalSesiones = "0";
            PromedioEjerciciosPorSesion = "0";
            PromedioRepeticionesPorEjercicio = "0";
            PromedioPesoLevado = "0";
            GraficoPesoSemanal = null;
            GraficoFrecuenciaSemanal = null;
            GraficoEjercicios = null;
            return;
        }

        Mensaje = "";

        TotalSesiones = Sesiones.Count.ToString();
        PromedioEjerciciosPorSesion = Sesiones.Average(s => s.Ejercicios.Count).ToString("F2");

        var totalReps = Sesiones.SelectMany(s => s.Ejercicios)
                                .SelectMany(e => e.Series)
                                .Sum(s => s.Repeticiones);
        var totalEjercicios = Sesiones.SelectMany(s => s.Ejercicios).Count();
        PromedioRepeticionesPorEjercicio = totalEjercicios > 0 ? (totalReps / (double)totalEjercicios).ToString("F2") : "0";

        var promedioPeso = Sesiones.SelectMany(s => s.Ejercicios)
                                  .SelectMany(e => e.Series)
                                  .Average(s => s.Peso);
        PromedioPesoLevado = promedioPeso.ToString("F2");

        // Gráfico: Evolución semanal del peso promedio levantado
        var pesoPorSemana = Sesiones
            .GroupBy(s => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                s.FechaSesion, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday))
            .OrderBy(g => g.Key)
            .Select(g =>
            {
                var promedio = g.SelectMany(s => s.Ejercicios)
                                .SelectMany(e => e.Series)
                                .Average(s => s.Peso);
                return new ChartEntry((float)promedio)
                {
                    Label = $"S{g.Key}",
                    ValueLabel = promedio.ToString("F1"),
                    Color = SKColor.Parse("#00FF99"),
                    ValueLabelColor = SKColors.White,
                };
            }).ToList();

        GraficoPesoSemanal = new LineChart
        {
            Entries = pesoPorSemana,
            LineMode = LineMode.Straight,
            LineSize = 4,
            PointMode = PointMode.Circle,
            PointSize = 8,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 20,
        };

        // Gráfico: Frecuencia semanal (cantidad de sesiones por semana)
        var frecuenciaSemanal = Sesiones
            .GroupBy(s => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                s.FechaSesion, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday))
            .OrderBy(g => g.Key)
            .Select(g => new ChartEntry(g.Count())
            {
                Label = $"S{g.Key}",
                ValueLabel = g.Count().ToString(),
                Color = SKColor.Parse("#4CAF50"),
                ValueLabelColor = SKColors.White,
            }).ToList();

        GraficoFrecuenciaSemanal = new BarChart
        {
            Entries = frecuenciaSemanal,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 20,
            ValueLabelOrientation = Orientation.Horizontal,
            MaxValue = (float)frecuenciaSemanal.Max(e => e.Value),
        };

        // Gráfico: Distribución de ejercicios con colores distintos
        var ejerciciosAgrupados = Sesiones.SelectMany(s => s.Ejercicios)
                                          .GroupBy(e => e.Nombre)
                                          .OrderByDescending(g => g.Count());

        var colores = new SKColor[]
        {
            SKColor.Parse("#00FF99"),
            SKColor.Parse("#FF5722"),
            SKColor.Parse("#2196F3"),
            SKColor.Parse("#FFC107"),
            SKColor.Parse("#9C27B0"),
            SKColor.Parse("#E91E63"),
            SKColor.Parse("#009688"),
        };

        var ejerciciosEntries = ejerciciosAgrupados.Select((g, i) => new ChartEntry(g.Count())
        {
            Label = g.Key,
            ValueLabel = g.Count().ToString(),
            Color = colores[i % colores.Length],
            ValueLabelColor = SKColors.White
        }).ToList();

        GraficoEjercicios = new BarChart
        {
            Entries = ejerciciosEntries,
            BackgroundColor = SKColors.Transparent,
            LabelTextSize = 18,
            ValueLabelOrientation = Orientation.Horizontal
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
