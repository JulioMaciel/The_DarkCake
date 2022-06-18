using AussieCake.Util;

namespace AussieCake.Question
{
    public class SpellModel : QuestionModel
    {
        public SpellModel(int id, string text, int isActive)
            : base(id, text, isActive)
        {
        }

        public SpellModel(string text, int isActive)
            : base(text, isActive)
        {
        }

        public SpellVM ToVM()
        {
            var isActive_vm = IsActive.ToBool();

            if (IsReal)
                return new SpellVM(Id, Text, isActive_vm);
            else
                return new SpellVM(Text, isActive_vm);
        }
    }
}
