using AussieCake.Util;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class SpellVM : QuestVM
    {
        public SpellVM(int id, string text, bool isActive)
            : base(id, text, isActive, Model.Spell)
        {
        }

        public SpellVM(string text, bool isActive)
            : base(text, isActive, Model.Spell)
        {
        }

        public SpellModel ToModel()
        {
            var isActive_raw = IsActive.ToInt();

            if (IsReal)
                return new SpellModel(Id, Text, isActive_raw);
            else
                return new SpellModel(Text, isActive_raw);
        }

        public override void LoadCrossData()
        {
            base.LoadCrossData();
        }
    }
}
