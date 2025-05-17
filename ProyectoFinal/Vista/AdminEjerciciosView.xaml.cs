namespace ProyectoFinal.Vista;
using ProyectoFinal.ViewsModel;

public partial class AdminEjerciciosView : ContentPage
{
	public AdminEjerciciosView()
	{
		InitializeComponent();
        BindingContext = new AdminEjerciciosViewModel();
    }
}