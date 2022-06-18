using AussieCake.Util;
using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public static class VocWpfController
    {
        public static void AddIntoItems(StackPanel stack_items, VocVM Voc, bool isNew)
        {
            var item_line = MyStacks.GetItemLine(stack_items, isNew);
            AddIntoThis(Voc, item_line);
        }

        public static void AddIntoThis(VocVM Voc, StackPanel item_line)
        {
            var row1 = MyGrids.GetRowItem(new List<int>() {
                3, 1
            }, item_line);

            var row2 = MyGrids.GetRowItem(new List<int>() {
                3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1
            }, item_line);

            var row3 = new StackPanel();
            row3.Margin = new Thickness(1, 2, 1, 2);
            item_line.Children.Add(row3);

            var wpf = new VocWpfItem();

            MyTxts.Get(wpf.Words, 0, 0, row1, Voc.Text);
            wpf.Words.ToolTip = "Id " + Voc.Id;
            MyTxts.Get(wpf.Answer, 0, 1, row1, Voc.Answer);

            MyLbls.AvgScore(wpf.Avg_w, 0, 0, row2, Voc, 7);
            MyLbls.AvgScore(wpf.Avg_m, 0, 1, row2, Voc, 30);
            MyLbls.AvgScore(wpf.Avg_all, 0, 2, row2, Voc, 2000);
            MyLbls.Tries(wpf.Tries, 0, 3, row2, Voc);
            MyLbls.LastTry(wpf.Last_try, 0, 4, row2, Voc);
            MyLbls.Chance(wpf.Chance, 0, 5, row2, Voc);

            MyBtns.Is_active(wpf.IsActive, 0, 6, row2, Voc.IsActive);
            MyBtns.PtBr(wpf.Show_ptbr, 0, 7, row2, Voc.PtBr, wpf.Ptbr);
            MyBtns.Definition(wpf.Show_def, 0, 8, row2, Voc.Definition, wpf.Def);
            MyBtns.Quest_Edit(wpf.Edit, 0, 9, row2, Voc, wpf, item_line);
            MyBtns.Remove_quest(wpf.Remove, 0, 10, row2, Voc, item_line);

            MyTxts.Definition(wpf.Def, Voc.Definition, row3);
            MyTxts.PtBr(wpf.Ptbr, Voc.PtBr, row3);

            MyTxts.Add_sentence(wpf.Add_sen, wpf.Stk_sen);
        }
    }
}
