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

namespace AussieCake.Challenge
{
    /// <summary>
    /// Interaction logic for SpellChallenge.xaml
    /// </summary>
    public partial class SpellChallenge : UserControl
    {
        List<ChalLine> lines = new List<ChalLine>(4);
        List<int> actual_chosen = new List<int>();

        public SpellChallenge()
        {
            InitializeComponent();

            LoadRequirements();

            ChalWPFControl.PopulateRows(userControlGrid, Model.Spell, lines, actual_chosen);

            CreateFrame();
        }

        private void LoadRequirements()
        {
            QuestControl.LoadCrossData(Model.Spell);
        }

        private void CreateFrame()
        {
            var grid = MyGrids.Get(4, 0, userControlGrid, new List<int>() { 1, 1 }, 1);

            var stk_btns = MyStacks.Get(new StackPanel(), 0, 1, grid);
            stk_btns.HorizontalAlignment = HorizontalAlignment.Right;

            var btn_next = new Button();
            var btn_verify = new Button();

            MyBtns.Chal_verify(btn_verify, stk_btns, btn_next);
            btn_verify.Click += (source, e) => lines.ForEach(x => ChalWPFControl.Verify(x, btn_verify, btn_next));

            MyBtns.Chal_next(btn_next, stk_btns, btn_verify, userControlGrid, lines, Model.Spell, actual_chosen);
        }

        //private void LastTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Left && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnStartSpeech.IsEnabled)
        //        BtnStartSpeech_Click(sender, e);
        //    else if (e.Key == Key.Down && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnVerify.IsEnabled)
        //        BtnVerify_Click(sender, e);
        //    else if (e.Key == Key.Right && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnNext.IsEnabled)
        //        BtnNext_Click(sender, e);
        //}
    }
}
