using ProyectoFinal.Modelos;
using ProyectoFinal.ViewModels;

namespace ProyectoFinal.Vista;

public partial class AdminUsuariosView : ContentPage
{
	public AdminUsuariosView()
	{
		InitializeComponent();
        BindingContext = new AdminUsuariosViewModel();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        MessagingCenter.Subscribe<AdminUsuariosViewModel, Usuario>(this, "UsuarioSeleccionado", async (sender, usuario) =>
        {
            await DisplayAlert("Usuario seleccionado", $"Has seleccionado: {usuario.Nombre}", "OK");
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<AdminUsuariosViewModel, Usuario>(this, "UsuarioSeleccionado");
    }
}