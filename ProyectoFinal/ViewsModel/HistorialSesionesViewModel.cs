using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using ProyectoFinal.Vista;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace ProyectoFinal.ViewsModel
{
    public class HistorialSesionesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SesionEntrenamiento> Sesiones { get; set; } = new();

        public Command CargarSesionesCommand { get; }
        public Command<SesionEntrenamiento> VerDetallesCommand { get; }


        public HistorialSesionesViewModel()
        {
            CargarSesionesCommand = new Command(async () => await CargarSesionesAsync());
            VerDetallesCommand = new Command<SesionEntrenamiento>(async (sesion) => await VerDetallesSesion(sesion));

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
        private async Task VerDetallesSesion(SesionEntrenamiento sesion)
        {
            if (sesion == null) return;

            await Application.Current.MainPage.Navigation.PushAsync(new DetallesSesionView(sesion));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }

}
