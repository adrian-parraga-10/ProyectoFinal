using ProyectoFinal.Modelos;

namespace ProyectoFinal.Vista;

public partial class DetallesSesionView : ContentPage
{
	public DetallesSesionView(SesionEntrenamiento sesion)
	{
		InitializeComponent();
        BindingContext = sesion;
    }
}