using AussieCake.Question;
using System.Windows.Controls;

namespace AussieCake.Util
{
    public interface IFilter
    {
        void SetSort(SortLbl sort, StackPanel stk_items);
        void Filter(IQuestWpfHeader wpf_header);
    }
}
