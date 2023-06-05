using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Enumeration;
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

        private const string fileName = "To_Do_List_MyDB.txt";

        private string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

        private Uri iconUri;


        public MainWindow()
        {
            InitializeComponent();


            // nastavení iconky
            string path2 = path.Substring(0, path.Length - 44);
            path2 += "icon\\todo3.png";
            iconUri = new Uri(path2, UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

            // vytvoření nových prázdných seznamů
            dailyScore = new List<DailyScore>();
            list = new List<Item>();

            // načtení z databáze a následné načtení
            LoadData();
            refresh();

            // - asynchroní funce na čas v reálném čase
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                textRealDatum.Text = DateTime.Now.ToString();
            }, this.Dispatcher);

            TodayList.MouseDoubleClick += (s, e) => EditWin(s);
            TomorrowList.MouseDoubleClick += (s, e) => EditWin(s);

        }

        private void Vytvorit_BTN(object sender, RoutedEventArgs e) // otevře nové okno na vytvoření nového úkolu
        {
            VytvoreniUkolu VU = new VytvoreniUkolu(list);

            // nastavení iconky
            VU.Icon = BitmapFrame.Create(iconUri);

            // při zavření se sputí fuknce refresh()
            VU.Closed += (s, e) =>
            {
                refresh();
            };
            VU.ShowDialog();

        }  

        private void Button_Denik(object sender, RoutedEventArgs e) // otevře nové okno na vytvoření denníku
        {
            Diary D = new Diary(dailyScore, DateTime.Now);

            // nastavení iconky
            D.Icon = BitmapFrame.Create(iconUri);

            // při zavření se sputí fuknce refresh()
            D.Closed += (s, e) =>
            {
                refresh();
            };
            D.ShowDialog();
        }

        private void Button_Calendar(object sender, RoutedEventArgs e)// otevře nové okno na kalendář
        {
            Calendar C = new Calendar(dailyScore);

            // nastavení iconky
            C.Icon = BitmapFrame.Create(iconUri);

            // při zavření se sputí fuknce refresh()
            C.Closed += (s, e) =>
            {
                refresh();
            };
            C.ShowDialog();

        }

        private void Button_nastaveni(object sender, RoutedEventArgs e) // otevře nové okno na vytvoření nového úkolu
        {
            Settings S = new Settings();

            // nastavení iconky
            S.Icon = BitmapFrame.Create(iconUri);

            // při zavření se sputí fuknce refresh()
            S.Closed += (s, e) =>
            {
                refresh();
            };
            S.ShowDialog();

        }

        private void EditWin(object s)// otevře nové okno na editaci ukolu (double click)
        {
            Item? selected = (s as ListView)?.SelectedItem as Item;

            if (selected != null)
            {
                EditUkol EU = new EditUkol(selected);

                EU.Icon = BitmapFrame.Create(iconUri);

                EU.Closed += (s, e) =>
                {
                    refresh();
                    (s as ListView)?.UnselectAll();
                };
                EU.ShowDialog();
            }
        }

        private void Button_next(object sender, RoutedEventArgs e) // přepínání dne (další den)
        {
            vyberDnu += 1;
            refresh();
        }

        private void Button_previous(object sender, RoutedEventArgs e) // přepínání dne (minulý den)
        {
            vyberDnu -= 1;
            refresh();
        }

        /// <summary>
        /// funkce na obnovu všech dat a uložení 
        /// </summary>
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

            textDailyScore.Text = ScoreRefresh(DateTime.Now)[0].ToString();

            DailyScoreRefresh();
            SaveData();
        }
        /// <summary>
        /// obnova seznamu dailyScore + přidávání nových itemů do dailyScore
        /// </summary>
        private void DailyScoreRefresh()
        {
            List<int> ints;

            int i2;
            bool b = true;
            DateTime date;

            // vytvoření nových pokud je potřeba nebo pokud chybí
            foreach (Item item in list)
            {
                foreach (DailyScore daily in dailyScore)
                {
                    if (DateTime.Parse(daily.Day.ToString("d")) == DateTime.Parse(item.Dokdy))
                    {
                        b = false;
                    }
                }

                if (b)
                {
                    date = DateTime.Parse(item.Dokdy);
                    ints = ScoreRefresh(date);
                    dailyScore.Add(new DailyScore(date, ints[0], ints[1], ints[2], " "));
                }

                b = true;
            }
            // seřadí podle datumu
            dailyScore = dailyScore.OrderBy(x => x.Day.TimeOfDay).ToList();

            // obnoví stávající
            for (int i = dailyScore.Count() - 1; i > -1 ; i--)
            {
                i2 = dailyScore.Count() - i - 1;
                ints = ScoreRefresh(dailyScore[i2].Day);

                // if (dailyScore[i2].Score < ints[0])
                dailyScore[i2].Score = ints[0];

                if (dailyScore[i2].NumberOfTasks < ints[1])
                    dailyScore[i2].NumberOfTasks = ints[1];

                if (dailyScore[i2].NumCompletedTasks < ints[2])
                    dailyScore[i2].NumCompletedTasks = ints[2];
            }
        }

        /// <summary>
        /// získání dat z hlavního listu kde najde všechny ukoly v zadaném datumu "d" a vrátí [score, počet úkolů, počet hotových úkolů] v danám datumu
        /// </summary>
        private List<int> ScoreRefresh(DateTime d) // získání dat z hlavní ho listu (score, počet úkolů a počet hotových úkolů)
        {
            List<int> seznam = new List<int>();

            int score = 0;
            int taskscomplete = 0;
            int tasks = 0;
            foreach (Item item in list)
            {

                if (item.Dokdy == d.ToString("d"))
                {

                    if (item.CheckBox == true)
                    {
                        score += item.Difficult;
                        taskscomplete++;
                    }

                    if (DateTime.Parse(item.Dokdy) < DateTime.Parse(DateTime.Now.ToString("d")))
                    {
                        if (item.CheckBox == false)
                        {
                            if (item.Difficult == 0)
                                continue;
                            
                            else if (item.Difficult == 100)
                                score += -1;
                       
                            else
                                score += item.Difficult - 100;
                        }
                    }
                    tasks++;
                }
            }

            seznam.Add(score);
            seznam.Add(tasks);
            seznam.Add(taskscomplete);

            return seznam;
        }
        /// <summary>
        /// funkce na uložení (v budoucnu jako vlastní třída "Save_Load")
        /// </summary>
        private void SaveData()
        {
            string res0 = string.Join("~~", dailyScore) + Environment.NewLine;
            string res = string.Join("~~", list) + Environment.NewLine;

            string resFinal = res + "~~~" + res0;
            File.WriteAllText(path, sifra.Cipher(resFinal));
        }

        /// <summary>
        /// funkce na načtení dat ze souboru (v budoucnu jako vlastní třída "Save_Load")
        /// </summary>
        private void LoadData()
        {
            string[] data;

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(fileName)) { }
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

        /// <summary>
        /// funkce vrátí uri trasu na iconku
        /// </summary>
        public Uri getPathIcon()
        {
            return iconUri;
        }
        /// <summary>
        /// ošetření toho že když se vypne aplikace, tak se sama uloží
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            refresh();
            base.OnClosing(e);
        }
    }
}
