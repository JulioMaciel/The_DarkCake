using AussieCake.Util;
using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Question
{
    /// <summary>
    /// Interaction logic for Vocabularies.xaml
    /// </summary>
    public partial class Vocabularies : UserControl
    {
        public Vocabularies()
        {
            InitializeComponent();

            QuestControl.LoadCrossData(Model.Voc);

            BuildHeader();
        }

        private void BuildHeader()
        {
            var wpf = new VocWpfHeader();

            var stk_insert = MyStacks.GetHeaderInsertFilter(wpf.Stk_insert, 0, 0, userControlGrid);

            MyGrids.Bulk_Insert(wpf.Grid_bulk_insert, userControlGrid);
            MyTxts.Bulk_Insert(wpf.Txt_bulk_insert, wpf.Grid_bulk_insert);
            MyBtns.Insert_Bulk(wpf.Grid_bulk_insert, wpf);
            MyBtns.Bulk_back(wpf.Grid_bulk_insert, wpf);

            var stk_items = MyStacks.GetListItems(1, 0, userControlGrid);

            var grid_insert = MyGrids.Get(new List<int>() {
                3, 1, 1
            }, 2, stk_insert);
            grid_insert.Margin = new Thickness(2, 0, 2, 0);

            var grid_filter = MyGrids.Get(new List<int>() {
                3, 3, 3, 3, 3, 2, 3
            }, 2, stk_insert);
            grid_filter.Margin = new Thickness(2, 0, 2, 0);
            wpf.Stk_items = stk_items;

            var filter = new VocFilter();

            MyLbls.Header(wpf.Lbl_words, 0, 0, grid_insert, SortLbl.Words, wpf.Txt_words, filter, stk_items);
            MyLbls.Header(wpf.Lbl_answer, 0, 1, grid_insert, SortLbl.Answer, wpf.Txt_answer, filter, stk_items);
            MyBtns.Show_bulk_insert(0, 2, grid_insert, wpf);

            MyTxts.Get(wpf.Txt_words, 1, 0, grid_insert);
            MyTxts.Get(wpf.Txt_answer, 1, 1, grid_insert);

            MyBtns.Quest_Insert(wpf.Btn_insert, 1, 2, grid_insert, stk_items, wpf);

            MyLbls.Header(wpf.Lbl_avg_w, 0, 0, grid_filter, SortLbl.Score_w, wpf.Txt_avg_w, filter, stk_items);
            MyLbls.Header(wpf.Lbl_avg_m, 0, 1, grid_filter, SortLbl.Score_m, wpf.Txt_avg_m, filter, stk_items);
            MyLbls.Header(wpf.Lbl_avg_all, 0, 2, grid_filter, SortLbl.Score_all, wpf.Txt_avg_all, filter, stk_items);
            MyLbls.Header(wpf.Lbl_tries, 0, 3, grid_filter, SortLbl.Tries, wpf.Txt_tries, filter, stk_items);
            MyLbls.Header(wpf.Lbl_chance, 0, 4, grid_filter, SortLbl.Chance, wpf.Txt_chance, filter, stk_items);
            MyLbls.Header(wpf.Lbl_def, 0, 5, grid_filter, SortLbl.Def, wpf.Txt_def, filter, stk_items);
            MyLbls.Header(wpf.Lbl_ptBr, 0, 6, grid_filter, SortLbl.PtBr, wpf.Txt_ptbr, filter, stk_items);

            MyTxts.Get(wpf.Txt_avg_w, 1, 0, grid_filter);
            MyTxts.Get(wpf.Txt_avg_m, 1, 1, grid_filter);
            MyTxts.Get(wpf.Txt_avg_all, 1, 2, grid_filter);
            MyTxts.Get(wpf.Txt_tries, 1, 3, grid_filter);
            MyTxts.Get(wpf.Txt_chance, 1, 4, grid_filter);
            MyTxts.Get(wpf.Txt_def, 1, 5, grid_filter);
            MyTxts.Get(wpf.Txt_ptbr, 1, 6, grid_filter);

            MyBtns.Is_active(wpf.Btn_isActive, 1, 7, grid_filter, true);
            MyBtns.Quest_Filter(wpf.Btn_filter, 1, 8, grid_filter, wpf, filter);

            filter.BuildStack(stk_items);
        }


    }
}
