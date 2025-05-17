using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Vista;

public partial class AdminUsuariosView : ContentPage
{
	public AdminUsuariosView()
	{
		InitializeComponent();
        BindingContext = new AdminUsuariosViewModel();
    }
}