using AussieCake.Question;
using AussieCake.Util;

namespace AussieCake.Templates
{
    public class EssayVM : QuestVM
    {
        public string Word { get; set; }        

        public EssayVM(int id, string word, bool isInit = false) : base(id, word, true, Model.Essay)
        {
            IsInit = isInit;
        }
    }
}
