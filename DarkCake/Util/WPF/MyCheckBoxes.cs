using AussieCake.Question;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AussieCake.Util.WPF
{
    public static class MyChBxs
    {
        public static CheckBox IsVerb(CheckBox reference, int row, int Column, Grid parent, bool isCompVerb)
        {
            return Get(reference, row, Column, parent, "isVerb", isCompVerb);
        }

        public static CheckBox Get(CheckBox reference, int row, int Column, Grid parent, string content, bool isChecked)
        {
            var cb = Get(reference, content, isChecked);
            UtilWPF.SetGridPosition(cb, row, Column, parent);

            return cb;
        }

        public static CheckBox Get(CheckBox reference, string content, bool isChecked, StackPanel parent)
        {
            var cb = Get(reference, content, isChecked);
            parent.Children.Add(cb);

            return cb;
        }

        private static CheckBox Get(CheckBox reference, string content, bool isChecked)
        {
            reference.Content = content;
            reference.IsChecked = isChecked;
            reference.VerticalAlignment = VerticalAlignment.Center;
            reference.HorizontalAlignment = HorizontalAlignment.Center;
            reference.HorizontalContentAlignment = HorizontalAlignment.Center;
            reference.VerticalContentAlignment = VerticalAlignment.Center;
            reference.Margin = new Thickness(1, 0, 1, 0);

            return reference;
        }

        //public class CheckQuest : CheckBox
        //{
        //    public Model Type { get; set; }
        //    public int Id { get; set; }

        //    public CheckQuest(IQuest quest, bool isChecked)
        //    {
        //        Content = quest.ToText();

        //        MouseEnter += new MouseEventHandler((source, e) => Foreground = Brushes.DarkRed);
        //        MouseLeave += new MouseEventHandler((source, e) => Foreground = Brushes.Black);
        //        ToolTip = "Question Id " + quest.Id + "; Click to copy";
        //        Type = Model.Voc;
        //        Id = quest.Id;
        //        IsChecked = isChecked;
        //        VerticalContentAlignment = VerticalAlignment.Center;
        //        MouseLeftButtonDown += (source, e) =>
        //        {
        //            Clipboard.SetText(quest.Id.ToString());
        //            Foreground = Brushes.DarkGreen;
        //        };
        //    }

        //}
    }
}
