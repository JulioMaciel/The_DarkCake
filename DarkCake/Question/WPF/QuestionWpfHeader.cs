using AussieCake.Util.WPF;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public class QuestWpfHeader : IQuestWpfHeader
    {
        public StackPanel Stk_items { get; set; }

        public StackPanel Stk_insert { get; set; }
        public Grid Grid_bulk_insert { get; set; }

        public ComboBox Cob_bulk_imp { get; set; }
        public TextBox Txt_bulk_insert { get; set; }

        public TextBox Txt_avg_w { get; set; }
        public TextBox Txt_avg_m { get; set; }
        public TextBox Txt_avg_all { get; set; }
        public TextBox Txt_tries { get; set; }
        public TextBox Txt_chance { get; set; }

        public Label Lbl_avg_w { get; set; }
        public Label Lbl_avg_m { get; set; }
        public Label Lbl_avg_all { get; set; }
        public Label Lbl_tries { get; set; }
        public Label Lbl_sen { get; set; }
        public Label Lbl_chance { get; set; }
        public Label Lbl_imp { get; set; }

        public ComboBox Cob_imp { get; set; }
        public ButtonActive Btn_isActive { get; set; }
        public Button Btn_insert { get; set; }
        public Button Btn_filter { get; set; }

        protected void Init()
        {
            Stk_items = new StackPanel();

            Stk_insert = new StackPanel();
            Grid_bulk_insert = new Grid();

            Txt_bulk_insert = new TextBox();

            Txt_avg_w = new TextBox();
            Txt_avg_m = new TextBox();
            Txt_avg_all = new TextBox();
            Txt_tries = new TextBox();
            Txt_chance = new TextBox();
            Lbl_avg_w = new Label();
            Lbl_avg_m = new Label();
            Lbl_avg_all = new Label();
            Lbl_tries = new Label();
            Lbl_sen = new Label();
            Lbl_chance = new Label();
            Lbl_imp = new Label();
            Cob_imp = new ComboBox();
            Btn_isActive = new ButtonActive();
            Btn_insert = new Button();
            Btn_filter = new Button();

            Cob_bulk_imp = new ComboBox();
        }
    }
}
