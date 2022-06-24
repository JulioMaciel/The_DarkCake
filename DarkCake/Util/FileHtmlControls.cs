using AussieCake.Question;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AussieCake.Util
{
    public static class FileHtmlControls
    {
        public static string GetTextFromSite(string url)
        {
            string htmlCode = string.Empty;

            using (WebClient client = new WebClient())
                htmlCode = client.DownloadString(url);

            string cleanedCode = CleanHtmlCode(htmlCode);

            return cleanedCode;
        }

        public static string GetTextFromHtmlBooks()
        {
            string htmlCode = string.Empty;

            string[] filePaths = Directory.GetFiles(CakePaths.ResourceHtmlBooks, "*.htm",
                                                    searchOption: SearchOption.TopDirectoryOnly);
            foreach (var path in filePaths)
                htmlCode += File.ReadAllText(path);

            string cleanedCode = CleanHtmlCode(htmlCode);

            return cleanedCode;
        }

        public static string GetTextFromTxtBooks()
        {
            string allStringBooks = string.Empty;
            string[] filePaths = Directory.GetFiles(CakePaths.ResourceTxtBooks, "*.txt",
                                                    searchOption: SearchOption.TopDirectoryOnly);

            foreach (var path in filePaths)
                allStringBooks += File.ReadAllText(path);

            return allStringBooks;
        }

        public static string CleanHtmlCode(string htmlCode)
        {
            var cleanedCode = WebUtility.HtmlDecode(htmlCode);
            cleanedCode = cleanedCode.NormalizeWhiteSpace();
            cleanedCode = Regex.Replace(cleanedCode, "<.*?>", " ");
            cleanedCode = Regex.Replace(cleanedCode, "\\r|\\n|\\t", "");
            cleanedCode = cleanedCode.Trim(' ');
            return cleanedCode;
        }

        public static IEnumerable<string> GetSynonyms(string word, List<string> invalid_synonyms, Microsoft.Office.Interop.Word.Application wordApp)
        {
            word = word.ToLower();

            var found = new List<string>();
            var theSynonyms = wordApp.get_SynonymInfo(word);

            foreach (var Meaning in theSynonyms.MeaningList as Array)
            {
                if (found.Count >= 4)
                    return found;

                var synonym = Meaning.ToString();
                if (!IsSynonymTooSimilar(word, synonym, found))
                {
                    if (!invalid_synonyms.Contains(synonym))
                    {
                        found.Add(synonym);
                    }
                    else
                        Debug.WriteLine("Synonym " + synonym + " was blocked because it was on the invalid list.");
                }
            }

            for (int ii = 0; ii < found.Count; ii++)
            {
                theSynonyms = wordApp.SynonymInfo[found[ii]];

                foreach (string synonym in theSynonyms.MeaningList as Array)
                {
                    if (found.Count >= 4)
                        return found;

                    if (IsSynonymTooSimilar(word, synonym, found))
                        continue;

                    found.Add(synonym);
                }
            }

            return found;
        }

        private static bool IsSynonymTooSimilar(string word, string synonym, List<string> found)
        {
            var are_bigger_than_5 = synonym.Length > 5 && word.Length > 5;

            var cut_slice = are_bigger_than_5 ? 5 : (word.Length < synonym.Length ? word.Length - 1 : synonym.Length - 1);

            var syn_part = synonym.Substring(0, cut_slice);
            var word_part = word.Substring(0, cut_slice);

            if (syn_part == word_part || found.Any(x => x.StartsWith(syn_part)))
                return true;

            return false;
        }

        public static bool PlayPronunciation(string text, Control ctrl = null)
        {
            string possibleExisting = CakePaths.ResourcePronunciations + "\\" + text + ".mp3";

            if (File.Exists(possibleExisting))
            {
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(possibleExisting))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                //Console.WriteLine("Playing existing mp3 for " + text);
                return true;
            }
            return false;
        }

        public async static Task PlayPearson(IPearsonVM vm, bool firstHalfOnly = false)
        {
            var fileWords = vm.Text.Split(' ');
            var fileName = PearsonTypeHelper.PersonModelToFileAbvMp3(vm.PearsonType) + "_" 
                           + fileWords[0] + "_" + fileWords[1] + "_"
                           + fileWords[fileWords.Count() - 2] + "_" + fileWords.Last();
            fileName = new Regex("[^a-zA-Z0-9_]").Replace(fileName, "").ToLower();

            var filePath = CakePaths.ResourcePearson + "\\" + fileName + ".mp3";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Not found: " + filePath);
                return;
            }

            using (WaveStream blockAlignedStream = 
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(filePath))))
            {
                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                {
                    waveOut.Volume = 1;
                    await Task.Run(() =>
                    {
                        System.Threading.Thread.Sleep(300);
                        waveOut.Init(blockAlignedStream);                        
                        waveOut.Play();

                        if (firstHalfOnly)
                        {
                            var duration = blockAlignedStream.TotalTime;
                            var toDelay = TimeSpan.FromMilliseconds(duration.TotalMilliseconds * 0.55);
                            Task.Delay(toDelay).ContinueWith(t => waveOut.Stop());
                            //Task.Delay(toDelay).ContinueWith(t => waveOut.Volume = waveOut.Volume / 5);
                        }

                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    });
                }
            }
        }

        private static void PlayIt(MatchCollection matches, string word)
        {
            if (matches.Count < 1)
            {
                using (WaveStream blockAlignedStream =
                    new BlockAlignReductionStream(
                        WaveFormatConversionStream.CreatePcmStream(
                            new Mp3FileReader(CakePaths.ResourcePronunciations + "\\quack.mp3"))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
                return;
            }
        }
    }
}
