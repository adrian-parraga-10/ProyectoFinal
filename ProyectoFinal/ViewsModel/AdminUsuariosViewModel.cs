using System.Collections.ObjectModel;
using ProyectoFinal.Modelos;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewModels
{
    public class AdminUsuariosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new();
        public ObservableCollection<Usuario> Usuarios { get; set; } = new();
        public ObservableCollection<Usuario> UsuariosFiltrados { get; set; } = new();
        private string _busqueda;

        public string Busqueda
        {
            get => _busqueda;
            set
            {
                _busqueda = value;
                OnPropertyChanged();
                FiltrarUsuarios();
            }
        }

        public Command EliminarUsuarioCommand { get; }

        private Usuario _usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                OnPropertyChanged();

                if (value != null)
                {
                    // Notificar a la vista que se ha seleccionado un usuario
                    MessagingCenter.Send(this, "UsuarioSeleccionado", value);
                }
            }
        }


        public AdminUsuariosViewModel()
        {
            EliminarUsuarioCommand = new Command(EliminarUsuario);
            CargarUsuarios();
        }

        private async Task CargarUsuarios()
        {
            // Obtener la lista de usuarios desde la base de datos
            var listaUsuarios = await _bbdd.ObtenerUsuariosAsync();
            Usuarios.Clear();
            foreach (var usuario in listaUsuarios)
            {
                Usuarios.Add(usuario);
            }
            FiltrarUsuarios();
        }

        private void FiltrarUsuarios()
        {
            // Filtrar la lista de usuarios
            UsuariosFiltrados.Clear();
            var filtrados = string.IsNullOrWhiteSpace(Busqueda)
                ? Usuarios
                : new ObservableCollection<Usuario>(Usuarios.Where(u =>
                    u.Nombre.Contains(Busqueda, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(Busqueda, StringComparison.OrdinalIgnoreCase)));

            foreach (var usuario in filtrados)
            {
                UsuariosFiltrados.Add(usuario);
            }
        }
        private async void EliminarUsuario()
        {
            if (UsuarioSeleccionado == null)
                return;

            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Seguro que deseas eliminar al usuario \"{UsuarioSeleccionado.Nombre}\"?",
                "Sí", "No");

            if (!confirmar)
                return;

            try
            {
                await _bbdd.EliminarUsuarioAsync(UsuarioSeleccionado.Id);
                await CargarUsuarios();
                UsuarioSeleccionado = null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar el usuario: {ex.Message}", "OK");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
