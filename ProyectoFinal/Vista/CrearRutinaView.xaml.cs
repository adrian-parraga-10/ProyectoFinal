using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class CrearRutinaView : ContentPage
{
	public CrearRutinaView()
	{
		InitializeComponent();
        BindingContext = new CrearRutinaViewModel();
    }
}