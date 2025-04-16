using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinal.Modelos;

namespace ProyectoFinal.ViewsModel
{
    public class AlimentosViewModel : INotifyPropertyChanged
    {
        private readonly BBDD _bbdd = new BBDD();

        public ObservableCollection<Alimento> Alimentos { get; set; } = new ObservableCollection<Alimento>();

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged();
            }
        }

        public AlimentosViewModel()
        {
            CargarAlimentos();
        }

        private async void CargarAlimentos()
        {
            var lista = await _bbdd.ObtenerAlimentosAsync();

            if (lista != null && lista.Count > 0)
            {
                foreach (var alimento in lista)
                    Alimentos.Add(alimento);
            }
            else
            {
                Mensaje = "No hay alimentos disponibles.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
