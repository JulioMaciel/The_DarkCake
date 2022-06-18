using AussieCake.Context;
using AussieCake.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;

namespace AussieCake.Verb
{
    public class VerbsController : SqLiteHelper
    {
        public static IEnumerable<VerbModel> Get()
        {
            if (Verbs == null)
                GetVerbsDB();

            return Verbs;
        }

        public static void Insert(VerbModel verb)
        {
            InsertVerb(verb);
            ScriptFileCommands.WriteVerbOnFile(verb);
        }

        public static List<string> VerbToBe = new List<string>()
        {
            "am", "'m", "is", "'s", "are", "'re", "being", "been", "was", "were"
        };

        public static VerbModel ConjugateUnknownVerb(string verb)
        {
            string htmlCode = string.Empty;

            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("http://conjugator.reverso.net/conjugation-english-verb-" + verb + ".html");
            }

            var newVerb = new VerbModel();
            newVerb.Infinitive = verb;

            if (htmlCode.IsEmpty())
            {
                MessageBox.Show("Site Conjugator doesn't have the verb '" + verb + "'. Operation will continue.");
                return newVerb;
            }

            newVerb.Past = GetVerbAfterTheIndex(htmlCode, "<p>Preterite</p>", "I </i><i class=\"verbtxt\">");
            newVerb.PastParticiple = GetVerbAfterTheIndex(htmlCode, "<h4>Participle</h4>", "<li><i class=\"verbtxt\">", true);
            newVerb.Person = GetVerbAfterTheIndex(htmlCode, "<h4>Indicative</h4>", "it </i><i class=\"verbtxt\">");
            newVerb.Gerund = GetVerbAfterTheIndex(htmlCode, "<p>Present continuous</p>", "am </i><i class=\"verbtxt\">");

            Insert(newVerb);

            return newVerb;
        }

        private static string GetVerbAfterTheIndex(string htmlCode, string time, string initial, bool isPP = false)
        {
            var timeInitial = htmlCode.IndexOf(time);

            if (isPP)
                timeInitial = htmlCode.IndexOf("<p>Past</p>", timeInitial);

            var initialIndex = htmlCode.IndexOf(initial, timeInitial) + initial.Length;
            var finalIndex = htmlCode.IndexOf('<', initialIndex);
            return htmlCode.Substring(initialIndex, finalIndex - initialIndex);
        }
    }
}
