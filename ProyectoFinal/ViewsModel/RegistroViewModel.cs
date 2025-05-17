using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.ViewModels
{
    public class RegistroViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _confirmarPassword;
        private string _nombre;
        private string _edadTexto;
        private string _pesoTexto;
        private string _alturaTexto;
        private string _sexo;
        private string _statusMessage;

        private readonly BBDD _bbdd;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RegistroCommand { get; }

        public RegistroViewModel()
        {
            _bbdd = new BBDD();
            RegistroCommand = new Command(async () => await RegistrarUsuarioAsync());
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ConfirmarPassword
        {
            get => _confirmarPassword;
            set { _confirmarPassword = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }

        public string EdadTexto
        {
            get => _edadTexto;
            set { _edadTexto = value; OnPropertyChanged(); }
        }

        public string PesoTexto
        {
            get => _pesoTexto;
            set { _pesoTexto = value; OnPropertyChanged(); }
        }

        public string AlturaTexto
        {
            get => _alturaTexto;
            set { _alturaTexto = value; OnPropertyChanged(); }
        }

        public string Sexo
        {
            get => _sexo;
            set { _sexo = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private async Task RegistrarUsuarioAsync()
        {
            if (!int.TryParse(EdadTexto, out int edad) || edad <= 0 ||
                !double.TryParse(PesoTexto, out double peso) || peso <= 0 ||
                !double.TryParse(AlturaTexto, out double altura) || altura <= 0)
            {
                StatusMessage = "Por favor complete todos los campos correctamente.";
                return;
            }

            if (Password != ConfirmarPassword)
            {
                StatusMessage = "Las contraseñas no coinciden.";
                return;
            }

            // Crear usuario nuevo
            var nuevoUsuario = new Usuario
            {
                Email = Email,
                Contraseña = Password,  // En producción encriptar la contraseña
                Rol = "usuario",
                Nombre = Nombre,
                Edad = edad,
                Sexo = Sexo,
                Peso = peso,
                Altura = altura,
                FechaCreacion = DateTime.Now,
                Objetivos = new List<string>(),
                FotoPerfil = "default.jpg",
                RutinasGuardadas = new List<string>(),
                DietaGuardada = ""
            };

            try
            {
                await _bbdd.InsertarUsuarioAsync(nuevoUsuario);
                StatusMessage = "Registro exitoso. Ahora puede iniciar sesión.";

                // Limpiar campos
                Email = string.Empty;
                Password = string.Empty;
                ConfirmarPassword = string.Empty;
                Nombre = string.Empty;
                EdadTexto = "";
                Sexo = string.Empty;
                PesoTexto = "";
                AlturaTexto = "";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al registrar: {ex.Message}";
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
