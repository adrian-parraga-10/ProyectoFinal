using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.ViewsModel
{
    public class EstadisticasViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();
        private ObservableCollection<SesionEntrenamiento> _sesiones;
        private string _mensaje;

        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
            }
        }

        private string _totalSesiones;
        public string TotalSesiones
        {
            get => _totalSesiones;
            set
            {
                _totalSesiones = value;
                OnPropertyChanged();
            }
        }

        private string _promedioEjerciciosPorSesion;
        public string PromedioEjerciciosPorSesion
        {
            get => _promedioEjerciciosPorSesion;
            set
            {
                _promedioEjerciciosPorSesion = value;
                OnPropertyChanged();
            }
        }

        private string _promedioRepeticionesPorEjercicio;
        public string PromedioRepeticionesPorEjercicio
        {
            get => _promedioRepeticionesPorEjercicio;
            set
            {
                _promedioRepeticionesPorEjercicio = value;
                OnPropertyChanged();
            }
        }

        private string _mejoraEnRepeticiones;
        public string MejoraEnRepeticiones
        {
            get => _mejoraEnRepeticiones;
            set
            {
                _mejoraEnRepeticiones = value;
                OnPropertyChanged();
            }
        }

        private string _promedioPesoLevado;
        public string PromedioPesoLevado
        {
            get => _promedioPesoLevado;
            set
            {
                _promedioPesoLevado = value;
                OnPropertyChanged();
            }
        }

        private string _mejoraEnPesoLevado;
        public string MejoraEnPesoLevado
        {
            get => _mejoraEnPesoLevado;
            set
            {
                _mejoraEnPesoLevado = value;
                OnPropertyChanged();
            }
        }

        private string _frecuenciaEntrenamientos;
        public string FrecuenciaEntrenamientos
        {
            get => _frecuenciaEntrenamientos;
            set
            {
                _frecuenciaEntrenamientos = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<SesionEntrenamiento> Sesiones
        {
            get => _sesiones;
            set
            {
                _sesiones = value;
                OnPropertyChanged();
            }
        }

        public EstadisticasViewModel()
        {
            CargarEstadisticas();
        }

        private async void CargarEstadisticas()
        {
            try
            {
                // Cargar las sesiones del usuario
                var sesiones = await _bbdd.ObtenerSesionesPorUsuario(GlobalData.Instance.UsuarioId);

                if (sesiones != null && sesiones.Count > 0)
                {
                    Sesiones = new ObservableCollection<SesionEntrenamiento>(sesiones);
                    CalcularEstadisticas();
                }
                else
                {
                    Mensaje = "No se han encontrado entrenamientos para este usuario.";
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al cargar las estadísticas: {ex.Message}";
            }
        }

        private void CalcularEstadisticas()
        {
            // Total de sesiones
            TotalSesiones = Sesiones.Count.ToString();

            // Promedio de ejercicios por sesión
            var promedioEjercicios = Sesiones.Average(s => s.Ejercicios.Count);
            PromedioEjerciciosPorSesion = promedioEjercicios.ToString("F2");

            // Promedio de repeticiones por ejercicio
            var totalRepeticiones = Sesiones.SelectMany(s => s.Ejercicios)
                                             .SelectMany(e => e.Series)
                                             .Sum(s => s.Repeticiones);
            var totalEjercicios = Sesiones.SelectMany(s => s.Ejercicios).Count();
            PromedioRepeticionesPorEjercicio = totalEjercicios > 0 ? (totalRepeticiones / totalEjercicios).ToString("F2") : "0";

            // Mejora en repeticiones (última repetición vs primera repetición)
            var mejoraRepeticiones = Sesiones.SelectMany(s => s.Ejercicios)
                                              .SelectMany(e => e.Series)
                                              .OrderBy(s => s.SerieNumero)
                                              .GroupBy(s => s.SerieNumero)
                                              .Select(g => g.Last().Repeticiones - g.First().Repeticiones)
                                              .Max();
            MejoraEnRepeticiones = mejoraRepeticiones > 0 ? $"Mejora en repeticiones: {mejoraRepeticiones} repeticiones" : "No hay mejora en repeticiones";

            // Promedio de peso levantado
            var promedioPeso = Sesiones.SelectMany(s => s.Ejercicios)
                                       .SelectMany(e => e.Series)
                                       .Average(s => s.Peso);
            PromedioPesoLevado = promedioPeso.ToString("F2");

            // Mejora en el peso levantado
            var mejoraPeso = Sesiones.SelectMany(s => s.Ejercicios)
                                     .SelectMany(e => e.Series)
                                     .OrderBy(s => s.Peso)
                                     .LastOrDefault()?.Peso ?? 0;
            MejoraEnPesoLevado = mejoraPeso > 0 ? $"Último peso levantado: {mejoraPeso} kg" : "No hay datos de peso";

            // Frecuencia de entrenamientos
            var frecuencia = Sesiones.GroupBy(s => s.FechaSesion.Date)
                                     .Count();
            FrecuenciaEntrenamientos = $"Frecuencia de entrenamientos: {frecuencia} días";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }


}
