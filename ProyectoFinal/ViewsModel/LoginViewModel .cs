
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _statusMessage;
        private readonly BBDD _bbdd;

        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor donde inicializas la conexión con la base de datos
        public LoginViewModel()
        {
            _bbdd = new BBDD(); // Inicializamos la instancia de BBDD
        }

        // Propiedades de Email, Password y StatusMessage
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // Método para manejar el login
        public async Task OnLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Por favor, ingrese su email y contraseña.";
                return;
            }

            var usuario = await _bbdd.ObtenerUsuarioAsync(Email, Password);

            if (usuario != null)
            {
                StatusMessage = "✅ Inicio de sesión correcto";

                // Realizamos la navegación según el rol
                if (usuario.Rol == "admin")
                {
                    // Cambiar de shell a AdminShell
                    Application.Current.MainPage = new AdminShell(); // Cambiar el Shell a AdminShell
                }
                else
                {
                    // Cambiar el MainPage a UserShell
                    Application.Current.MainPage = new UserShell();
                }
            }
            else
            {
                StatusMessage = "❌ Usuario o contraseña incorrectos";
            }
        }

        // Método que notifica cuando se ha actualizado una propiedad
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
