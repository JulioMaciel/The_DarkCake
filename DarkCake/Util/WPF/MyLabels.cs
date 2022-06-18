using AussieCake.Challenge;
using AussieCake.Question;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AussieCake.Util.WPF
{
    public static class MyLbls
    {
        public static Label AvgScore(Label reference, int row, int Column, Grid parent, IQuest quest, int duration, bool useColours = true)
        {
            var isWeek = duration <= 7;
            var isMonth = duration <= 30;

            var avg = isWeek ? quest.Avg_week : (isMonth ? quest.Avg_month : quest.Avg_all);
            var content = avg + "% ";// + (isWeek ? "(w)" : isMonth ? "(m)" : ""); 

            var lbl = Get(reference, row, Column, parent, content);

            if (useColours)
                lbl.Foreground = UtilWPF.GetAvgColor(avg);

            lbl.ToolTip = isWeek ? "week" : isMonth ? "month" : "all";

            return lbl;
        }

        public static Label Tries(Label reference, int row, int Column, Grid parent, IQuest quest)
        {
            var content = quest.Tries.Count > 0 ? quest.Tries.Count + " tries" : "New";

            return Get(reference, row, Column, parent, content);
        }

        public static Label LastTry(Label reference, int row, int Column, Grid parent, IQuest quest)
        {
            var content = string.Empty;
            if (quest.Tries.Any())
            {
                var last = DateTime.Now.Subtract(quest.LastTry.When).Days;
                content = last != 0 ? last + "d ago" : "Today";
            }
            else
                content = "Never";

            return Get(reference, row, Column, parent, content);
        }

        public static Label Chance(Label reference, int row, int Column, Grid parent, IQuest quest)
        {
            var lbl = Get(reference, row, Column, parent, quest.Chance + " (" + Math.Round(quest.Chance_real, 2) + "%)");
            lbl.ToolTip = quest.Chance_toolTip;

            lbl.MouseEnter += new MouseEventHandler((source, e) => lbl.Foreground = Brushes.DarkRed);
            lbl.MouseLeave += new MouseEventHandler((source, e) => lbl.Foreground = Brushes.Black);

            return lbl;
        }

        public static Label Header(Label reference, int row, int Column, Grid parent, SortLbl sort, Control control, IFilter filter, StackPanel stk_items)
        {
            var lbl = Get(reference, row, Column, parent, sort.ToDesc());

            lbl.MouseEnter += new MouseEventHandler((source, e) => lbl.Foreground = Brushes.DarkRed);
            lbl.MouseLeave += new MouseEventHandler((source, e) => lbl.Foreground = Brushes.Black);

            lbl.ToolTip = "Left click to sort";

            if (control is TextBox)
            {
                lbl.MouseRightButtonDown += (source, e) => (control as TextBox).Text = string.Empty;
                lbl.ToolTip += "; Right to erase; Double [txt] to paste.";
            }

            lbl.MouseLeftButtonDown += (source, e) => filter.SetSort(sort, stk_items);

            //if (sort == SortLbl.Answer || sort == SortLbl.Voc_comp2)
            //    Grid.SetColumnSpan(lbl, 2);

            return lbl;
        }

        public static Label Get(Label reference, int row, int Column, Grid parent, string content)
        {
            var lbl = Get(reference, content);
            UtilWPF.SetGridPosition(reference, row, Column, parent);

            return reference;
        }

        public static Label Get(Label reference, StackPanel parent, string content)
        {
            var lbl = Get(reference, content);

            if (!parent.Children.Contains(lbl))
                parent.Children.Add(lbl);

            return lbl;
        }

        public static Label Get(Label reference, string content)
        {
            reference.Content = content;
            reference.VerticalContentAlignment = VerticalAlignment.Center;
            reference.HorizontalContentAlignment = HorizontalAlignment.Center;
            reference.Margin = new Thickness(1, 0, 1, 0);

            return reference;
        }

        public static Label Get(int row, int Column, Grid parent, string content)
        {
            return Get(new Label(), row, Column, parent, content);
        }

        public static Label Chal_answer(Label reference, StackPanel parent, string content)
        {
            var lbl = Get(reference, parent, content);
            return lbl;
        }

        public static Label Chal_quest_id(ChalLine line, int Column)
        {
            var reference = line.Chal.Id_Voc;
            var content = "Quest Id: " + line.Quest.Id;
            Get(reference, 0, Column, line.Chal.Row_4, content);
            reference.ToolTip = "Right click to copy the Id";
            reference.MouseRightButtonDown += (source, e) => Clipboard.SetText(line.Quest.Id.ToString());

            return reference;
        }
    }
}

public enum SortLbl
{
    Id,

    [Description("(w) Score")]
    Score_w,

    [Description("(m) Score")]
    Score_m,

    [Description("Score")]
    Score_all,

    [Description("Tries")]
    Tries,

    [Description("Sentences")]
    Sen,

    [Description("% Show")]
    Chance,

    [Description("Definition")]
    Def,

    [Description("PtBr")]
    PtBr,

    [Description("Words [Id]")]
    Words,

    [Description("Answer")]
    Answer,

    [Description("Phonemes")]
    Phonemes,
}
