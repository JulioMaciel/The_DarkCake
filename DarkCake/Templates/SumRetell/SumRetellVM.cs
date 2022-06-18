using AussieCake.Question;
using AussieCake.Util;

namespace AussieCake.Templates
{
    public class SumRetellVM : QuestVM
    {
        public string Word { get; set; }

        public SumRetellVM(int id, string word, bool isInit = false) : base(id, word, true, Model.SumRetell)
        {
            IsInit = isInit;
        }
    }
}
