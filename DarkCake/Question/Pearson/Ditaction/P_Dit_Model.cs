using AussieCake.Util;

namespace AussieCake.Question
{ 
    public class P_Dit_Model : QuestionModel
    {
        private PearsonType PearsonType { get; set; }

        public P_Dit_Model(int id, string text, int type, int isActive)
            : base(id, text, isActive)
        {
            PearsonType = (PearsonType)type;
        }

        public P_Dit_VM ToVM()
        {
            var isActive_vm = IsActive.ToBool();

            return new P_Dit_VM(Id, Text, isActive_vm, PearsonType);
        }
    }
}
