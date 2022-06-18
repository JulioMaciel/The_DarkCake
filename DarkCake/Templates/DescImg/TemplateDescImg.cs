using AussieCake.Question;
using AussieCake.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AussieCake.Templates
{
    public class TemplateDescImg : Template
    {
        public static string GetRndType1Img()
        {
            string[] filePaths = Directory.GetFiles(CakePaths.DescImgFolder, ".", 
                                                    searchOption: SearchOption.TopDirectoryOnly);

            return filePaths.PickRandom();
        }

		public static List<IQuest> Words = new List<IQuest>()
		{
			new DescImgVM(0, "xxx", DescImgType.Stressed),
			new DescImgVM(1, "can", DescImgType.Init),
			new DescImgVM(2, "play", DescImgType.Init),
			new DescImgVM(3, "an"),
			new DescImgVM(4, "important", DescImgType.Stressed),
			new DescImgVM(5, "role"),
			new DescImgVM(6, "and"),
			new DescImgVM(7, "be", DescImgType.Init),
			new DescImgVM(8, "a", DescImgType.Init),
			new DescImgVM(9, "major", DescImgType.Init_Stressed),
			new DescImgVM(10, "aspect"),
			new DescImgVM(11, "that"),
			new DescImgVM(12, "relates"),
			new DescImgVM(13, "to"),
			new DescImgVM(14, "modern", DescImgType.Stressed),
			new DescImgVM(15, "society"),
			new DescImgVM(16, "these"),
			new DescImgVM(17, "days"),
			new DescImgVM(18, ","),
			new DescImgVM(19, "so", DescImgType.Init),
			new DescImgVM(20, "people", DescImgType.Init),
			new DescImgVM(21, "must"),
			new DescImgVM(22, "pay"),
			new DescImgVM(23, "close", DescImgType.Stressed),
			new DescImgVM(24, "attention"),
			new DescImgVM(25, "to"),
			new DescImgVM(26, "it"),
			new DescImgVM(27, ";"),
			new DescImgVM(28, "therefore", DescImgType.Init_Stressed),
			new DescImgVM(29, ","),
			new DescImgVM(30, "the", DescImgType.Init),
			new DescImgVM(31, "importance", DescImgType.Init),
			new DescImgVM(32, "of", DescImgType.Init),
			new DescImgVM(33, "xxx", DescImgType.Stressed),
			new DescImgVM(34, "must"),
			new DescImgVM(35, "not", DescImgType.Stressed),
			new DescImgVM(36, "be"),
			new DescImgVM(37, "overlooked"),
			new DescImgVM(38, "because", DescImgType.Init),
			new DescImgVM(39, "the", DescImgType.Init),
			new DescImgVM(40, "topic", DescImgType.Init),
			new DescImgVM(41, "of", DescImgType.Init),
			new DescImgVM(42, "xxx", DescImgType.Stressed),
			new DescImgVM(43, "will", DescImgType.Init),
			new DescImgVM(44, "often", DescImgType.Init_Stressed),
			new DescImgVM(45, "lead"),
			new DescImgVM(46, "to"),
			new DescImgVM(47, "debates"),
			new DescImgVM(48, "and", DescImgType.Init),
			new DescImgVM(49, "continue", DescImgType.Init_Stressed),
			new DescImgVM(50, "to"),
			new DescImgVM(51, "increase"),
			new DescImgVM(52, "in"),
			new DescImgVM(53, "the"),
			new DescImgVM(54, "future", DescImgType.Stressed),
			new DescImgVM(55, "."),
		};
    }
}
