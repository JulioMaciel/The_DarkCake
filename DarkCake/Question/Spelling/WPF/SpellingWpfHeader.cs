using System.Windows.Controls;

namespace AussieCake.Question
{
    public class SpellWpfHeader : QuestWpfHeader
    {
        public TextBox Txt_words { get; set; }

        public Label Lbl_words { get; set; }

        public SpellWpfHeader()
        {
            Init();

            Txt_words = new TextBox();
            Lbl_words = new Label();
        }
    }
}
