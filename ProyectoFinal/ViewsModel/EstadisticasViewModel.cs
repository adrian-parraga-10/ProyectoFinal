using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microcharts;
using SkiaSharp;


namespace ProyectoFinal.ViewsModel
{
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();
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

        private DateTime _fechaInicio = DateTime.Now.AddDays(-7);
        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set
            {
                _fechaInicio = value;
                OnPropertyChanged();
                CalcularEstadisticasFiltradas();
            }
        }

        private DateTime _fechaFin = DateTime.Now;
        public DateTime FechaFin
        {
            get => _fechaFin;
            set
            {
                _fechaFin = value;
                OnPropertyChanged();
                CalcularEstadisticasFiltradas();
            }
        }

        private Chart _graficoPesoSemanal;
        public Chart GraficoPesoSemanal
        {
            get => _graficoPesoSemanal;
            set
            {
                _graficoPesoSemanal = value;
                OnPropertyChanged();
            }
        }


        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
                MensajeVisible = !string.IsNullOrWhiteSpace(value);
            }
        }

        private bool _mensajeVisible;
        public bool MensajeVisible
        {
            get => _mensajeVisible;
            set
            {
                _mensajeVisible = value;
                OnPropertyChanged();
            }
        }

        public string TotalSesiones { get => _totalSesiones; set { _totalSesiones = value; OnPropertyChanged(); } }
        public string PromedioEjerciciosPorSesion { get => _promedioEjercicios; set { _promedioEjercicios = value; OnPropertyChanged(); } }
        public string PromedioRepeticionesPorEjercicio { get => _promedioReps; set { _promedioReps = value; OnPropertyChanged(); } }
        public string PromedioPesoLevado { get => _promedioPeso; set { _promedioPeso = value; OnPropertyChanged(); } }

        private string _totalSesiones;
        private string _promedioEjercicios;
        private string _promedioReps;
        private string _promedioPeso;

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
                CalcularEstadisticasFiltradas();
            }
            catch (Exception ex)
            {
                Mensaje = $"Error: {ex.Message}";
            }
        }

        private void CalcularEstadisticasFiltradas()
        {
            var filtradas = Sesiones.Where(s =>
                s.FechaSesion.Date >= FechaInicio.Date &&
                s.FechaSesion.Date <= FechaFin.Date).ToList();

            if (!filtradas.Any())
            {
                Mensaje = "No hay entrenamientos en este periodo.";
                TotalSesiones = "0";
                PromedioEjerciciosPorSesion = "0";
                PromedioRepeticionesPorEjercicio = "0";
                PromedioPesoLevado = "0";
                return;
            }

            Mensaje = ""; // limpiar
            TotalSesiones = filtradas.Count.ToString();

            PromedioEjerciciosPorSesion = filtradas.Average(s => s.Ejercicios.Count).ToString("F2");

            var totalReps = filtradas.SelectMany(s => s.Ejercicios).SelectMany(e => e.Series).Sum(s => s.Repeticiones);
            var totalEjercicios = filtradas.SelectMany(s => s.Ejercicios).Count();
            PromedioRepeticionesPorEjercicio = totalEjercicios > 0 ? (totalReps / totalEjercicios).ToString("F2") : "0";

            var promedioPeso = filtradas.SelectMany(s => s.Ejercicios).SelectMany(e => e.Series).Average(s => s.Peso);
            PromedioPesoLevado = promedioPeso.ToString("F2");

            // Agrupar por semana y calcular el peso promedio
            var porSemana = Sesiones
                .Where(s => s.FechaSesion >= FechaInicio && s.FechaSesion <= FechaFin)
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
                        Label = g.Key.ToString(),  // Semana
                        ValueLabel = promedio.ToString("F1"),
                        Color = SKColor.Parse("#00FF99")
                    };
                }).ToList();

            // Cambiar a LineChart
            GraficoPesoSemanal = new LineChart
            {
                Entries = porSemana,
                LineMode = LineMode.Straight,
                LineSize = 3,
                BackgroundColor = SKColors.Transparent,
                LabelTextSize = 20
            };
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

