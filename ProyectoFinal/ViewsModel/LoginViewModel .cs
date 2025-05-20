using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Win32;
using ProyectoFinal.Singleton;
using ProyectoFinal.Vista;

namespace ProyectoFinal.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _email;
        private string _password;
        private string _statusMessage;
        private readonly BBDD _bbdd;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel()
        {
            _bbdd = new BBDD();

            LoginCommand = new Command(async () => await OnLoginAsync());
            NavigateToRegisterCommand = new Command(async () => await NavegarRegistroAsync());
           
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

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        private async Task OnLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Por favor, ingrese su email y contraseña.";
                return;
            }

            var usuario = await _bbdd.ObtenerUsuarioAsync(Email, Password);

            if (usuario != null)
            {
                GlobalData.Instance.UsuarioActual = usuario;
                GlobalData.Instance.UsuarioId = usuario.Id;
                StatusMessage = "✅ Inicio de sesión correcto";

                if (usuario.Rol == "admin")
                    Application.Current.MainPage = new AdminShell();
                else
                    Application.Current.MainPage = new UserShell();
            }
            else
            {
                StatusMessage = "❌ Usuario o contraseña incorrectos.";
            }
        }

        private async Task NavegarRegistroAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegistroView());
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
