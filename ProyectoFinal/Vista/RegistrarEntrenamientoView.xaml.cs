using ProyectoFinal.Modelos;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Vista;

public partial class RegistrarEntrenamientoView : ContentPage
{
	public RegistrarEntrenamientoView(Rutina rutina)
	{
		InitializeComponent();
        BindingContext = new RegistrarEntrenamientoViewModel(rutina);
    }
}