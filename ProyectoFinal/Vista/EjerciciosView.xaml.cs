using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class EjerciciosView : ContentPage
{
    public EjerciciosView()
    {
        InitializeComponent();
        BindingContext = new EjerciciosViewModel();
    }

    // Manejar el evento cuando se toca un ítem en la lista
    private void OnItemTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            var viewModel = BindingContext as EjerciciosViewModel;
            viewModel.EjercicioSeleccionado = e.CurrentSelection[0] as Ejercicio;
        }
    }

}
