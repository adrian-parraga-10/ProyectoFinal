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
    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var ejercicioSeleccionado = e.Item as Ejercicio;
            var viewModel = BindingContext as EjerciciosViewModel;
            viewModel.EjercicioSeleccionado = ejercicioSeleccionado; // Establecer el ejercicio seleccionado
        }
    }
}
