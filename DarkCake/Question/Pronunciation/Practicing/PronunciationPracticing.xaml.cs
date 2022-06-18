using AussieCake.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace AussieCake.Question
{
    /// <summary>
    /// Interaction logic for PronunPract.xaml
    /// </summary>
    public partial class PronunPract : UserControl
    {
        List<int> passedQuestIds;
        IQuest actualQuest;

        public PronunPract()
        {
            InitializeComponent();

            QuestControl.LoadCrossData(Model.Pron);

            passedQuestIds = new List<int>();
            LoadPronunQuest();
        }

        private void LoadPronunQuest()
        {
            actualQuest = QuestControl.GetRandomAvailableQuestion(Model.Pron, passedQuestIds);

            while (cb_justTH.IsChecked.Value && 
                (!(actualQuest as PronVM).Phonemes.Contains("θ") && !(actualQuest as PronVM).Phonemes.Contains("ð")))
                actualQuest = QuestControl.GetRandomAvailableQuestion(Model.Pron, passedQuestIds);

            lblWord.Content = actualQuest.Text;
            lblPhonemes.Content = (actualQuest as PronVM).Phonemes;

            FileHtmlControls.PlayPronunciation(actualQuest.Text);
            passedQuestIds.Add(actualQuest.Id);
        }

        private void BtnOpenSite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://dictation.io/");
        }

        private void BtnListenAgain_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FileHtmlControls.PlayPronunciation(actualQuest.Text);
        }

        private void BtnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadPronunQuest();
        }
    }
}
