using AussieCake.Util;

namespace AussieCake.Question
{
    public class VocVM : QuestVM
    {
        public string Answer { get; private set; }
        public string PtBr { get; protected set; }
        public string Definition { get; protected set; }

        public VocVM(int id, string text, string answer, string definition, string ptBr,
                        bool isActive)
            : base(id, text, isActive, Model.Voc)
        {
            SetProperties(answer, ptBr, definition);
        }

        public VocVM(string text, string answer, string definition, string ptBr,
                        bool isActive)
            : base(text, isActive, Model.Voc)
        {
            SetProperties(answer, ptBr, definition);
        }

        private void SetProperties(string answer, string ptBr, string def)
        {
            Answer = answer.Trim().ToLower();
            PtBr = ptBr.Trim();
            Definition = def.Trim();
        }

        public VocModel ToModel()
        {
            var isActive_raw = IsActive.ToInt();

            if (IsReal)
                return new VocModel(Id, Text, Answer, PtBr, Definition, isActive_raw);
            else
                return new VocModel(Text, Answer, PtBr, Definition, isActive_raw);
        }

        public override void LoadCrossData()
        {
            base.LoadCrossData();
        }

    }
}
