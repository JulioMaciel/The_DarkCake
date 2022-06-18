using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Util.WPF
{
    public static class MyTxts
    {
        public static TextBox PtBr(TextBox reference, string ptBr, StackPanel parent)
        {
            var txt = Get(reference, ptBr, parent);
            txt.Visibility = Visibility.Collapsed;
            txt.Background = UtilWPF.GetBrushFromHTMLColor("#edfaeb");
            txt.ToolTip = "PtBr";

            return txt;
        }

        public static TextBox Definition(TextBox reference, string definition, StackPanel parent)
        {
            var txt = Get(reference, definition, parent);
            txt.Visibility = Visibility.Collapsed;
            txt.ToolTip = "Definition";

            return txt;
        }

        public static TextBox Add_sentence(TextBox reference, StackPanel stk_sen)
        {
            var txt = Get(reference, string.Empty, stk_sen);
            txt.ToolTip = "Add Sentence";

            return txt;
        }

        public static TextBox Get(TextBox reference, int row, int Column, Grid parent)
        {
            return Get(reference, row, Column, parent, string.Empty);
        }

        public static TextBox Bulk_Insert(TextBox reference, Grid parent)
        {
            reference.Text = "// format:  words;answer";
            reference.TextWrapping = TextWrapping.Wrap;
            reference.AcceptsReturn = true;
            UtilWPF.SetGridPosition(reference, 0, 0, parent);
            Grid.SetRowSpan(reference, 3);

            return reference;
        }

        public static TextBox Get(TextBox reference, StackPanel parent)
        {
            return Get(reference, string.Empty, parent);
        }

        public static TextBox Get(TextBox reference, int row, int Column, Grid parent, string content)
        {
            var txt = Get(reference, content);
            UtilWPF.SetGridPosition(txt, row, Column, parent);

            return txt;
        }

        public static TextBox Get(TextBox reference, string content, StackPanel parent)
        {
            var txt = Get(reference, content);
            parent.Children.Add(txt);

            return txt;
        }

        private static TextBox Get(TextBox reference, string content)
        {
            reference.Text = content;
            reference.VerticalContentAlignment = VerticalAlignment.Center;
            reference.Height = 28;
            reference.Margin = new Thickness(1, 0, 1, 0);

            return reference;
        }

        static public bool IsEmpty(this TextBox txt)
        {
            return txt.Text.IsEmpty();
        }
    }

}
