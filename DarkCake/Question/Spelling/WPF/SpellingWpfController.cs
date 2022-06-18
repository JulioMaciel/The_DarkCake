using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public static class SpellWpfController
    {
        public static void AddIntoItems(StackPanel stack_items, SpellVM Spell, bool isNew)
        {
            var item_line = MyStacks.GetItemLine(stack_items, isNew);
            AddIntoThis(Spell, item_line);
        }

        public static void AddIntoThis(SpellVM Spell, StackPanel item_line)
        {
            var row1 = MyGrids.GetRowItem(new List<int>() {
                4, 1, 2, 2, 2, 2, 3, 1, 1, 1
            }, item_line);            

            var row3 = new StackPanel();
            row3.Margin = new Thickness(1, 2, 1, 2);
            item_line.Children.Add(row3);

            var wpf = new SpellWpfItem();

            MyTxts.Get(wpf.Words, 0, 0, row1, Spell.Text);
            wpf.Words.ToolTip = "Id " + Spell.Id;
            MyBtns.PlayPron(wpf.PlayPron, 0, 1, row1, wpf.Words);
            MyLbls.AvgScore(wpf.Avg_m, 0, 2, row1, Spell, 30);
            MyLbls.AvgScore(wpf.Avg_all, 0, 3, row1, Spell, 2000);
            MyLbls.Tries(wpf.Tries, 0, 4, row1, Spell);
            MyLbls.LastTry(wpf.Last_try, 0, 5, row1, Spell);
            MyLbls.Chance(wpf.Chance, 0, 6, row1, Spell);

            MyBtns.Is_active(wpf.IsActive, 0, 7, row1, Spell.IsActive);
            MyBtns.Quest_Edit(wpf.Edit, 0, 8, row1, Spell, wpf, item_line);
            MyBtns.Remove_quest(wpf.Remove, 0, 9, row1, Spell, item_line);
        }
    }
}
