using ProyectoFinal.ViewsModel;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.Vista;

public partial class AlimentosView : ContentPage
{
    public AlimentosView()
    {
        InitializeComponent();
        BindingContext = new AlimentosViewModel();
    }

    // Manejar el evento cuando se toca un ítem en la lista
    private void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            var alimentoSeleccionado = e.Item as Alimento;
            var viewModel = BindingContext as AlimentosViewModel;
            viewModel.AlimentoSeleccionado = alimentoSeleccionado; 
        }
    }
}
