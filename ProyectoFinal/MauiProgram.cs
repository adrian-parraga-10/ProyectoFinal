using Microsoft.Extensions.Logging;
using ProyectoFinal.Vista;
using ProyectoFinal.ViewsModel;
using Microcharts.Maui; 

namespace ProyectoFinal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMicrocharts() 
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Registro de servicios y vistas
            builder.Services.AddTransient<EstadisticasViewModel>();
            builder.Services.AddTransient<EstadisticasView>();

            return builder.Build();
        }
    }
}

