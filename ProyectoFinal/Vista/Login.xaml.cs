using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoFinal.Vista;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        // Manejar el evento Clicked del botón de login
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as LoginViewModel;
            if (viewModel != null)
            {
                await viewModel.OnLoginAsync(); // Llama al método OnLoginAsync desde el ViewModel
            }
        }
    }
}
