using AussieCake.Context;
using AussieCake.Question;
using AussieCake.Util;
using AussieCake.Verb;
using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Linq;
using System.Text.RegularExpressions;

namespace AussieCake.Challenge
{
    public static class Sentences
    {
        public static string GetSentenceToQuestion(IQuest quest)
        {
            var sens = quest.GetSentences();

            if (!sens.Any())
            {
                var url = quest.ToLudwigUrl();
                TryToGetSentencesOnThisSite(quest, sens, url);

                if (sens.Any())
                {
                    foreach (var sen in sens)
                        SqLiteHelper.InsertSentence(sen);

                    Console.WriteLine("Sen got on Ludwig, and " + sens.Count + " were added in DB");
                }

                if (!sens.Any() && !quest.Text.Contains(' '))
                {
                    url = quest.ToBritannicaUrl();
                    TryToGetSentencesOnThisSite(quest, sens, url);

                    if (sens.Any())
                        Console.WriteLine("Sen got on Britannica.");
                }

                if (!sens.Any())
                {
                    Console.WriteLine("Sen not found. URL: " + url);
                    return quest.Text;
                }
            }

            var chosen = sens.PickRandom();

            return chosen;
        }

        private static void TryToGetSentencesOnThisSite(IQuest quest, List<string> sens, string url)
        {
            var raw_Text = FileHtmlControls.GetTextFromSite(url);
            var raw_sentences = GetRawSentencesFromSource(raw_Text);

            foreach (var sen in raw_sentences)
            {
                if (DoesSenContainsVoc(quest, sen) && !sens.Contains(sen))
                    sens.Add(sen);
            }
        }

        private static bool DoesStartEndProperly(string s)
        {
            return ((s.EndsWith(".") && !s.EndsWith("Dr.") && !s.EndsWith("Mr.") && !s.EndsWith("Ms.") && !s.EndsWith("..."))
                                                || s.EndsWith("!") || s.EndsWith("?"));
        }

        private static List<string> GetSentencesFromSource(string source)
        {
            var matchList = Regex.Matches(source, @"[A-Z]+(\w+\,*\;*[ ]{0,1}[\.\?\!]*)+");
            return matchList.Cast<Match>().Select(match => match.Value).ToList();
        }

        private static List<string> GetRawSentencesFromSource(string source)
        {
            var sentences = new List<string>();

            sentences = GetSentencesFromSource(source);

            var filteredSentences = new List<string>();
            foreach (var sen in sentences)
            {
                if (!Errors.IsNullSmallerOrBigger(sen, 40, 80, false))
                {
                    if (DoesStartEndProperly(sen))
                        filteredSentences.Add(sen);
                }
            }

            return filteredSentences;
        }

        public static bool DoesSenContainsVoc(IQuest quest, string sen)
        {
            //System.Console.WriteLine(Voc.Text + ": " + sen);

            var lastIndexFound = -1;
            var words = quest.Text.SplitSentence().ToList();
            for (int i = 0; i < words.Count; i++)
            {
                var word = words[i];
                var actualIndex = -1;

                if (word.Length < 5)
                {
                    if (!sen.ContainsInsensitive(word))
                        return false;
                    else
                        actualIndex = sen.IndexOf(word);
                }
                else
                {
                    var that = GetIndexOfCompatibleWord(word, sen);

                    if (that.Any())
                        actualIndex = that.First();
                    else
                        return false;
                }

                if (lastIndexFound - actualIndex > 20)
                    return false;

                lastIndexFound = actualIndex;
            }

            return true;
        }

        //private static bool DoesSenContainsComp(string comp, bool isVerb, string sentence)
        //{
        //    var compatible = GetCompatibleWord(comp, sentence);

        //    if (!compatible.IsEmpty())
        //        return true;

        //    return false;
        //}

        public static string GetCompatibleWord(string comp, string sen)
        {
            if (sen.ContainsInsensitive(comp))
                return comp;

            VerbModel staticVerb = new VerbModel();

            if (VerbsController.Get().Any(v => v.Infinitive.EqualsNoCase(comp)))
            {
                staticVerb = VerbsController.Get().First(v => v.Infinitive.EqualsNoCase(comp));
                //else
                //    staticVerb = VerbsController.ConjugateUnknownVerb(comp);

                if (sen.ContainsInsensitive(staticVerb.Gerund))
                    return staticVerb.Gerund;
                else if (sen.ContainsInsensitive(staticVerb.Past))
                    return staticVerb.Past;
                else if (sen.ContainsInsensitive(staticVerb.PastParticiple))
                    return staticVerb.PastParticiple;
                else if (sen.ContainsInsensitive(staticVerb.Person))
                    return staticVerb.Person;
            }

            var service = PluralizationService.CreateService(System.Globalization.CultureInfo.CurrentCulture);
            if (service.IsSingular(comp))
            {
                var plural = service.Pluralize(comp);
                if (sen.ContainsInsensitive(plural))
                    return plural;
            }

            return string.Empty;
        }

        private static List<int> GetIndexOfCompatibleWord(string comp, string sen)
        {
            var result = new List<int>();

            if (sen.ContainsInsensitive(comp, true))
            {
                var indexes = sen.IndexesFrom(comp, true);
                AddUniqueValues(result, indexes);
            }

            VerbModel staticVerb = new VerbModel();

            if (VerbsController.Get().Any(v => v.Infinitive.EqualsNoCase(comp)))
            {
                staticVerb = VerbsController.Get().First(v => v.Infinitive.EqualsNoCase(comp));
                //else
                //    staticVerb = VerbsController.ConjugateUnknownVerb(comp);

                if (sen.ContainsInsensitive(staticVerb.Gerund, true))
                {
                    var indexes = sen.IndexesFrom(staticVerb.Gerund, true);
                    AddUniqueValues(result, indexes);
                }

                if (sen.ContainsInsensitive(staticVerb.Past, true))
                {
                    var indexes = sen.IndexesFrom(staticVerb.Past, true);
                    AddUniqueValues(result, indexes);
                }

                if (sen.ContainsInsensitive(staticVerb.PastParticiple, true))
                {
                    var indexes = sen.IndexesFrom(staticVerb.PastParticiple, true);
                    AddUniqueValues(result, indexes);
                }

                if (sen.ContainsInsensitive(staticVerb.Person, true))
                {
                    var indexes = sen.IndexesFrom(staticVerb.Person, true);
                    AddUniqueValues(result, indexes);
                }
            }

            var service = PluralizationService.CreateService(System.Globalization.CultureInfo.CurrentCulture);
            if (service.IsSingular(comp))
            {
                var plural = service.Pluralize(comp);
                if (sen.ContainsInsensitive(plural, true))
                {
                    var indexes = sen.IndexesFrom(plural, true);
                    AddUniqueValues(result, indexes);
                }
            }


            return result;
        }

        private static void AddUniqueValues(List<int> result, List<int> indexes)
        {
            foreach (var ind in indexes)
            {
                if (!result.Contains(ind))
                    result.Add(ind);
            }
        }

        private static List<int> GetIndexOfPart(List<string> parts, string sentence)
        {
            var indexes = new List<int>();

            if (parts == null)
                return indexes;

            if (!parts.Any())
                return indexes;

            foreach (var item in parts)
            {
                // check if it is Be
                if (item.ContainsInsensitive("Be"))
                {
                    foreach (var toBe in VerbsController.VerbToBe)
                    {
                        if (sentence.ContainsInsensitive(item.ReplaceInsensitive("Be", toBe)))
                            indexes.AddRange(sentence.IndexesFrom(toBe));
                    }

                    return indexes;
                }

                if (sentence.ContainsInsensitive(item))
                    indexes.AddRange(sentence.IndexesFrom(item));
            }
            return indexes;
        }
    }
}
