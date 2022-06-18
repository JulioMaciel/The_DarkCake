using AussieCake.Verb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AussieCake.Util
{
    public static class UtilExtensions
    {
        public static string ToDesc(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string RemoveLast(this string text, int quantity)
        {
            if (quantity >= text.Length + 1)
                Errors.ThrowErrorMsg(ErrorType.TooBig, quantity);

            return text.Remove(text.Length - quantity);
        }

        public static string GetSinceTo(this string text, string startsAt, string endsAt)
        {
            var idx1 = text.IndexOf(startsAt);
            var idx2 = text.IndexOf(endsAt);
            return text.Substring(idx1, idx2 - idx1);
        }

        public static string GetBetween(this string text, string startsAt, string endsAt)
        {
            var from = text.IndexOf(startsAt) + startsAt.Length;
            var to = text.IndexOf(endsAt);
            return text.Substring(from, to - from);
        }

        public static string ToText(this List<string> list)
        {
            if (list == null)
                return null;

            list.Sort();

            return list.Any() ? (list.Count == 1 ? list.First() : String.Join(";", list.ToArray())) : string.Empty;
        }

        public static string ToText(this List<int> list)
        {
            if (list == null)
                return null;

            return ToText(list.ConvertAll(x => x.ToString()));
        }

        public static int ToInt(this bool value)
        {
            return Convert.ToInt16(value);
        }

        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }

        public static bool EqualsNoCase(this string me, string other)
        {
            return me.Equals(other, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsInsensitive(this string source, string toCheck, bool addSpaceBefore = false)
        {
            toCheck = addSpaceBefore ? " " + toCheck : toCheck;
            return source?.IndexOf(toCheck, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static int IndexFrom(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.CurrentCultureIgnoreCase);
        }

        public static List<int> IndexesFrom(this string source, string toCheck, bool addOne = false)
        {
            var indexes = new List<int>();

            while (source.ContainsInsensitive(toCheck))
            {
                var index = source.IndexFrom(toCheck);
                index = addOne ? index + 1 : index;
                indexes.Add(index);
                source = source.Remove(index, toCheck.Count());
                source = source.Insert(index, new string('*', toCheck.Length));
            }

            return indexes;
        }

        public static int GetMinimumDistance(this List<int> bigger, List<int> smaller)
        {
            int minDistance = int.MaxValue;

            foreach (var big in bigger)
            {
                foreach (var small in smaller.Where(x => x < big))
                {
                    if (big - small < minDistance)
                        minDistance = big - small;
                }
            }

            return minDistance;
        }

        public static string ReplaceInsensitive(this string str, string from, string to)
        {
            str = Regex.Replace(str, from, to, RegexOptions.IgnoreCase);
            return str;
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string EmptyIfNull(this string str)
        {
            return str.IsEmpty() ? string.Empty : str;
        }

        public static string NormalizeWhiteSpace(this string input, char normalizeTo = ' ')
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            int current = 0;
            char[] output = new char[input.Length];
            bool skipped = false;

            foreach (char c in input.ToCharArray())
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output[current++] = normalizeTo;

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[current++] = c;
                }
            }

            return new string(output, 0, skipped ? current - 1 : current);
        }

        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static bool IsLettersOnly(this string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }

        public static bool IsLettersOnly(this string str, char exception)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z" + exception + "]+$");
        }

        public static bool HasLettersOnly(this List<string> list)
        {
            foreach (var str in list)
            {
                if (!str.IsLettersOnly())
                    return false;
            }

            return true;
        }

        public static string UpperFirst(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }


        public static List<string> ToListString(this string raw, char separator = ';')
        {
            return raw.IsEmpty() ? new List<string>() : raw.Split(separator).ToList();
        }

        public static List<int> ToListInt(this string raw)
        {
            return raw.IsEmpty() ? new List<int>() : raw.Split(';')
                                                        .Select(s => Int32.TryParse(s, out int n) ? n : (int?)null)
                                                        .Where(n => n.HasValue)
                                                        .Select(n => n.Value)
                                                        .ToList();
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.Shuffle().First();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static bool IsVerb(this string word)
        {
            if (VerbsController.Get().Any(v => v.Infinitive.EqualsNoCase(word)))
                return true;

            if (VerbsController.Get().Any(v => v.Gerund.EqualsNoCase(word)))
                return true;

            if (VerbsController.Get().Any(v => v.Past.EqualsNoCase(word)))
                return true;

            if (VerbsController.Get().Any(v => v.PastParticiple.EqualsNoCase(word)))
                return true;

            if (VerbsController.Get().Any(v => v.Person.EqualsNoCase(word)))
                return true;

            return false;
        }

        public static void CleanClickEvents(this Button b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventClick",
                BindingFlags.Static | BindingFlags.NonPublic);
            if (f1 == null)
                return;
            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events",
                BindingFlags.NonPublic | BindingFlags.Instance);
            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }

        public static IEnumerable<T> GetChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in GetChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        public static IEnumerable<T> GetRandomPercent<T>(this IEnumerable<T> source, int percent)
        {
            return source.OrderBy(arg => Guid.NewGuid()).Take(percent);
        }

        public static IEnumerable<string> SplitSentence(this string source)
        {
            if (!source.Contains(' '))
                return new List<string>() { source };

            var words = new List<string>();
            var actualWord = string.Empty;
            //Console.WriteLine("sen original: " + source);

            foreach (char c in source)
            {
                if (char.IsLetter(c))
                    actualWord += c;
                else
                {
                    words.Add(actualWord);
                    actualWord = string.Empty;

                    if (c != ' ')
                        actualWord = c.ToString();
                }
            }
            words.Add(actualWord);

            //Console.WriteLine("sen modified: ");
            //foreach (var w in words)
            //    Console.Write(" " + w);

            return words;
        }

        public static string ReplaceIgnoreCase(this string source, string toReplace, string replacement)
        {
            return Regex.Replace(source, toReplace, replacement, RegexOptions.IgnoreCase);
        }

        public static string GetUntil(this string text, string stopAt)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }

        public static string GetFrom(this string text, string startAt)
        {
            return text.Substring(text.LastIndexOf(startAt) + 1);
        }
    }
}
