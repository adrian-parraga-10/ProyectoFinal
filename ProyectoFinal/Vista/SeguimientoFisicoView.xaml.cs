using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class SeguimientoFisicoView : ContentPage
{
	public SeguimientoFisicoView()
	{
		InitializeComponent();
		BindingContext = new SeguimientoFisicoViewModel();
	}
}