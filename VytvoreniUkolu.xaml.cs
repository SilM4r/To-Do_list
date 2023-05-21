using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace To_Do_list
{
    /// <summary>
    /// Interakční logika pro VytvoreniUkolu.xaml
    /// </summary>
    /// 

    //// public Item(string titulek, string obsah, DateTime dokdy, DateTime date, int difficult)
    ///MessageBox.Show("Došlo k nějaké chybě");
    public partial class VytvoreniUkolu : Window
    {
        public string? titulek, obsah, dokdy;
        public int obt;
        public DateTime date;

        public bool konec = true;


        List<Item> i;
        public VytvoreniUkolu(List<Item> i)
        {
            this.i = i;
            InitializeComponent();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {

            titulek = textNazev.Text;
            obsah = textObsah.Text;
            dokdy =  textdatum.Text;

            date = DateTime.Now;
            obt = Convert.ToInt32(sObtiznost.Value);

            if (!string.IsNullOrEmpty(titulek))
            {
                if (string.IsNullOrEmpty(dokdy))
                    PridaniItemu(date.ToString("d"));

                else if (dokdy.ToLower() == "dnešek")
                    PridaniItemu(date.ToString("d"));

                else if (dokdy.ToLower() == "dneska")
                    PridaniItemu(date.ToString("d"));

                else if (dokdy.ToLower() == "dnes")
                    PridaniItemu(date.ToString("d"));

                else if (dokdy.ToLower() == "zítřka")
                    PridaniItemu(date.AddDays(1).ToString("d"));

                else if (dokdy.ToLower() == "zítřek")
                    PridaniItemu(date.AddDays(1).ToString("d"));

                else if (dokdy.ToLower() == "zítra")
                    PridaniItemu(date.AddDays(1).ToString("d"));

                if (konec)
                {
                    string[] formats = { "d.M.yyyy" };
                    DateTime dateValue;

                    if (DateTime.TryParseExact(dokdy, formats, new CultureInfo("en-ES"), DateTimeStyles.None, out dateValue))
                    {
                        if (dateValue >= date.AddDays(-1))
                            PridaniItemu(dateValue.ToString("d"));
                    }
                    else
                        MessageBox.Show("Nepodporovaný formát datumu, prosím pište jenom ve formátu '1.1.2022'");
                }
            }           
        }

        private void PridaniItemu(string datum)
        {
            Item p = new Item(titulek.Replace('~', ' '), obsah.Replace('~', ' '), datum, date.ToString(), obt, false);
            i.Add(p);
            Close();
            konec = false;

            MessageBox.Show("Nový úkol byl úspěšně přidán");
        }

    }
}
