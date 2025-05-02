namespace ProyectoFinal.Vista;
using ProyectoFinal.ViewsModel;

public partial class HistorialSesionesView : ContentPage
{
    private readonly HistorialSesionesViewModel viewModel;

    public HistorialSesionesView()
    {
        InitializeComponent();
        viewModel = new HistorialSesionesViewModel();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.CargarSesionesCommand.Execute(null);
    }
}
