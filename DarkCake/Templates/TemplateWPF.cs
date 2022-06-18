using AussieCake.Attempt;
using AussieCake.Question;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AussieCake.Templates
{
    public static class TemplateWPF
    {

        public static List<CellTemplate> BuildTemplate(List<IQuest> templateList, double percentageTextBox, StackPanel parent, List<int> skip = null)
        {
            parent.Children.Clear();

            var LineLenght = 140;
            if (percentageTextBox == 100)
                LineLenght = 100;
            else if (percentageTextBox == 80)
                LineLenght = 100;
            else if (percentageTextBox == 60)
                LineLenght = 110;
            else if (percentageTextBox == 40)
                LineLenght = 115;
            else if (percentageTextBox == 33)
                LineLenght = 117;
            else if (percentageTextBox == 20)
                LineLenght = 120;

            var templateLines = new List<StackPanel>();

            var cellList = new List<CellTemplate>();

            var actualLine = new StackPanel();
            templateLines.Add(actualLine);
            var actualLineLenght = 0.0;

            for (int i = 0; i < templateList.Count; i++)
            {
                var item = templateList[i];
                var nextItem = item;

                if (i != templateList.Count - 1)
                    nextItem = templateList[i + 1];

                if (item.Text == TemplateEssay.Paragraph)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                    continue;
                }

                actualLineLenght = actualLineLenght + item.Text.Length * 1.5 + 0.5;
                var willNextBreakLine = (actualLineLenght + nextItem.Text.Length * 1.5 + 1) > LineLenght;

                if (((item.Text != "," && item.Text != "." && item.Text != ")" && item.Text != "]") &&
                    actualLineLenght >= LineLenght) ||
                    (willNextBreakLine && (item.Text == "[" || item.Text == "(")))
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                }

                var cell = new CellTemplate(item, skip);
                cellList.Add(cell);
                actualLine.Children.Add(cell.Stk);
            }

            var avaliable_cells = cellList.Where(w => w.IsAvailableToTurnTxtOn());
            int quantToAnswer = Convert.ToInt16(((double)avaliable_cells.Count() / 100) * percentageTextBox);

            if (percentageTextBox != 33)
            {
                for (int i = 0; i < quantToAnswer; i++)
                {
                    var rnd = cellList.Where(c => c.IsAvailableToTurnTxtOn()).PickRandom();
                    rnd.TurnTxtOn();
                }
            }
            else
            {
                // 35% most erroneous + 15% random -> remove 17 random

                foreach (var item in cellList)
                    item.Quest.LoadCrossData();

                // aqui o Avg All ta chegando zerado... tem que dar Load em cada item quest
                // such as item.Quest.LoadCrossData();
                // depois que funcionar, levar o codigo para essay e sum spk blabla

                var top25worst = cellList.OrderByDescending(x => x.Quest.Avg_all).Where(c => c.IsAvailableToTurnTxtOn()).Take(cellList.Count / 4);
                var more15rnd = new List<CellTemplate>();

                var fifteenPercent = Convert.ToInt16(((double)avaliable_cells.Count() / 100) * 15);
                for (int i = 0; i < fifteenPercent; i++)
                {
                    var rnd = cellList.Where(c => c.IsAvailableToTurnTxtOn() && !top25worst.Any(x => x == c)).PickRandom();
                    more15rnd.Add(rnd);
                }

                var chosen = top25worst.Union(more15rnd).GetRandomPercent(66);
                foreach (var item in chosen.OrderBy(x => x.Quest.Avg_all))
                {
                    Console.WriteLine(item.Quest.Text + ": " + item.Quest.Avg_all);
                    item.TurnTxtOn();
                }
                Console.WriteLine("---");
            }

            foreach (var stk in templateLines)
            {
                stk.Orientation = Orientation.Horizontal;
                stk.VerticalAlignment = VerticalAlignment.Center;
                parent.Children.Add(stk);
            }

            return cellList;
        }

        public static List<CellTemplate> BuildInitTemplate(StackPanel parent, List<IQuest> templateList)
        {
            parent.Children.Clear();

            var LineLenght = 140;

            var templateLines = new List<StackPanel>();

            var cellList = new List<CellTemplate>();

            var actualLine = new StackPanel();
            templateLines.Add(actualLine);
            var actualLineLenght = 0.0;

            for (int i = 0; i < templateList.Count; i++)
            {
                var item = templateList[i];
                var nextItem = item;

                if (i != templateList.Count - 1)
                    nextItem = templateList[i + 1];

                if (item.Text == TemplateEssay.Paragraph)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                    continue;
                }

                actualLineLenght = actualLineLenght + item.Text.Length * 1.5 + 0.5;
                var willNextBreakLine = (actualLineLenght + nextItem.Text.Length * 1.5 + 1) > LineLenght;

                if (((item.Text != "," && item.Text != ".") &&
                    actualLineLenght >= LineLenght) || willNextBreakLine)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                }

                var cell = new CellTemplate(item, null);
                cellList.Add(cell);
                actualLine.Children.Add(cell.Stk);
            }

            foreach (var cell in cellList)
            {
                if (cell.Quest.IsInit)
                    cell.TurnTxtOn();
            }

            foreach (var stk in templateLines)
            {
                stk.Orientation = Orientation.Horizontal;
                stk.VerticalAlignment = VerticalAlignment.Center;
                parent.Children.Add(stk);
            }

            return cellList;
        }

        public static List<CellTemplate> BuildStressedTemplate(StackPanel parent)
        {
            parent.Children.Clear();

            var LineLenght = 140;

            var templateLines = new List<StackPanel>();

            var cellList = new List<CellTemplate>();

            var actualLine = new StackPanel();
            templateLines.Add(actualLine);
            var actualLineLenght = 0.0;

            var templateList = TemplateDescImg.Words;

            for (int i = 0; i < templateList.Count; i++)
            {
                var item = templateList[i];
                var nextItem = item;

                if (i != templateList.Count - 1)
                    nextItem = templateList[i + 1];

                if (item.Text == TemplateEssay.Paragraph)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                    continue;
                }

                actualLineLenght = actualLineLenght + item.Text.Length * 1.5 + 0.5;
                var willNextBreakLine = (actualLineLenght + nextItem.Text.Length * 1.5 + 1) > LineLenght;

                if (((item.Text != "," && item.Text != ".") &&
                    actualLineLenght >= LineLenght) || willNextBreakLine)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                }

                var cell = new CellTemplate(item, null);
                cellList.Add(cell);
                actualLine.Children.Add(cell.Stk);
            }

            foreach (var cell in cellList)
            {
                var descImgVm = cell.Quest as DescImgVM;
                if (descImgVm.IsStressed)
                    cell.TurnTxtOn();
            }

            foreach (var stk in templateLines)
            {
                stk.Orientation = Orientation.Horizontal;
                stk.VerticalAlignment = VerticalAlignment.Center;
                parent.Children.Add(stk);
            }

            return cellList;
        }

        public static void ShowScoredTemplate(List<CellTemplate> cellList, int lastDays)
        {
            foreach (var item in cellList)
            {
                item.Quest.LoadCrossData();

                if (item.Quest.Tries.Count != 0)
                {
                    var avg = item.Quest.GetAverageScoreByTime(lastDays);
                    item.Lbl.Foreground = UtilWPF.GetAvgColor(avg);

                    var tries_period = item.Quest.Tries.Where(x => x.When >= (DateTime.Now.AddDays(-lastDays)));
                    var corrects_period = tries_period.Where(x => x.Score == 10);
                    item.Lbl.ToolTip = corrects_period.Count() + " / " + tries_period.Count();
                }
            }
        }

        public static (string, int) CheckAnswers(List<CellTemplate> cellList, List<int> ignore = null)
        {
            var total = 0;
            var corrects = 0;

            if (ignore != null)
                cellList = cellList.Where(c => !ignore.Contains(c.Quest.Id)).ToList();

            foreach (var item in cellList)
            {
                if (item.Txt != null)
                {
                    item.Txt.IsReadOnly = true;
                    var answer = string.Empty;

                    if (!item.Txt.IsEmpty())
                        answer = item.Txt.Text;

                    var isCorrect = answer.Equals(item.Quest.Text, StringComparison.OrdinalIgnoreCase);
                    var score = 0;

                    if (isCorrect)
                    {
                        score = 10;
                        item.Txt.Background = Brushes.LightGreen;
                        corrects = corrects + 1;
                    }
                    else
                    {
                        item.Txt.Background = Brushes.LightSalmon;
                        item.Txt.ToolTip = item.Quest.Text;
                    }

                    var vm = new AttemptVM(item.Quest.Id, score, DateTime.Now, item.Quest.Type);
                    AttemptsControl.Insert(vm);
                    total = total + 1;
                }
            }

            var percent_corrects = 0;
            if (corrects != 0)
                percent_corrects = (int)Math.Round((double)(100 * corrects) / total);

            return (percent_corrects + "% (" + corrects + " corrects)", percent_corrects);
        }
    }

    public class CellTemplate
    {
        public IQuest Quest { get; private set; }

        public TextBlock Lbl { get; private set; }
        public TextBox Txt { get; private set; }

        public StackPanel Stk { get; private set; }

        private List<int> Skip { get; set; }

        static List<string> TheseSynonyms = new List<string>() { /*"it", */ "that" /*, "this"*/ };

        public CellTemplate(IQuest quest, List<int> skip = null)
        {
            Quest = quest;
            var word = quest.Text;
            Stk = new StackPanel();

            if (skip != null)
                Skip = skip;
            else
                Skip = new List<int>();

            Lbl = new TextBlock();
            Lbl.Text = quest.Text;
            Lbl.Margin = new Thickness(0, 0, 3, 1);
            Stk.Children.Add(Lbl);
        }

        public void TurnTxtOn()
        {
            Stk.Children.Clear();

            var word = Quest.Text;

            if (IsWordAvailable(word))
            {
                Txt = new TextBox();
                Txt.Width = word.Length * 5 + 26;
                Txt.Margin = new Thickness(0, 0, 3, 1);
                Txt.PreviewKeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Space)
                    {
                        var focusDirection = FocusNavigationDirection.Next;
                        var request = new TraversalRequest(focusDirection);
                        var elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                            elementWithFocus.MoveFocus(request);
                        e.Handled = true;
                    }
                    else if (e.Key == Key.OemQuotes)
                    {
                        var focusDirection = FocusNavigationDirection.Previous;
                        var request = new TraversalRequest(focusDirection);
                        var elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                            elementWithFocus.MoveFocus(request);
                        e.Handled = true;
                    }
                };
                Stk.Children.Add(Txt);
            }
        }

        public bool IsAvailableToTurnTxtOn()
        {
            if (Skip.Contains(Quest.Id))
                return false;

            var word = Quest.Text;
            if (IsWordAvailable(word) && Txt == null)
                return true;

            return false;
        }

        private bool IsWordAvailable(string word)
        {
            if (TheseSynonyms.Contains(word, StringComparer.OrdinalIgnoreCase))
                return false;

            if (word.IsLettersOnly() || word.Contains('-') || 
                word.Contains('+') || word.Contains('%') || word.Contains("[...]"))
                return true;

            return false;
        }
    }
}
