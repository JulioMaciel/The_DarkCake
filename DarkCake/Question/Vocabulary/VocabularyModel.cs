using AussieCake.Util;

namespace AussieCake.Question
{
    public class VocModel : QuestionModel
    {
        public string Answer { get; private set; }
        public string PtBr { get; protected set; }
        public string Definition { get; protected set; }

        public VocModel(int id, string text, string answer, string ptBr, string definition, int isActive)
            : base(id, text, isActive)
        {
            SetProperties(answer, ptBr, definition);
        }

        public VocModel(string text, string answer, string ptBr, string definition, int isActive)
            : base(text, isActive)
        {
            SetProperties(answer, ptBr, definition);
        }

        private void SetProperties(string answer, string ptBr, string definition)
        {
            Answer = answer;
            PtBr = ptBr;
            Definition = definition;
        }

        public VocVM ToVM()
        {
            var def_vm = Definition.EmptyIfNull();
            var ptBr_vm = PtBr.EmptyIfNull();
            var isActive_vm = IsActive.ToBool();

            if (IsReal)
                return new VocVM(Id, Text, Answer, def_vm, ptBr_vm, isActive_vm);
            else
                return new VocVM(Text, Answer, def_vm, ptBr_vm, isActive_vm);
        }
    }
}
