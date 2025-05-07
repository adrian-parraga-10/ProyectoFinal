using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class HistorialConsumoViewModel : INotifyPropertyChanged
{
    private readonly BBDD _bbdd = new();

    // Observable collection para mostrar los alimentos consumidos
    public ObservableCollection<ConsumoAlimento> AlimentosDeEseDia { get; set; } = new();

    // Propiedad para la fecha seleccionada desde el calendario
    private DateTime _fechaSeleccionada;
    public DateTime FechaSeleccionada
    {
        get => _fechaSeleccionada;
        set
        {
            if (_fechaSeleccionada != value)
            {
                _fechaSeleccionada = value;
                OnPropertyChanged(nameof(FechaSeleccionada));
                _ = CargarAlimentosPorFecha(); // Cargar alimentos para la fecha seleccionada
            }
        }
    }

    public HistorialConsumoViewModel()
    {
        // Inicializamos la fecha seleccionada como la fecha actual
        FechaSeleccionada = DateTime.Today;
        _ = CargarAlimentosPorFecha(); // Cargar alimentos para la fecha inicial
    }

    private async Task CargarAlimentosPorFecha()
    {
        var usuarioId = GlobalData.Instance.UsuarioId;
        var consumo = await _bbdd.ObtenerConsumoDiarioPorFechaAsync(usuarioId, FechaSeleccionada);

        // Limpiamos la lista y cargamos los nuevos alimentos para la fecha seleccionada
        AlimentosDeEseDia.Clear();
        if (consumo != null)
        {
            foreach (var alimento in consumo.AlimentosConsumidos)
                AlimentosDeEseDia.Add(alimento);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
