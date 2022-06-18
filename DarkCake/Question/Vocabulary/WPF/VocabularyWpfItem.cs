using System.Windows.Controls;

namespace AussieCake.Question
{
    public class VocWpfItem : QuestWpfItem
    {
        public TextBox Words { get; set; }
        public TextBox Answer { get; set; }

        public VocWpfItem()
        {
            Words = new TextBox();
            Answer = new TextBox();
        }
    }
}
