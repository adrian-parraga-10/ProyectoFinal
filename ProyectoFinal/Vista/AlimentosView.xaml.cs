namespace ProyectoFinal.Vista;

using ProyectoFinal.ViewsModel;

public partial class AlimentosView : ContentPage
{
    public AlimentosView()
    {
        InitializeComponent();
        BindingContext = new AlimentosViewModel();
    }
}