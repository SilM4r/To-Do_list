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
    /// Interakční logika pro EditUkol.xaml
    /// </summary>
    public partial class EditUkol : Window
    {
        Item i;
        public EditUkol(Item i)
        {
            this.i = i;
            InitializeComponent();
            textNazev.Text = i.Titulek;
            textObsah.Text = i.Obsah;
            sObtiznost.Value = i.Difficult;
            checkBox.IsChecked = i.CheckBox;
            textdatum.Text = i.Dokdy.ToString();
            textdatumV.Text = i.Date.ToString();


            if (i.CheckBox == true || DateTime.Parse(i.Dokdy) < DateTime.Now.AddDays(-1))
            {
                textNazev.IsReadOnly = true;
                textdatum.IsReadOnly = true;
                textObsah.IsReadOnly = true;
                textObt.IsReadOnly = true;
                sObtiznost.IsEnabled = false;
            }
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            if (i.CheckBox == false)
            {
                if (i.Dokdy == DateTime.Now.ToString("d") || DateTime.Parse(i.Dokdy) < DateTime.Now.AddDays(-1))
                {
                    i.CheckBox = true;
                    i.Titulek = textNazev.Text.Replace('~', ' ');
                    i.Obsah = textObsah.Text.Replace('~', ' ');
                    i.Difficult = Convert.ToInt32(sObtiznost.Value);
                    this.Close();
                }
            }
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            i.Titulek = textNazev.Text.Replace('~', ' ');
            i.Obsah = textObsah.Text.Replace('~', ' ');
            i.Difficult = Convert.ToInt32(sObtiznost.Value);


            string[] formats = { "d.M.yyyy" };
            DateTime dateValue;

            if (DateTime.TryParseExact(textdatum.Text, formats, new CultureInfo("en-ES"), DateTimeStyles.None, out dateValue))
            {
                if (dateValue >= DateTime.Now.AddDays(-1))
                {
                    i.Dokdy = dateValue.ToString("d");
                    this.Close();
                }
            }
            else
                MessageBox.Show("Nepodporovaný formát datumu, prosím pište jenom ve formátu '1.1.2022'");
        }
    }
}
