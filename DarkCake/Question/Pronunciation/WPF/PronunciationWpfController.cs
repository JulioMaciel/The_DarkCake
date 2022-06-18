using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public static class PronWpfController
    {
        public static void AddIntoItems(StackPanel stack_items, PronVM Pron, bool isNew)
        {
            var item_line = MyStacks.GetItemLine(stack_items, isNew);
            AddIntoThis(Pron, item_line);
        }

        public static void AddIntoThis(PronVM Pron, StackPanel item_line)
        {
            var row1 = MyGrids.GetRowItem(new List<int>() {
                3, 3, 1, 2, 1, 1, 1
            }, item_line);

            var wpf = new PronWpfItem();

            MyTxts.Get(wpf.Words, 0, 0, row1, Pron.Text);
            wpf.Words.ToolTip = "Id " + Pron.Id;
            MyTxts.Get(wpf.Phonemes, 0, 1, row1, Pron.Phonemes);
            MyBtns.PlayPron(wpf.PlayPron, 0, 2, row1, wpf.Words);
            MyLbls.Chance(wpf.Chance, 0, 3, row1, Pron);
            MyBtns.Is_active(wpf.IsActive, 0, 4, row1, Pron.IsActive);
            MyBtns.Quest_Edit(wpf.Edit, 0, 5, row1, Pron, wpf, item_line);
            MyBtns.Remove_quest(wpf.Remove, 0, 6, row1, Pron, item_line);
        }
    }
}
