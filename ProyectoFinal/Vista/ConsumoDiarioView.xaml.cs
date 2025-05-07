using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista;

public partial class ConsumoDiarioView : ContentPage
{
    public ConsumoDiarioView()
    {
        InitializeComponent();
        BindingContext = new ConsumoDiarioViewModel();
    }
}