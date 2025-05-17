using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using ProyectoFinal.Modelos;
using ProyectoFinal.Singleton;

namespace ProyectoFinal.ViewsModel
{
    public class ConsumoDiarioViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();
        private DateTime _fechaActual = DateTime.Today;

        public ObservableCollection<Alimento> Alimentos { get; set; } = new();
        public ObservableCollection<ConsumoAlimento> AlimentosConsumidos { get; set; } = new();

        public int TotalCalorias => AlimentosConsumidos.Sum(a => a.Calorias);
        public int TotalProteinas => AlimentosConsumidos.Sum(a => a.Proteinas);
        public int TotalCarbohidratos => AlimentosConsumidos.Sum(a => a.Carbohidratos);
        public int TotalGrasas => AlimentosConsumidos.Sum(a => a.Grasas);

        private Alimento _alimentoSeleccionado;
        public Alimento AlimentoSeleccionado
        {
            get => _alimentoSeleccionado;
            set
            {
                _alimentoSeleccionado = value;
                OnPropertyChanged();
            }
        }

        private int _cantidadGramos;
        public int CantidadGramos
        {
            get => _cantidadGramos;
            set
            {
                _cantidadGramos = value;
                OnPropertyChanged();
            }
        }

        public Command AgregarAlimentoCommand { get; }
        public Command<ConsumoAlimento> EliminarAlimentoCommand { get; }

        public ConsumoDiarioViewModel()
        {
            CargarAlimentos();
            CargarConsumo();
            AgregarAlimentoCommand = new Command(async () => await AgregarAlimento());
            EliminarAlimentoCommand = new Command<ConsumoAlimento>(async (alimento) => await EliminarAlimento(alimento));
        }

        private async void CargarAlimentos()
        {
            var lista = await _bbdd.ObtenerAlimentosAsync();
            foreach (var alimento in lista)
                Alimentos.Add(alimento);
        }

        private async void CargarConsumo()
        {
            var consumo = await _bbdd.ObtenerConsumoDiarioPorFechaAsync(GlobalData.Instance.UsuarioId, _fechaActual);
            if (consumo != null)
            {
                foreach (var item in consumo.AlimentosConsumidos)
                    AlimentosConsumidos.Add(item);
            }

            
            RecalcularTotales();
        }


        private async Task AgregarAlimento()
        {
            if (AlimentoSeleccionado == null || CantidadGramos <= 0) return;

            var factor = CantidadGramos / 100.0;

            var consumo = new ConsumoAlimento
            {
                AlimentoId = AlimentoSeleccionado.Id,
                Nombre = AlimentoSeleccionado.Nombre,
                CantidadGramos = CantidadGramos,
                Calorias = (int)(AlimentoSeleccionado.Calorias * factor),
                Proteinas = (int)(AlimentoSeleccionado.Proteinas * factor),
                Carbohidratos = (int)(AlimentoSeleccionado.Carbohidratos * factor),
                Grasas = (int)(AlimentoSeleccionado.Grasas * factor),
                Fibra = (int)(AlimentoSeleccionado.Fibra * factor),
                ConsumoId = ObjectId.GenerateNewId() 
            };

            
            await _bbdd.AgregarAlimentoADiaAsync(GlobalData.Instance.UsuarioId, _fechaActual, consumo);
            AlimentosConsumidos.Add(consumo);
            CantidadGramos = 0;

            RecalcularTotales();
        }

        private void RecalcularTotales()
        {
            OnPropertyChanged(nameof(TotalCalorias));
            OnPropertyChanged(nameof(TotalProteinas));
            OnPropertyChanged(nameof(TotalCarbohidratos));
            OnPropertyChanged(nameof(TotalGrasas));
        }


        private async Task EliminarAlimento(ConsumoAlimento alimento)
        {
            if (alimento == null) return;
           
            await _bbdd.EliminarAlimentoDeDiaAsync(GlobalData.Instance.UsuarioId, _fechaActual, alimento);
         
            AlimentosConsumidos.Remove(alimento);

            RecalcularTotales();
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
