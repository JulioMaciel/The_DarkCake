using AussieCake.Context;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class QuestControl : SqLiteHelper
    {
        public static IEnumerable<IQuest> Get(Model type)
        {
            LoadDB(type);

            switch (type)
            {
                case Model.Voc:
                    return Vocabularies.Cast<IQuest>();
                case Model.Pron:
                    return Pronunciations.Cast<IQuest>();
                case Model.Spell:
                    return Spellings.Cast<IQuest>();
                case Model.P_FIB:
                    return PearsonFIBs.Cast<IQuest>();
                case Model.P_Dit:
                    return PearsonDits.Cast<IQuest>();
                default:
                    Errors.ThrowErrorMsg(ErrorType.InvalidModelType, type);
                    return new List<IQuest>();
            }
        }

        public static bool Insert(IQuest quest)
        {
            if (quest is VocVM Voc)
                return VocControl.Insert(Voc);
            else if (quest is PronVM pron)
                return PronControl.Insert(pron);
            else if (quest is SpellVM spell)
                return SpellControl.Insert(spell);

            return false;
        }

        public static bool Update(IQuest quest)
        {
            if (quest is VocVM Voc)
                return VocControl.Update(Voc);
            else if (quest is PronVM pron)
                return PronControl.Update(pron);
            else if (quest is SpellVM spell)
                return SpellControl.Update(spell);

            return false;
        }

        public static void Remove(IQuest quest)
        {
            if (quest is VocVM Voc)
                VocControl.Remove(Voc);
            else if (quest is PronVM pron)
                PronControl.Remove(pron);
            else if (quest is SpellVM spell)
                SpellControl.Remove(spell);

            quest.RemoveAllAttempts();
        }

        public static void LoadCrossData(Model type)
        {
            //if (Get(type).First().Tries != null)
            //    return;

            foreach (var quest in Get(type))
                quest.LoadCrossData();

            LoadRealChances(type);
        }

        public static void LoadDB(Model type)
        {
            if (type == Model.Voc && Vocabularies == null)
                GetVocabulariesDB();
            else if (type == Model.Pron && Pronunciations == null)
                GetPronunciationsDB();
            else if (type == Model.Spell && Spellings == null)
                GetSpellingsDB();
            else if (type == Model.P_FIB && PearsonFIBs == null)
                GetP_FIB_DB();
            else if (type == Model.P_Dit && PearsonDits == null)
                GetP_Dit_DB();
        }

        public static void LoadEveryQuestionDB()
        {
            LoadDB(Model.Voc);
            LoadDB(Model.Pron);
            LoadDB(Model.Spell);
            LoadDB(Model.P_FIB);
            LoadDB(Model.P_Dit);
        }

        private static void LoadRealChances(Model type)
        {
            var summed_chances = Get(type).Sum(x => x.Chance);

            foreach (var quest in Get(type))
                quest.Chance_real = (quest.Chance * 100) / summed_chances;
        }

        public static IQuest GetRandomAvailableQuestion(Model type, List<int> actual_chosen)
        {
            double actual_index = 0;

            var quests = Get(type).Where(x => x.IsActive && !actual_chosen.Contains(x.Id));

            foreach (var quest in quests)
            {
                actual_index += quest.Chance;
                quest.Index_show = actual_index;
            }

            var totalChances = quests.Select(x => x.Chance).Sum();
            int pickBasedOnChance = UtilWPF.RandomNumber(0, (int)totalChances);

            IQuest selected = quests.First();

            double cumulative = 0.0;
            for (int i = 0; i < quests.Count(); i++)
            {
                cumulative += quests.ElementAt(i).Chance;
                if (pickBasedOnChance < cumulative)
                {
                    selected = quests.ElementAt(i);
                    break;
                }
            }

            return selected;
        }

        public static List<string> GetSentences(IQuest quest)
        {
            var words = quest.Text.SplitSentence().ToList();
            return SqLiteHelper.GetSentenceWhichContains(words);
        }
    }
}
