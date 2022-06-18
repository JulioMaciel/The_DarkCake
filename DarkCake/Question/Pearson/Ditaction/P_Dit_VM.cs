using AussieCake.Util;
using System;
using System.Linq;

namespace AussieCake.Question
{
    public class P_Dit_VM : QuestVM, IPearsonVM
    {
        public PearsonType PearsonType { get; private set; }

        public P_Dit_VM(int id, string text, bool isActive, PearsonType pearsonType)
            : base(id, text, isActive, Model.P_Dit)
        {
            PearsonType = pearsonType;
        }

        public override void LoadCrossData()
        {
            base.LoadCrossData();
        }

        public string GetUpdatedRealChance()
        {
            var summed_chances = QuestControl.Get(Model.P_Dit).Sum(x => x.Chance);

            Chance_real = (Chance * 100) / summed_chances;

            return " (" + Math.Round(Chance_real, 2) + "%)";
        }
    }
}
