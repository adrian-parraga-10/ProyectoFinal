using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ProyectoFinal.ViewsModel
{
    public class HistorialSesionesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SesionEntrenamiento> Sesiones { get; set; } = new();

        public Command CargarSesionesCommand { get; }

        public HistorialSesionesViewModel()
        {
            CargarSesionesCommand = new Command(async () => await CargarSesionesAsync());
        }

        public async Task CargarSesionesAsync()
        {
            var bbdd = new BBDD();
            var usuarioId = GlobalData.Instance.UsuarioId;
            var sesiones = await bbdd.ObtenerSesionesPorUsuario(usuarioId);

            Sesiones.Clear();
            foreach (var sesion in sesiones.OrderByDescending(s => s.FechaSesion))
            {
                Sesiones.Add(sesion);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
