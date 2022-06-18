using AussieCake.Question;
using AussieCake.Util;

namespace AussieCake.Templates
{
    public class DescImgVM : QuestVM
    {
        public string Word { get; set; }
        public bool IsStressed { get; }

        public DescImgVM(int id, string word, DescImgType type = DescImgType.Common) : base(id, word, true, Model.DescImg)
        {
            if (type == DescImgType.Init)
                IsInit = true;
            else if (type == DescImgType.Stressed)
                IsStressed = true;
            else if (type == DescImgType.Init_Stressed)
            {
                IsInit = true;
                IsStressed = true;
            }
        }
    }

    public enum DescImgType
    {
        Common,
        Init,
        Stressed,
        Init_Stressed
    }
}
