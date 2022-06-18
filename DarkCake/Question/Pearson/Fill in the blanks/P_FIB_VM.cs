using AussieCake.Util;
using System;
using System.Linq;

namespace AussieCake.Question
{
    public class P_FIB_VM : QuestVM, IPearsonVM
    {
        public PearsonType PearsonType { get; private set; }

        public P_FIB_VM(int id, string text, bool isActive, PearsonType pearsonType)
            : base(id, text, isActive, Model.P_FIB)
        {
            PearsonType = pearsonType;
        }

        public override void LoadCrossData()
        {
            base.LoadCrossData();
        }

        public string GetUpdatedRealChance()
        {
            var summed_chances = QuestControl.Get(Model.P_FIB).Sum(x => x.Chance);

            Chance_real = (Chance * 100) / summed_chances;

            return " (" + Math.Round(Chance_real, 2) + "%)";
        }
    }
}
