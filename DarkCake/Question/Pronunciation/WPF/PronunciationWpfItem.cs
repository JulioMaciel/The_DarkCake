using System.Windows.Controls;

namespace AussieCake.Question
{
    public class PronWpfItem : QuestWpfItem
    {
        public TextBox Words { get; set; }
        public TextBox Phonemes { get; set; }

        public PronWpfItem()
        {
            Words = new TextBox();
            Phonemes = new TextBox();
        }
    }
}
