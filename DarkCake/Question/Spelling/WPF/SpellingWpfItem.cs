using System.Windows.Controls;

namespace AussieCake.Question
{
    public class SpellWpfItem : QuestWpfItem
    {
        public TextBox Words { get; set; }
        public TextBox Answer { get; set; }

        public SpellWpfItem()
        {
            Words = new TextBox();
            Answer = new TextBox();
        }
    }
}
