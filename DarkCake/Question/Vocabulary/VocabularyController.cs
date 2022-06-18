using AussieCake.Attempt;
using AussieCake.Context;
using AussieCake.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AussieCake.Question
{
    public class VocControl : QuestControl
    {
        public static bool Insert(VocVM Vocabulary)
		{
            if (Vocabularies.Any(s => s.Text == Vocabulary.Text))
            {
                Errors.ThrowErrorMsg(ErrorType.AlreadyInserted, Vocabulary.Text);
            }

            if (!ValidWordsAndAnswerSize(Vocabulary.Text, Vocabulary.Answer))
                return false;

            if (!InsertVocabulary(Vocabulary.ToModel()))
                return false;

            Vocabulary.LoadCrossData();

            return true;
		}

        public static bool Update(VocVM Vocabulary)
		{
            if (!ValidWordsAndAnswerSize(Vocabulary.Text, Vocabulary.Answer))
                return false;

            if (!UpdateVocabulary(Vocabulary.ToModel()))
                return false;


			var oldVM = Vocabularies.FindIndex(x => x.Id == Vocabulary.Id);
            Vocabularies.Insert(oldVM, Vocabulary);

            return true;
		}

        public static bool Remove(VocVM Vocabulary)
		{
            if (!RemoveVocabulary(Vocabulary))
                return false;

            return true;
        }

        private static bool ValidWordsAndAnswerSize(string words, string answer)
        {
            if (!words.Contains(answer))
                return Errors.ThrowErrorMsg(ErrorType.AnswerNotFound, answer);

            if (answer.Length < 2)
                return Errors.ThrowErrorMsg(ErrorType.TooSmall, "Item wasn't saved. '" + answer + "' is not valid.");

            if (words.Length < 6)
                return Errors.ThrowErrorMsg(ErrorType.TooSmall, "Item wasn't saved. '" + words + "' is not valid.");

            if (!words.Contains(' '))
                return Errors.ThrowErrorMsg(ErrorType.InvalidCharacters, "Item wasn't saved. There's just one word.");

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
