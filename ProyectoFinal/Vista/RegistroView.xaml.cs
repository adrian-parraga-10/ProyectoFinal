using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Vista;

public partial class RegistroView : ContentPage
{
	public RegistroView()
	{
		InitializeComponent();
		BindingContext = new RegistroViewModel();
    }
}