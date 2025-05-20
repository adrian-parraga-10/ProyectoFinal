using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class AjustesView : ContentPage
{
	public AjustesView()
	{
		InitializeComponent();
		BindingContext = new AjustesViewModel();
	}
}