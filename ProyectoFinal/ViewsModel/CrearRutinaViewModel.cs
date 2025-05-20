using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProyectoFinal.ViewsModel
{
    public class CrearRutinaViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();
        public ObservableCollection<Ejercicio> TodosEjercicios { get; set; } = new ObservableCollection<Ejercicio>();
        public ObservableCollection<Ejercicio> EjerciciosSeleccionados { get; set; } = new ObservableCollection<Ejercicio>();

        public string NombreRutina { get; set; }
        public string DescripcionRutina { get; set; }

        public ICommand GuardarRutinaCommand { get; }
        public ICommand ActualizarSeleccionCommand { get; }  

        public CrearRutinaViewModel()
        {
            GuardarRutinaCommand = new Command(async () => await GuardarRutina());
            ActualizarSeleccionCommand = new Command<Ejercicio>(ActualizarSeleccion);  
            CargarEjercicios();
        }

        private async Task CargarEjercicios()
        {
            var lista = await _bbdd.ObtenerEjerciciosAsync();
            foreach (var e in lista)
                TodosEjercicios.Add(e);
        }

        private async Task GuardarRutina()
        {
            // Verifica que EjerciciosSeleccionados no esté vacío
            if (EjerciciosSeleccionados.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar al menos un ejercicio.", "OK");
                return; 
            }

            // Crear la lista de EjercicioRutina a partir de los ejercicios seleccionados
            var ejerciciosRutina = EjerciciosSeleccionados.Select(e => new EjercicioRutina
            {
                EjercicioId = e.Id, 
                Nombre = e.Nombre,
                Categoria = e.Categoria,
                SerieHistorial = new List<SerieHistorial>() 
            }).ToList();

            // Crear la nueva rutina con la lista de ejercicios
            var nuevaRutina = new Rutina
            {
                Nombre = NombreRutina,
                Descripcion = DescripcionRutina,
                UsuarioId = GlobalData.Instance.UsuarioActual.Id,
                Publica = false,
                Ejercicios = ejerciciosRutina 
            };

            
            await _bbdd.GuardarRutinaAsync(nuevaRutina);

            
            await Application.Current.MainPage.DisplayAlert("Éxito", "Rutina guardada correctamente.", "OK");

            // Limpiar campos
            NombreRutina = string.Empty;
            DescripcionRutina = string.Empty;
            EjerciciosSeleccionados.Clear();

            OnPropertyChanged(nameof(NombreRutina));
            OnPropertyChanged(nameof(DescripcionRutina));
            OnPropertyChanged(nameof(EjerciciosSeleccionados));
        }

        private void ActualizarSeleccion(Ejercicio ejercicio)
        {
            if (EjerciciosSeleccionados.Contains(ejercicio))
            {
                EjerciciosSeleccionados.Remove(ejercicio); 
            }
            else
            {
                EjerciciosSeleccionados.Add(ejercicio); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
