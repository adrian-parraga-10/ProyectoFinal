using ProyectoFinal.ViewsModel;

namespace ProyectoFinal.Vista
{
    public partial class RutinasView : ContentPage
    {
        public RutinasView()
        {
            InitializeComponent();
            BindingContext = new RutinasViewModel();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is RutinasViewModel vm)
            {
                await vm.CargarRutinasAsync();
            }
        }


    }
}