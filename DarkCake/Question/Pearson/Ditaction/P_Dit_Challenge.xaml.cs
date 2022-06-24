using AussieCake.Attempt;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AussieCake.Question
{
    /// <summary>
    /// Interaction logic for P_Dit_Challenge.xaml
    /// </summary>
    public partial class P_Dit_Challenge : UserControl
    {
        List<int> passedQuestIds;
        IQuest actualQuest;

        public P_Dit_Challenge()
        {
            InitializeComponent();

            QuestControl.LoadCrossData(Model.P_Dit);

            passedQuestIds = new List<int>();
            LoadNextQuest();
            btnStartSpeech.Focus();
        }

        private void LoadNextQuest()
        {
            actualQuest = QuestControl.GetRandomAvailableQuestion(Model.P_Dit, passedQuestIds);

            if (rb_ASQ.IsChecked.Value)
                while (((IPearsonVM)actualQuest).PearsonType != PearsonType.ASQ)
                    actualQuest = QuestControl.GetRandomAvailableQuestion(Model.P_Dit, passedQuestIds);
            else if (rb_RS.IsChecked.Value)
                while (((IPearsonVM)actualQuest).PearsonType != PearsonType.RS)
                    actualQuest = QuestControl.GetRandomAvailableQuestion(Model.P_Dit, passedQuestIds);
            else if (rb_WFD.IsChecked.Value)
                while (((IPearsonVM)actualQuest).PearsonType != PearsonType.WFD)
                    actualQuest = QuestControl.GetRandomAvailableQuestion(Model.P_Dit, passedQuestIds);

            ShowAttributes();

            btnStartSpeech.Focus();
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadNextQuest();

            btnRemoveAttempt.IsEnabled = false;
            btnNext.IsEnabled = false;
            txtAnswer.Text = "Waiting for answer...";
            txtAttempt.Text = string.Empty;
            txtAttempt.Background = Brushes.White;
            lblPercent.Content = string.Empty;
            stkDiff.Children.Clear();
            btnStartSpeech.IsEnabled = true;
            btnStartSpeech.Focus();
            Footer.Log("");
        }

        private async void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            passedQuestIds.Add(actualQuest.Id);
            btnRemoveAttempt.IsEnabled = true;
            CheckAnswers();
            txtAnswer.Text = actualQuest.Text;
            ShowAttributes();
            btnVerify.IsEnabled = false;

            await FileHtmlControls.PlayPearson((P_Dit_VM)actualQuest);
            btnNext.IsEnabled = true;
            btnNext.Focus();
        }

        private void CheckAnswers()
        {
            var theseQuests = QuestControl.Get(actualQuest.Type).Where(q => q is P_Dit_VM && (q as P_Dit_VM).PearsonType == (actualQuest as P_Dit_VM).PearsonType);
            var theseQuestsToday = theseQuests.Where(q => q.Tries.Any() && q.Tries.Last().When.Date == DateTime.Today).Count();
            var footerText = $"Completed {theseQuestsToday+1} of {theseQuests.Count()} {(actualQuest as P_Dit_VM).PearsonType.ToDesc()} today!";

            if (theseQuests.Any(q => !q.Tries.Any()))
                footerText += $" There are still {theseQuests.Where(q => !q.Tries.Any()).Count()-1} quests without tries.";

            Footer.Log(footerText);

            if (actualQuest.Type == Model.P_Dit &&
                (actualQuest as P_Dit_VM).PearsonType == PearsonType.RS &&
                txtAttempt.IsEmpty())
            {
                var rs_vm = new AttemptVM(actualQuest.Id, 100, DateTime.Now, Model.P_Dit);
                AttemptsControl.Insert(rs_vm);
                return;
            }

            var parts = actualQuest.Text.Split(new[] { ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (((IPearsonVM)actualQuest).PearsonType == PearsonType.ASQ)
            {
                var lastQuestionPart = parts.First(p => p.Contains("?"));
                var indexOfThat = parts.ToList().IndexOf(lastQuestionPart);
                var extraQuantity = parts.Count() - indexOfThat - 1;
                var asqAnswer = parts.ToList().GetRange(indexOfThat + 1, extraQuantity);
                parts = asqAnswer.ToArray();
            }

            var total = parts.Count();
            var corrects = 0;

            var partsAttempt = txtAttempt.Text.Split(new[] { ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
            corrects = parts.Where(i => partsAttempt.Contains(i)).ToList().Count();

            var percent_corrects = corrects != 0 ? (int)Math.Round((double)(100 * corrects) / total) : 0;
            lblPercent.Content = percent_corrects + "%";
            lblPercent.Background = UtilWPF.GetBrushFromHTMLColor("#edf2f7");
            lblPercent.Foreground = UtilWPF.GetAvgColor(percent_corrects);

            foreach (var chunck in parts.Except(partsAttempt))
            {
                var lbl = new Label();
                lbl.Content = chunck;
                lbl.Margin = new Thickness(0,0,6,0);
                stkDiff.Children.Add(lbl);
                lbl.ToolTip = "Click to add this word into the spelling list";
                lbl.MouseEnter += (source, e) => lbl.FontWeight = FontWeights.Bold;
                lbl.MouseLeave += (source, e) => lbl.FontWeight = FontWeights.Regular;

                if (QuestControl.Get(Model.Spell).Any(w => w.Text == chunck))
                    lbl.Foreground = Brushes.LightSalmon;
                else 
                    lbl.MouseLeftButtonDown += (source, e) =>
                    {
                        var spelling = new SpellVM(chunck, true);
                        QuestControl.Insert(spelling);
                        lbl.Foreground = Brushes.OrangeRed;
                    };
            }

            if (parts.Except(partsAttempt).Count() == 0 && percent_corrects != 100)
                Console.WriteLine("It shouldn't happen");

            var vm = new AttemptVM(actualQuest.Id, percent_corrects, DateTime.Now, actualQuest.Type);
            AttemptsControl.Insert(vm);
        }

        private async void BtnStartSpeech_Click(object sender, RoutedEventArgs e)
        {
            btnStartSpeech.IsEnabled = false;
            txtAttempt.Focus();

            //if (((IPearsonVM)actualQuest).PearsonType == PearsonType.RS)
            //    await FileHtmlControls.PlayPearson((P_Dit_VM)actualQuest, true);
            //else
                await FileHtmlControls.PlayPearson((P_Dit_VM)actualQuest);

            btnVerify.IsEnabled = true;
        }

        private void BtnRemoveAttempt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AttemptsControl.RemoveLast(Model.P_Dit);

            actualQuest.LoadCrossData();

            var updated = QuestControl.Get(Model.P_Dit).First(x => x.Id == actualQuest.Id);
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

            
        }

        private void TxtAttempt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnStartSpeech.IsEnabled)
                BtnStartSpeech_Click(sender, e);
            else if (e.Key == Key.Down && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnVerify.IsEnabled)
                BtnVerify_Click(sender, e);
            else if (e.Key == Key.Right && Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && btnNext.IsEnabled)
                BtnNext_Click(sender, e);
            else if (e.Key == Key.Enter && btnVerify.IsEnabled)
                btnVerify.Focus();
        }

        //private void cb_Type_Click(object sender, RoutedEventArgs e)
        //{
        //    LoadNextQuest();
        //    btnStartSpeech.Focus();
        //}

        private void rb_Type_Click(object sender, RoutedEventArgs e)
        {
            BtnNext_Click(sender, e);
        }
    }
}
