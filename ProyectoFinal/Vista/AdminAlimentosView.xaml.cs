using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class AdminAlimentosView : ContentPage
{
	public AdminAlimentosView()
	{
		InitializeComponent();
        BindingContext = new AdminAlimentosViewModel();
    }
}