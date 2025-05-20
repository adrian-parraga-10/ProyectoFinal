using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinal.Vista;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Vista
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private void OnEmailCompleted(object sender, EventArgs e)
        {
            PasswordEntry.Focus(); // Salta al siguiente campo al presionar Enter
        }

        private void OnPasswordCompleted(object sender, EventArgs e)
        {
            if (BindingContext is LoginViewModel vm && vm.LoginCommand.CanExecute(null))
                vm.LoginCommand.Execute(null); // Presiona Enter para iniciar sesión
        }


    }
}
