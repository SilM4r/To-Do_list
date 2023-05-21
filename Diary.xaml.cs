using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace To_Do_list
{
    /// <summary>
    /// Interakční logika pro Diary.xaml
    /// </summary>
    public partial class Diary : Window
    {
        List<DailyScore> d;

        DailyScore today;
        public Diary(List<DailyScore> d, DateTime date)
        {
            this.d = d;
            InitializeComponent();

            textObsah.Text = "";
            foreach (DailyScore item in d)
            {
                if (item.Day.ToString("d") == date.ToString("d"))
                {
                    textObsah.Text = item.Diary;
                    today = item;
                }
            }

            if(date.Day != DateTime.Now.Day)
            {
                buttonSave.Visibility = Visibility.Collapsed;
                textObsah.IsReadOnly = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (buttonSave.Visibility == Visibility.Visible)
                today.Diary = textObsah.Text;
            this.Close();
            return;
                
            
        }
    }
}
