using AussieCake.Util.WPF;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public abstract class QuestWpfItem
    {
        public StackPanel Stk_sen { get; set; }

        public TextBox Def { get; set; }
        public TextBox Ptbr { get; set; }
        public TextBox Add_sen { get; set; }

        public Label Avg_w { get; set; }
        public Label Avg_m { get; set; }
        public Label Avg_all { get; set; }
        public Label Tries { get; set; }
        public Label Last_try { get; set; }
        public Label Chance { get; set; }

        public ComboBox Imp { get; set; }
        public Button Show_sen { get; set; }
        public ButtonActive IsActive { get; set; }
        public Button Show_ptbr { get; set; }
        public Button Show_def { get; set; }
        public Button Edit { get; set; }
        public Button Remove { get; set; }
        public Button PlayPron { get; set; }


        public QuestWpfItem()
        {
            Stk_sen = new StackPanel();
            Def = new TextBox();
            Ptbr = new TextBox();
            Add_sen = new TextBox();
            Avg_w = new Label();
            Avg_m = new Label();
            Avg_all = new Label();
            Tries = new Label();
            Last_try = new Label();
            Chance = new Label();
            Imp = new ComboBox();
            Show_sen = new Button();
            IsActive = new ButtonActive();
            Show_ptbr = new Button();
            Show_def = new Button();
            Edit = new Button();
            Remove = new Button();
            PlayPron = new Button();
        }
    }
}
