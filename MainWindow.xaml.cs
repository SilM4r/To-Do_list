using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace To_Do_list
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Item> list;
        List<Item> todayList;
        List<Item> anotherDayList;

        List<DailyScore> dailyScore;

        SimpleSifra sifra = new SimpleSifra();

        private int vyberDnu = 1;

        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "To_Do_List_MyDB.txt");

        public MainWindow()
        {
            InitializeComponent();
            dailyScore = new List<DailyScore>();
            list = new List<Item>();
            LoadData();
            refresh();

            DateTime dateTime = DateTime.Now;
            DateTime dateTime1 = DateTime.Now;

            //MessageBox.Show((dateTime1 - dateTime).Days.ToString());

            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                textRealDatum.Text = DateTime.Now.ToString();
            }, this.Dispatcher);

            TodayList.MouseDoubleClick += (s, e) => EditWin(s);
            TomorrowList.MouseDoubleClick += (s, e) => EditWin(s);

        }

        private void Vytvorit_BTN(object sender, RoutedEventArgs e)
        {
            VytvoreniUkolu VU = new VytvoreniUkolu(list);

            VU.Closed += (s, e) =>
            {
                refresh();
            };
            VU.ShowDialog();

        }
        private void Button_Denik(object sender, RoutedEventArgs e)
        {
            Diary D = new Diary(dailyScore, DateTime.Now);

            D.Closed += (s, e) =>
            {
                refresh();
            };
            D.ShowDialog();
        }
        private void Button_Calendar(object sender, RoutedEventArgs e)
        {
            Calendar C = new Calendar(dailyScore);

            C.Closed += (s, e) =>
            {
                refresh();
            };
            C.ShowDialog();

        }

        private void EditWin(object s)
        {
            Item? selected = (s as ListView)?.SelectedItem as Item;
            EditUkol EU = new EditUkol(selected);
            EU.Closed += (s, e) =>
            {
                refresh();
                (s as ListView)?.UnselectAll();
            };
            EU.ShowDialog();
        }

        private void refresh()
        {
            TodayList.ItemsSource = null;
            TomorrowList.ItemsSource = null;
            todayList = new List<Item>();
            anotherDayList = new List<Item>();

            foreach ( Item item in list)
            {
                if (item.Dokdy == DateTime.Now.ToString("d")) 
                {
                    todayList.Add(item);
                }

                if (item.Dokdy == DateTime.Now.AddDays(vyberDnu).ToString("d"))
                {
                    anotherDayList.Add(item);
                }
            }
            TodayList.ItemsSource = todayList;
            TomorrowList.ItemsSource = anotherDayList;

            anotherDay.Text = DateTime.Now.AddDays(vyberDnu).ToString("d");

            textDailyScore.Text = ScoreRefresh(0)[0].ToString();

            DailyScoreRefresh();
            SaveData();
        }

        private void Button_next(object sender, RoutedEventArgs e)
        {
            vyberDnu += 1;
            refresh();
        }

        private void Button_previous(object sender, RoutedEventArgs e)
        {
            vyberDnu -= 1;
            refresh();
        }


        private void SaveData()
        {
            string res0 = string.Join("~~", dailyScore) + Environment.NewLine;
            string res = string.Join("~~", list) + Environment.NewLine;

            string resFinal = res + "~~~" + res0;
            File.WriteAllText(path, sifra.Cipher(resFinal));
        }

        private void LoadData()
        {
            string[] data;

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create("To_Do_List_MyDB.txt")){ }
            }

            if (File.ReadAllText(path) == string.Empty)
            {
                list = new List<Item>();
                dailyScore = new List<DailyScore>();
                return;
            }

            string[] rawData = sifra.Cipher(File.ReadAllText(path)).Split("~~~");

            if (rawData[0] != "")
            {
                foreach (string Items in rawData[0].Split("~~"))
                {
                    data = Items.Split(";~");
                    Item p = new Item(data[0], data[1], data[2], data[3], Convert.ToInt16(data[4]), bool.Parse(data[5]));
                    list.Add(p);
                }
            }

            if (rawData[1] != "")
            {
                foreach (string Items in rawData[1].Split("~~"))
                {
                    data = Items.Split(";~");
                    DailyScore p1 = new DailyScore(DateTime.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), data[4]);
                    dailyScore.Add(p1);
                }
            }
        }

        private void DailyScoreRefresh()
        {
            List<int> ints;
            string den = "0";
            int numDay;

            int i2;

            ///// (1) nemusí být
            foreach (Item item in list) {

                if (den == "0")
                {
                    den = item.Dokdy;
                }

                else if (DateTime.Parse(item.Dokdy) < DateTime.Parse(den)) 
                {
                    den = item.Dokdy;
                }
            }

            if (den == "0")
            {
                den = DateTime.Now.ToString();
            }

            numDay = Convert.ToInt16((DateTime.Now - DateTime.Parse(den)).Days);

            ///// (1)

            if (dailyScore.Any())
            {
                DailyScore lastitem = dailyScore.Last();
                numDay = Convert.ToInt16(DateTime.Now.Day - lastitem.Day.Day);

            }

            //MessageBox.Show(string.Join("~~", dailyScore));
            //MessageBox.Show(numDay.ToString());

            // vytvoří nové pokud je potřeba
            for (int i = numDay; i >= 1; i--)
            {
                ints = ScoreRefresh(-i);
                dailyScore.Add(new DailyScore(DateTime.Now.AddDays(-i+1), ints[0], ints[1], ints[2], " "));
            }

            // obnoví stávající
            for (int i = dailyScore.Count() - 1; i > -1 ; i--)
            {
                ints = ScoreRefresh(-i);

                i2 = dailyScore.Count() - i - 1;

                if (dailyScore[i2].Score < ints[0])
                    dailyScore[i2].Score = ints[0];

                if (dailyScore[i2].NumberOfTasks < ints[1])
                    dailyScore[i2].NumberOfTasks = ints[1];

                if (dailyScore[i2].NumCompletedTasks < ints[2])
                    dailyScore[i2].NumCompletedTasks = ints[2];
            }
        }

        private List<int> ScoreRefresh(int i)
        {
            List<int> seznam = new List<int>();

            int score = 0;
            int taskscomplete = 0;
            int tasks = 0;
            foreach (Item item in list)
            {
                if (item.Dokdy == DateTime.Now.AddDays(i).ToString("d"))
                {
                    if (item.CheckBox == true)
                    {
                        score += item.Difficult;
                        taskscomplete++;

                    }
                    tasks++;
                }
            }
            seznam.Add(score);
            seznam.Add(tasks);
            seznam.Add(taskscomplete);

            return seznam;
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            refresh();
            base.OnClosing(e);
        }

        
    }
}
