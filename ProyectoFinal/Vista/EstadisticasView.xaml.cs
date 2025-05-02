using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class EstadisticasView : ContentPage
{
	public EstadisticasView()
	{
		InitializeComponent();
        BindingContext = new EstadisticasViewModel();
    }
}