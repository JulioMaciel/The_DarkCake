using AussieCake.Util;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class PronVM : QuestVM
    {
        public string Phonemes { get; private set; }

        public PronVM(int id, string text, string phonemes, bool isActive)
            : base(id, text, isActive, Model.Pron)
        {
            SetProperties(phonemes);
        }

        public PronVM(string text, string phonemes, bool isActive)
            : base(text, isActive, Model.Pron)
        {
            SetProperties(phonemes);
        }

        private void SetProperties(string answer)
        {
            Phonemes = answer;
        }

        public PronModel ToModel()
        {
            var isActive_raw = IsActive.ToInt();
            Text = Text.Replace('\'', '`');
            Phonemes = Phonemes.Replace('\'', '`');

            if (IsReal)
                return new PronModel(Id, Text, Phonemes, isActive_raw);
            else
                return new PronModel(Text, Phonemes, isActive_raw);
        }

        public override void LoadCrossData()
        {
            base.LoadCrossData();
        }
    }
}
