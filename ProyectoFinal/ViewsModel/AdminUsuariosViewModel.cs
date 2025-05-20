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

        public Command NuevoUsuarioCommand { get; }
        public Command EliminarUsuarioCommand { get; }

        private Usuario _usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                OnPropertyChanged();
            }
        }

        // Variables para los campos de nuevo usuario
        public string NuevoUsuarioNombre { get; set; }
        public string NuevoUsuarioEmail { get; set; }
        public int NuevoUsuarioEdad { get; set; }
        public string NuevoUsuarioSexo { get; set; }
        public double NuevoUsuarioPeso { get; set; }
        public double NuevoUsuarioAltura { get; set; }

        public AdminUsuariosViewModel()
        {
            NuevoUsuarioCommand = new Command(NuevoUsuario);
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

        private async void NuevoUsuario()
        {
            
            var nuevoUsuario = new Usuario
            {
                Nombre = NuevoUsuarioNombre,
                Email = NuevoUsuarioEmail,
                Edad = NuevoUsuarioEdad,
                Sexo = NuevoUsuarioSexo,
                Rol = "Usuario",  
                Peso = NuevoUsuarioPeso,
                Altura = NuevoUsuarioAltura,
                FechaCreacion = DateTime.Now,
                Objetivos = new List<string>(), 
                FotoPerfil = "default.jpg",     
                RutinasGuardadas = new List<string>(),
                DietaGuardada = ""
            };

            // Guardar el nuevo usuario en la base de datos
            await _bbdd.InsertarUsuarioAsync(nuevoUsuario);
            await CargarUsuarios();
        }

        private async void EliminarUsuario()
        {
            if (UsuarioSeleccionado != null)
            {
                // Eliminar el usuario seleccionado
                await _bbdd.EliminarUsuarioAsync(UsuarioSeleccionado.Id);
                await CargarUsuarios();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
