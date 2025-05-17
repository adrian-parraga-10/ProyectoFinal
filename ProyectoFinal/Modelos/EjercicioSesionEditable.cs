using MongoDB.Bson;
using ProyectoFinal.Modelos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class EjercicioSesionEditable : INotifyPropertyChanged
{
    public ObjectId EjercicioId { get; set; }

    private string _nombre;
    public string Nombre
    {
        get => _nombre;
        set
        {
            _nombre = value;
            OnPropertyChanged();
        }
    }

    private string _categoria;
    public string Categoria
    {
        get => _categoria;
        set
        {
            _categoria = value;
            OnPropertyChanged();
        }
    }

    private int _numeroSeriesDeseadas = 3;
    public int NumeroSeriesDeseadas
    {
        get => _numeroSeriesDeseadas;
        set
        {
            if (_numeroSeriesDeseadas != value)
            {
                _numeroSeriesDeseadas = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Serie> Series { get; set; } = new ObservableCollection<Serie>();

    public void EstablecerSeriesPredeterminadas(int numeroDeSeries)
    {
        Series.Clear();
        for (int i = 0; i < numeroDeSeries; i++)
        {
            Series.Add(new Serie
            {
                SerieNumero = i + 1,
                Repeticiones = 10,
                Peso = 0
            });
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}



