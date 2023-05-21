using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace To_Do_list
{
    public class DailyScore
    {

        public DateTime Day { get; set; }

        public int Score { get; set; }

        public int NumberOfTasks { get; set; }

        public int NumCompletedTasks { get; set; }

        public string Diary { get; set; }

        public DailyScore(DateTime day, int score,int numberOfTasks, int numCompletedTasks, string diary) 
        {
            Day = day;
            Score = score;
            NumberOfTasks = numberOfTasks;
            NumCompletedTasks = numCompletedTasks;
            Diary = diary;
        }

        public override string ToString()
        {
            return $"{Day};~{Score};~{NumberOfTasks};~{NumCompletedTasks};~{Diary}";
        }
    }
}