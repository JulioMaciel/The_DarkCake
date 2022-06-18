
using AussieCake.Util;
using AussieCake.Verb;
using System;
using System.IO;
using System.Linq;

namespace AussieCake.Context
{
    public class ScriptFileCommands
    {

		public static void WriteVerbOnFile(VerbModel verb)
		{
			var actualFile = File.ReadAllLines(CakePaths.ScriptVerbs);

			using (var tw = new StreamWriter(CakePaths.ScriptVerbs, true))
			{
				if (!actualFile.Any(s => s.Contains(verb.Infinitive)))
					tw.WriteLine("insert into Verb values(NULL, '" + verb.Infinitive + "', '" + verb.Past + "', '"
																	+ verb.PastParticiple + "', '" + verb.Person + "', '" + verb.Gerund + "');");
				else
					Console.WriteLine("Verb already stored: " + verb);
			}
		}

        // Vocabulary script é fixo e não tem insert, pq são do pdf do PTE
        // ou seja, sempre que quiser add algo nos scripts que não seja Verb, 
        // faz manualmente, direto no arquivo

        public static string GetStringFromScriptFile(string scriptPath)
        {
            var Voc_lines = File.ReadAllLines(scriptPath);
            var Voc_joined = String.Join(Environment.NewLine, Voc_lines);
            return Voc_joined;
        }
    }
}
