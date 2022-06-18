using System.Windows.Controls;

namespace AussieCake.Question
{
    public class PronWpfHeader : QuestWpfHeader
    {
        public TextBox Txt_words { get; set; }
        public TextBox Txt_phonemes { get; set; }

        public Label Lbl_words { get; set; }
        public Label Lbl_phonemes { get; set; }

        public PronWpfHeader()
        {
            Init();

            Txt_words = new TextBox();
            Txt_phonemes = new TextBox();
            Lbl_words = new Label();
            Lbl_phonemes = new Label();
        }
    }
}
