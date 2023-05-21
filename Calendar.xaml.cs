using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interakční logika pro Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        private int vyberDnu = -3;

        List<DailyScore> dailyScore;

        public Calendar(List<DailyScore> dailyScore)
        {
            InitializeComponent();

            this.dailyScore = dailyScore;

            refresh(dailyScore);


        }

        private void ButtonD1_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string buttonName = clickedButton.Name;

            char lastCharacter = buttonName[buttonName.Length - 1];

            Diary D = new Diary(dailyScore, DateTime.Now.AddDays((vyberDnu) + lastCharacter - '0' -1));

            D.ShowDialog();


        }

        private void Button_next(object sender, RoutedEventArgs e)
        {
            vyberDnu += 1;
            refresh(this.dailyScore);
        }

        private void Button_previous(object sender, RoutedEventArgs e)
        {
            vyberDnu -= 1;
            refresh(this.dailyScore);
        }

        private void refresh(List<DailyScore> dailyScore)
        {
            bool check = false;
            TextBlock[,] seznamDnu = { { TextDatumD1, TextScoreD1, TextUkolyD1, TextSplnenoD1 }, { TextDatumD2, TextScoreD2, TextUkolyD2, TextSplnenoD2 }, { TextDatumD3, TextScoreD3, TextUkolyD3, TextSplnenoD3 }, { TextDatumD4, TextScoreD4, TextUkolyD4, TextSplnenoD4 }, { TextDatumD5, TextScoreD5, TextUkolyD5, TextSplnenoD5 }, { TextDatumD6, TextScoreD6, TextUkolyD6, TextSplnenoD6 }, { TextDatumD7, TextScoreD7, TextUkolyD7, TextSplnenoD7 }, };

            for (int i = 0; i < 7; i++)
            {
                foreach (DailyScore item in dailyScore)
                {
                    if (item.Day.ToString("d") == DateTime.Now.AddDays(vyberDnu+i).ToString("d"))
                    {
                        check = true;
                        seznamDnu[i, 0].Text = DateTime.Now.AddDays(vyberDnu + i).ToString("M");
                        seznamDnu[i, 1].Text = $"Score: {item.Score}";
                        seznamDnu[i, 2].Text = $"Ukoly: {item.NumberOfTasks}";
                        seznamDnu[i, 3].Text = $"Ukoly: {item.NumCompletedTasks}";
                    }
                }

                if (check)
                    check = false;
                else
                {
                    seznamDnu[i, 0].Text = DateTime.Now.AddDays(vyberDnu + i).ToString("M");
                    seznamDnu[i, 1].Text = $"Score: 0";
                    seznamDnu[i, 2].Text = $"Ukoly: 0";
                    seznamDnu[i, 3].Text = $"Ukoly: 0";
                }
            }
        }

        private void Button_back(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
