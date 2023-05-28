using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace To_Do_list
{
    public class Item : INotifyPropertyChanged
    {
        string titulek, obsah, date, dokdy;
        bool checkBox;
        int difficult;

        public bool CheckBox
        {
            get => checkBox;
            set
            {
                checkBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CheckBox"));
            }
        }

        public string Date
        {
            get => date;
            set
            {
                date = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
            }
        }
        public string Dokdy
        {
            get => dokdy;
            set
            {
                dokdy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dokdy"));
            }
        }

        public int Difficult
        {
            get => difficult;
            set 
            { 
                difficult = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Difficult"));
            } 
        }

        public string Titulek
        {
            get => titulek;
            set
            {
                titulek = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Titulek"));
            }
        }
        public string Obsah
        {
            get => obsah;
            set
            {
                obsah = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Obsah"));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Item(string titulek, string obsah, string dokdy, string date, int difficult, bool checkBox)
        {
            Dokdy = dokdy;
            Titulek = titulek;
            Obsah = obsah;
            Date = date;
            Difficult = difficult;
            CheckBox = checkBox;
        }

        public override string ToString()
        {
            return $"{Titulek};~{Obsah};~{Dokdy};~{Date};~{Difficult};~{CheckBox}";
        }

    }
}
