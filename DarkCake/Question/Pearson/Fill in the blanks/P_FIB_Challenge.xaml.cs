using AussieCake.Attempt;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AussieCake.Question.P_FIB_WPF;

namespace AussieCake.Question
{
    /// <summary>
    /// Interaction logic for P_FIB_Challenge.xaml
    /// </summary>
    public partial class P_FIB_Challenge : UserControl
    {
        List<int> passedQuestIds;
        IQuest actualQuest;

        List<CellPearson> actualCells;

        public P_FIB_Challenge()
        {
            InitializeComponent();

            QuestControl.LoadCrossData(Model.P_FIB);

            passedQuestIds = new List<int>();
            LoadNextQuest();

            //PreviewKeyDown += (sender, e) => LastTxt_PreviewKeyDown(sender, e);
        }

        private void LoadNextQuest()
        {
            actualQuest = QuestControl.GetRandomAvailableQuestion(Model.P_FIB, passedQuestIds);

            StkChallenge.Children.Clear();
            actualCells = CreateChallengeFromPearsonText((IPearsonVM)actualQuest, StkChallenge);

            ShowAttributes();

            foreach (var txt in StkChallenge.GetChildren<TextBox>())
                txt.PreviewKeyDown += (sender, e) => LastTxt_PreviewKeyDown(sender, e);

            StkChallenge.GetChildren<TextBox>().First().Focus();
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadNextQuest();

            btnRemoveAttempt.IsEnabled = false;
            btnNext.IsEnabled = false;
            btnStartSpeech.IsEnabled = true;
        }

        private void BtnVerify_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            passedQuestIds.Add(actualQuest.Id);
            btnRemoveAttempt.IsEnabled = true;
            btnNext.IsEnabled = true;

            CheckAnswers(actualCells, actualQuest);
            //actualQuest = QuestControl.Get(actualQuest.Type).FirstOrDefault(q => q.Id == actualQuest.Id);
            ShowAttributes();
            btnVerify.IsEnabled = false;
        }

        private async void BtnStartSpeech_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnStartSpeech.IsEnabled = false;
            var firstTxt = StkChallenge.GetChildren<TextBox>().FirstOrDefault();
            firstTxt.Focus();

            await FileHtmlControls.PlayPearson((P_FIB_VM)actualQuest);

            btnVerify.IsEnabled = true;
        }

        private void BtnRemoveAttempt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AttemptsControl.RemoveLast(Model.P_FIB);
            StkChallenge.Background = UtilWPF.Vocour_row_off;

            actualQuest.LoadCrossData();

            var updated = QuestControl.Get(Model.P_FIB).First(x => x.Id == actualQuest.Id);
            ShowAttributes();
        }

        private void ShowAttributes()
        {
            lblType.Content = ((IPearsonVM)actualQuest).PearsonType;

            var isNew = actualQuest.Tries.Count == 0;

            lblScoreWeek.Content = isNew ? "" : actualQuest.Avg_week + "% week";
            lblScoreWeek.Foreground = UtilWPF.GetAvgColor(actualQuest.Avg_week);
            
            lblScoreMonth.Content = isNew ? "First Try" : actualQuest.Avg_month + "% month";
            lblScoreMonth.Foreground = UtilWPF.GetAvgColor(actualQuest.Avg_month);

            lblScoreAll.Content = isNew ? "" : actualQuest.Avg_all + "% all";
            lblScoreAll.Foreground = UtilWPF.GetAvgColor(actualQuest.Avg_all);

            lblTries.Content = actualQuest.Tries.Count + " tries";

            lblChanceShow.ToolTip = actualQuest.Chance_toolTip;
            lblChanceShow.Content = actualQuest.Chance + ((IPearsonVM)actualQuest).GetUpdatedRealChance();

            var doneToday = AttemptsControl.Get(Model.P_FIB).Where(x => x.When.Date == DateTime.Today).Count();
            var total = QuestControl.Get(Model.P_FIB).Count();
            lblMeta.Content = "Meta (" + doneToday + "/" + total + ")";
            var percent_done = (int)Math.Round((double)(100 * doneToday) / total);
            lblMeta.Foreground = UtilWPF.GetAvgColor(percent_done);
        }

        private void LastTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnStartSpeech.IsEnabled)
                BtnStartSpeech_Click(sender, e);
            else if (e.Key == Key.Down && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnVerify.IsEnabled)
                BtnVerify_Click(sender, e);
            else if (e.Key == Key.Right && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnNext.IsEnabled)
                BtnNext_Click(sender, e);
        }
    }
}
