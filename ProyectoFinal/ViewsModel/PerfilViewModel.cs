using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System.ComponentModel;

namespace ProyectoFinal.ViewsModel
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new();
        private Usuario _usuario;

        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public Command GuardarCambiosCommand { get; }

        public PerfilViewModel()
        {
            _ = CargarUsuario();
            GuardarCambiosCommand = new Command(async () => await GuardarCambios());
        }

        private async Task CargarUsuario()
        {
            var usuarioId = GlobalData.Instance.UsuarioId;
            Usuario = await _bbdd.ObtenerUsuarioPorIdAsync(usuarioId); 
        }

        private async Task GuardarCambios()
        {
            await _bbdd.ActualizarUsuarioAsync(Usuario); 
            await App.Current.MainPage.DisplayAlert("Perfil", "Datos actualizados correctamente", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }

}
