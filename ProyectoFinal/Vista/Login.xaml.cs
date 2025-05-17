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

    }
}
