using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class HistorialConsumoView : ContentPage
{
	public HistorialConsumoView()
	{
		InitializeComponent();
        BindingContext = new HistorialConsumoViewModel();
    }
}