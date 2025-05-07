using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Usuario = await _bbdd.ObtenerUsuarioPorIdAsync(usuarioId); // Debes implementar este método si aún no lo tienes
        }

        private async Task GuardarCambios()
        {
            await _bbdd.ActualizarUsuarioAsync(Usuario); // Debes implementar este método si aún no lo tienes
            await App.Current.MainPage.DisplayAlert("Perfil", "Datos actualizados correctamente", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }

}
