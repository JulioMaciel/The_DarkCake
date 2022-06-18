using AussieCake.Util;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class SpellControl : QuestControl
    {
        public static bool Insert(SpellVM Spelling)
        {
            if (Spellings.Any(s => s.Text == Spelling.Text))
            {
                return Errors.ThrowErrorMsg(ErrorType.AlreadyInserted, Spelling.Text);
            }

            if (!ValidWordsAndAnswerSize(Spelling.Text))
                return false;

            if (!InsertSpelling(Spelling.ToModel()))
                return false;

            Spelling.LoadCrossData();

            return true;
        }

        public static bool Update(SpellVM Spelling)
        {
            if (!ValidWordsAndAnswerSize(Spelling.Text))
                return false;

            if (!UpdateSpelling(Spelling.ToModel()))
                return false;


            var oldVM = Spellings.FindIndex(x => x.Id == Spelling.Id);
            Spellings.Insert(oldVM, Spelling);

            return true;
        }

        public static bool Remove(SpellVM Spelling)
        {
            if (!RemoveSpelling(Spelling))
                return false;

            return true;
        }

        private static bool ValidWordsAndAnswerSize(string words)
        {
            if (words.Length < 4)
                return Errors.ThrowErrorMsg(ErrorType.TooSmall, "Item wasn't saved. '" + words + "' is not valid.");

            return true;
        }

        private static List<string> RemoveUselessPart(List<string> parts)
        {
            if (parts.Any(x => x == "a"))
                parts.Remove(parts.First(y => y == "a"));

            if (parts.Any(x => x == "an"))
                parts.Remove(parts.First(y => y == "an"));

            if (parts.Any(x => x == "the"))
                parts.Remove(parts.First(y => y == "the"));

            return parts;
        }
    }
}
