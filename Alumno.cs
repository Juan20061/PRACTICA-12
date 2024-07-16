using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataBinding3716370
{
    public class Alumno : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string nombre = string.Empty;

        public string Nombre
        {
            get => nombre;
            set
            {
                if (nombre == value)
                    return;
                nombre = value;
                onPropertyChamged(nameof(Nombre));
                onPropertyChamged(nameof(MostrarNombre));
            }
        }

        public string MostrarNombre => $"Nombre ingresado:{Nombre}";

        void onPropertyChamged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        
        
    }
}
