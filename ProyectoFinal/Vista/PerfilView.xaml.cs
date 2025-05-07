using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class PerfilView : ContentPage
{
	public PerfilView()
	{
		InitializeComponent();
        BindingContext = new PerfilViewModel();
    }
}