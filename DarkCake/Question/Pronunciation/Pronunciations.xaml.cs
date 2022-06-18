using AussieCake.Util;
using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Question
{
    /// <summary>
    /// Interaction logic for Pronabularies.xaml
    /// </summary>
    public partial class Pronunciations : UserControl
    {
        public Pronunciations()
        {
            InitializeComponent();

            QuestControl.LoadCrossData(Model.Pron);

            BuildHeader();
        }

        private void BuildHeader()
        {
            var wpf = new PronWpfHeader();

            var stk_insert = MyStacks.GetHeaderInsertFilter(wpf.Stk_insert, 0, 0, userControlGrid);

            MyGrids.Bulk_Insert(wpf.Grid_bulk_insert, userControlGrid);
            MyTxts.Bulk_Insert(wpf.Txt_bulk_insert, wpf.Grid_bulk_insert);
            MyBtns.Insert_Bulk(wpf.Grid_bulk_insert, wpf);
            MyBtns.Bulk_back(wpf.Grid_bulk_insert, wpf);

            var stk_items = MyStacks.GetListItems(1, 0, userControlGrid);
            wpf.Stk_items = stk_items;

            var grid_insert = MyGrids.Get(new List<int>() {
                3, 2, 2, 2, 2
            }, 2, stk_insert);
            grid_insert.Margin = new Thickness(2, 0, 2, 0);

            var filter = new PronFilter();

            MyLbls.Header(wpf.Lbl_words, 0, 0, grid_insert, SortLbl.Words, wpf.Txt_words, filter, stk_items);
            MyLbls.Header(wpf.Lbl_phonemes, 0, 1, grid_insert, SortLbl.Phonemes, wpf.Txt_phonemes, filter, stk_items);
            MyLbls.Header(wpf.Lbl_chance, 0, 2, grid_insert, SortLbl.Chance, wpf.Txt_chance, filter, stk_items);

            MyBtns.Is_active(wpf.Btn_isActive, 0, 3, grid_insert, true);

            MyBtns.Show_bulk_insert(0, 4, grid_insert, wpf);

            MyTxts.Get(wpf.Txt_words, 1, 0, grid_insert);
            wpf.Txt_words.LostFocus += (source, e) => 
            {
                if (wpf.Txt_words.Text.EndsWith(" "))
                    wpf.Txt_words.Text = wpf.Txt_words.Text.RemoveLast(1);

                if (wpf.Txt_words.Text.Contains("/"))
                    wpf.Txt_words.Text = wpf.Txt_words.Text.Remove('/');
            };

            MyTxts.Get(wpf.Txt_phonemes, 1, 1, grid_insert);

            MyTxts.Get(wpf.Txt_chance, 1, 2, grid_insert);
            MyBtns.Quest_Filter(wpf.Btn_filter, 1, 3, grid_insert, wpf, filter);
            MyBtns.Quest_Insert(wpf.Btn_insert, 1, 4, grid_insert, stk_items, wpf);

            filter.BuildStack(stk_items);
        }


    }
}
