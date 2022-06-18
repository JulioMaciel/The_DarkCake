using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Windows.Controls;

namespace AussieCake.Challenge
{
    public class ChalWpfItem
    {
        public Grid Grid_chal { get; set; }

        public StackPanel Row_1 { get; set; }
        public StackPanel Row_2 { get; set; }
        public Grid Row_3 { get; set; }
        public Grid Row_4 { get; set; }

        public Label Answer { get; set; }
        public Label PtBr { get; set; }
        public Label Definition { get; set; }

        public List<Label> Quest_words { get; set; }
        public ComboChallenge Cb_Answer { get; set; }
        public TextBox Txt_Spell { get; set; }

        public Label Avg_w { get; set; }
        public Label Avg_m { get; set; }
        public Label Avg_all { get; set; }
        public Label Tries { get; set; }
        public Label Importante { get; set; }
        public Label Chance { get; set; }
        public Label Id_Voc { get; set; }
        public Button Disable_quest { get; set; }
        public Button Remove_att { get; set; }

        public ChalWpfItem()
        {
            Grid_chal = new Grid();

            Row_1 = new StackPanel();
            Row_2 = new StackPanel();
            Row_3 = new Grid();
            Row_4 = new Grid();

            PtBr = new Label();
            Definition = new Label();

            Quest_words = new List<Label>();
            Cb_Answer = new ComboChallenge();

            Answer = new Label();
            Avg_w = new Label();
            Avg_m = new Label();
            Avg_all = new Label();
            Tries = new Label();
            Importante = new Label();
            Chance = new Label();

            Id_Voc = new Label();
            Disable_quest = new Button();
            Remove_att = new Button();
        }
    }
}
