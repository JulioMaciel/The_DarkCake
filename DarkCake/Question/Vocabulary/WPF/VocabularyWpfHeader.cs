using System.Windows.Controls;

namespace AussieCake.Question
{
    public class VocWpfHeader : QuestWpfHeader
    {
        public TextBox Txt_words { get; set; }
        public TextBox Txt_answer { get; set; }
        public TextBox Txt_def { get; set; }
        public TextBox Txt_ptbr { get; set; }

        public Label Lbl_words { get; set; }
        public Label Lbl_answer { get; set; }
        public Label Lbl_ptBr { get; set; }
        public Label Lbl_def { get; set; }

        public VocWpfHeader()
        {
            Init();

            Txt_words = new TextBox();
            Txt_answer = new TextBox();
            Txt_def = new TextBox();
            Txt_ptbr = new TextBox();

            Lbl_words = new Label();
            Lbl_answer = new Label();
            Lbl_ptBr = new Label();
            Lbl_def = new Label();
        }
    }
}
