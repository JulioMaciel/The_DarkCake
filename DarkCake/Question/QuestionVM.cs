using AussieCake.Attempt;
using AussieCake.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class QuestVM : IQuest
    {
        public int Id { get; protected set; }

        public bool IsActive { get; protected set; }
        public string Text { get; protected set; }

        public Model Type { get; protected set; }

        public List<DateTry> Tries { get; set; }
        public int Corrects { get; protected set; }
        public int Incorrects { get; protected set; }
        public double Avg_week { get; protected set; }
        public double Avg_month { get; protected set; }
        public double Avg_all { get; protected set; }
        public DateTry LastTry { get; set; }
        public double Chance { get; protected set; }
        public double Chance_real { get; set; }
        public string Chance_toolTip { get; protected set; }

        public bool IsInit { get; protected set; }

        public double Index_show { get; set; }

        protected bool IsReal { get; set; } = false;

        protected QuestVM(int id, string text, bool isActive, Model type)
            : this(text, isActive, type)
        {
            Id = id;

            IsReal = true;
        }

        protected QuestVM(string text, bool isActive, Model type)
        {
            Text = text;
            IsActive = isActive;

            Type = type;

            Tries = new List<DateTry>();
        }

        public QuestVM()
        {
        }

        public virtual void LoadCrossData()
        {
            if (Type != Model.Pron)
                LoadTries();

            LoadChanceToAppear();
        }

        public void LoadTries()
        {
            Tries = new List<DateTry>();

            GetAttempts();

            LastTry = Tries.Any() ? Tries.Last() : null;

            Avg_week = Math.Round(GetAverageScoreByTime(7), 2);
            Avg_month = Math.Round(GetAverageScoreByTime(30), 2);
            Avg_all = Math.Round(GetAverageScoreByTime(2000), 2);
        }

        public void Disable()
        {
            IsActive = false;
        }

        public double GetAverageScoreByTime(int lastDays)
        {
            if (!Tries.Any())
                return 0;
            else
            {
                var filtered = Tries.Where(x => x.When >= (DateTime.Now.AddDays(-lastDays)));
                if (filtered.Any())
                    return filtered.Average(x => x.Score) 
                            * (Type != Model.P_Dit && Type != Model.P_FIB ? 10 : 1);
                else
                    return 0;
            }

        }

        private void GetAttempts()
        {
            var att = AttemptsControl.Get(Type);
            var attempts = att.Where(x => x.IdQuestion == Id);

            foreach (var item in attempts)
                Tries.Add(new DateTry(item.Score, item.When));
        }

        public void RemoveAllAttempts()
        {
            //var attToRemove = AttemptsControl.Get(Type).Where(q => q.IdQuestion == Id);
            //foreach (var att in attToRemove)
            //    AttemptsControl.Remove(att);
        }

        private void LoadChanceToAppear()
        {
            // peso 2
            var daysSince = LastTry != null ? DateTime.Now.Subtract(LastTry.When).Days : 100;
            var lastTry_score = 0;
            if (daysSince < 20 && daysSince >= 1)
                lastTry_score = daysSince / 2;
            else
                lastTry_score = 200;

            if (Avg_all >= 90 && Tries.Count > 1)
                lastTry_score = -20;

            if (Avg_all == 100 && Tries.Count > 2)
                lastTry_score = -40;

            if (Tries.Count == 1)
                lastTry_score = lastTry_score + 30;

            if (Tries.Count == 2)
                lastTry_score = lastTry_score + 15;

            if (Tries.Count == 3)
                lastTry_score = lastTry_score + 5;

            // peso 4
            var inv_avg = 40.0;
            if (daysSince <= 7)
                inv_avg -= ((20 * Avg_week) / 100) + ((15 * Avg_month) / 100) + ((05 * Avg_all) / 100);
            else if (daysSince <= 30)
                inv_avg -= ((30 * Avg_month) / 100) + ((10 * Avg_all) / 100);
            else if (daysSince <= 7)
                inv_avg -= ((40 * Avg_all) / 100);

            // peso 1
            var lastWasWrong = Tries != null && Tries.Any() ? (LastTry.Score == 100 ? 0 : 10) : 10;

            Chance = Math.Round(lastTry_score + inv_avg + lastWasWrong, 2);

            Chance_toolTip = inv_avg + " (inv_avg) -> " + (daysSince <= 7 ? "avg_week (20%) + avg_month (15%) + avg_all (5%)" :
                (daysSince <= 30 ? "avg_month (30%) + avg_all (10%)" :
                "avg_all (40%) + ")) + "\n";
            Chance_toolTip += lastTry_score + " (lastTry) + " + lastWasWrong + " (lastWrong)";

            if (LastTry != null && LastTry.When.Day == DateTime.Now.Day)
            {
                Chance_toolTip = "Question already completed today. Chance was " + Chance + ", but was set to " + Chance / 8;
            }

            if (Chance < 0)
                Chance = 0;
        }

        public virtual string ToText()
        {
            return string.Empty;
        }

        public string ToLudwigUrl()
        {
            var original = "https://ludwig.guru/s/";

            if (Text.Contains(' '))
            {
                var words = Text.Split(' ');
                original += string.Join("+", words);
            }
            else
            {
                original += Text;
            }

            return original;
        }

        public string ToBritannicaUrl()
        {
            if (Text.Contains(' '))
                return string.Empty;

            return "https://www.britannica.com/search?query=" + Text;
        }

        public List<string> GetSentences()
        {
            return QuestControl.GetSentences(this);
        }
    }

    public class DateTry
    {
        public int Score { get; set; }
        public DateTime When { get; set; }

        public DateTry(int score, DateTime when)
        {
            Score = score;
            When = when;
        }
    }
}
