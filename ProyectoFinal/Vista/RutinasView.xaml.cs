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
    }
}