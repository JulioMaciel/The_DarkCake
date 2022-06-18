using System;
using System.IO;

namespace AussieCake.Util
{
	public static class CakePaths
	{
		//private static string Project = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;// + "\\AussieCake";
		private static readonly string Project = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;//System.Environment.CurrentDirectory;

		private static readonly string Script = "\\Context\\Scripts";

		public static string Database = Project + "\\" + "Cake.sqlite";

		public static string ScriptVocabulary = Project + Script + "\\Insert_Vocabulary.sql";
		public static string ScriptVerbs = Project + Script + "\\Insert_Verbs.sql";

		public static string ResourceHtmlBooks = Project + "\\Resources\\Books\\html";
		public static string ResourceTxtBooks = Project + "\\Resources\\Books\\txt"; //\\Debug

        public static string ResourcePronunciations = Project + "\\Resources\\Pronunciations";
        //public static string ResourcePronunciations = Project + "\\Pronunciations";

        public static string ResourcePearson = Project + "\\Resources\\Pearson";
        //public static string ResourcePearson = Project + "\\Pearson";

        public static string WelcomeCake = Project + "\\Images\\its_a_lie.png";

        public static string DescImgFolder = Project + "\\Images\\DescribeImages\\";

        public static string GetIconPath(string icon)
		{
			return Project + @"\\Images\\Icons\\" + icon + ".ico";
		}

	}
}
