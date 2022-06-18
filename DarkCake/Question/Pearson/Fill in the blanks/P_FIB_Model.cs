using AussieCake.Util;

namespace AussieCake.Question
{ 
    public class P_FIB_Model : QuestionModel
    {
        private PearsonType PearsonType { get; set; }

        public P_FIB_Model(int id, string text, int type, int isActive)
            : base(id, text, isActive)
        {
            PearsonType = (PearsonType)type;
        }

        public P_FIB_VM ToVM()
        {
            var isActive_vm = IsActive.ToBool();

            return new P_FIB_VM(Id, Text, isActive_vm, PearsonType);
        }
    }
}
