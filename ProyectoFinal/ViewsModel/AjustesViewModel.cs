using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ProyectoFinal.ViewsModel
{
    public class AjustesViewModel : INotifyPropertyChanged
    {
        private bool _temaClaroSeleccionado;
        private bool _temaOscuroSeleccionado;

        public bool TemaClaroSeleccionado
        {
            get => _temaClaroSeleccionado;
            set
            {
                if (_temaClaroSeleccionado != value)
                {
                    _temaClaroSeleccionado = value;
                    if (value)
                        CambiarTema(AppTheme.Light);
                    OnPropertyChanged();
                }
            }
        }

        public bool TemaOscuroSeleccionado
        {
            get => _temaOscuroSeleccionado;
            set
            {
                if (_temaOscuroSeleccionado != value)
                {
                    _temaOscuroSeleccionado = value;
                    if (value)
                        CambiarTema(AppTheme.Dark);
                    OnPropertyChanged();
                }
            }
        }

        public AjustesViewModel()
        {
            // Inicializa los valores según el tema actual
            TemaClaroSeleccionado = Application.Current.UserAppTheme == AppTheme.Light;
            TemaOscuroSeleccionado = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void CambiarTema(AppTheme nuevoTema)
        {
            Application.Current.UserAppTheme = nuevoTema;

            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            mergedDictionaries.Clear();

            if (nuevoTema == AppTheme.Dark)
                mergedDictionaries.Add(new Resources.Styles.TemaOscuro());
            else
                mergedDictionaries.Add(new Resources.Styles.TemaClaro());

           
            TemaClaroSeleccionado = nuevoTema == AppTheme.Light;
            TemaOscuroSeleccionado = nuevoTema == AppTheme.Dark;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
