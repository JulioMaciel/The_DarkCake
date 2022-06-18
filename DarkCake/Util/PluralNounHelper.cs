using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AussieCake.Util
{
    class PluralNounHelper
    {
        #region Exceptional nouns
        public static string[] ExceptionWords_DirectAddS =
        {
            "canto", "solo","piano","lasso","halo", "memento","albino","sirocco",
            "chief","fife","mischief","hoof","roof","grief","kerchief","safe"
        };

        public static string[] ExceptionWords_IrregularInput = 
        {
            "man", "foot", "mouse","woman","tooth","louse","child","ox","goose"
        };
        public static string[] ExceptionWords_IrregularOutput = 
        {
            "men","feet","mice","women","teeth","lice","children","oxen","geese"
        };

        public static string[] ExceptionWords_NoPlural = 
        {
            "gold","silver","wheat","corn","molasses","copper","sugar","cotton"
        };
        #endregion

        public static string GetPlural(string NounString)
        {
            NounString = NounString.Trim();

            foreach (var str in ExceptionWords_DirectAddS)
            {
             if(string.Compare(str,NounString,true, CultureInfo.InvariantCulture) == 0)
                 return NounString + "s";
            }
            
            for(int i=0;i<ExceptionWords_IrregularInput.Length;i++)
            {
                if (string.Compare(ExceptionWords_IrregularInput[i], NounString, true, CultureInfo.InvariantCulture) == 0)
                    return ExceptionWords_IrregularOutput[i];
            }

            foreach (var str in ExceptionWords_NoPlural)
            {
             if(string.Compare(str,NounString,true, CultureInfo.InvariantCulture) == 0)
                 return null;
            }
            

            //see also http://www.lovetolearnplace.com/Grammar/singular&pluralnouns.html#anchor1709890
            //Nouns ending in s, z, x, sh, and ch form the plural by adding - es
            //Nouns ending in - y preceded by a consonant is formed into a plural by changing - y to - ies.  
            //Nouns ending in y preceded by a vowel form their plurals by adding - s.  
            //      Example:  boy, boys; day, days
            //Most nouns ending in o preceded by a consonant is formed into a plural by adding es
            //Some nouns ending in f or fe are made plural by changing f or fe to - ves.  
            //      Example:  beef, beeves; wife, wives

            Regex g = new Regex(@"s\b|z\b|x\b|sh\b|ch\b");
            MatchCollection matches = g.Matches(NounString);
            if (matches.Count > 0)
                NounString += "es"; //Sketches
            else if (NounString.EndsWith("y", true, CultureInfo.InvariantCulture))
            {
                Regex g2 = new Regex(@"(ay|ey|iy|oy|uy)\b");
                if (g2.Matches(NounString).Count <= 0) //e.g. cities 
                    NounString = NounString.Substring(0, NounString.Length - 1) + "ies";
                else
                    NounString += "s";
            }
            else if (NounString.EndsWith("o", true, CultureInfo.InvariantCulture))
            {
                Regex g3 = new Regex(@"(ao|eo|io|oo|uo)\b");
                if (g3.Matches(NounString).Count <= 0) //e.g. heroes 
                    NounString += "es";
                else
                    NounString += "s";
            }
            else if (NounString.EndsWith("f", true, CultureInfo.InvariantCulture) && NounString.Length >= 1)
            {
                NounString = NounString.Substring(0, NounString.Length - 1) + "ves";
            }
            else if (NounString.EndsWith("fe", true, CultureInfo.InvariantCulture) && NounString.Length >= 2)
                NounString = NounString.Substring(0, NounString.Length - 2) + "ves";
            else
                NounString +="s";
            
            return NounString;
        }

        public static bool Test()
        {
            string[] Input = {
                                 "lamp", "cat", "fork", "flower", "pen",
                                 "moss", "buzz","box","dish","church",
                                 "lady", "city","army", "boy","day",
                                 "hero","grotto","beef", "wife",
                                 "canto", "solo","piano","lasso","halo", "memento","albino","sirocco",
                                 "chief","fife","mischief","hoof","roof","grief","kerchief","safe",
                                 "man", "foot", "mouse","woman","tooth","louse","child","ox","goose",
                                 //"golds"
                                 };
            
            string[] Output ={
                                 "lamps", "cats","forks", "flowers","pens",
                                 "mosses","buzzes","boxes","dishes","churches",
                                 "ladies","cities", "armies", "Boys", "days",
                                 "heroes","grottoes","beeves","wives",
                                 "cantos", "solos","pianos","lassos","halos", "mementos","albinos","siroccos",
                                 "chiefs","fifes","mischiefs","hoofs","roofs","griefs","kerchiefs","safes",
                                 "men","feet","mice","women","teeth","lice","children","oxen","geese",
                                 //"golds"
                             };
            
            for(int i=0;i<Input.Length;i++)
            {
                
                string str = GetPlural(Input[i]);
                if(string.Compare(str,Output[i], true,CultureInfo.InvariantCulture)!=0)
                {
                    Console.WriteLine("Error for the word {0}", Input[i]);
                    return false;
                }
            }
            return true;
        }
    }
}
