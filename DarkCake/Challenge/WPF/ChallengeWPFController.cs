using AussieCake.Attempt;
using AussieCake.Question;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AussieCake.Challenge
{
    public static class ChalWPFControl
    {
        public static ChalLine CreateChalLine(IQuest quest, int row, Grid userControlGrid, Microsoft.Office.Interop.Word.Application wordApp = null)
        {
            var line = new ChalLine();
            line.Quest = quest;

            line.Chal.Grid_chal = MyGrids.GetChallenge(row, userControlGrid);

            MyStacks.Get(line.Chal.Row_1, 0, 0, line.Chal.Grid_chal);
            line.Chal.Row_1.Visibility = Visibility.Collapsed;

            MyLbls.Chal_answer(line.Chal.Answer, line.Chal.Row_1, quest.Text);

            if (quest is VocModel voc)
            {
                MyLbls.Get(line.Chal.PtBr, line.Chal.Row_1, voc.PtBr);
                line.Chal.PtBr.Foreground = Brushes.DarkBlue;
                MyLbls.Get(line.Chal.Definition, line.Chal.Row_1, voc.Definition);
            }

            var stk_2 = BuildSenChal(line, wordApp);
            UtilWPF.SetGridPosition(stk_2, 1, 0, line.Chal.Grid_chal);

            MyGrids.GetRow(line.Chal.Row_3, 2, 0, line.Chal.Grid_chal, new List<int>() { 1, 1, 1, 1, 1, 1 });
            line.Chal.Row_3.Visibility = Visibility.Collapsed;
            MyLbls.AvgScore(line.Chal.Avg_w, 0, 0, line.Chal.Row_3, line.Quest, 7, false);
            MyLbls.AvgScore(line.Chal.Avg_m, 0, 1, line.Chal.Row_3, line.Quest, 30, false);
            MyLbls.AvgScore(line.Chal.Avg_all, 0, 2, line.Chal.Row_3, line.Quest, 2000, false);
            MyLbls.Tries(line.Chal.Tries, 0, 3, line.Chal.Row_3, line.Quest);
            MyLbls.Chance(line.Chal.Chance, 0, 5, line.Chal.Row_3, line.Quest);
            line.Chal.Chance.Content.ToString().Insert(0, "was ");

            MyGrids.GetRow(line.Chal.Row_4, 3, 0, line.Chal.Grid_chal, new List<int>() { 2, 1, 2, 1, 2 });
            line.Chal.Row_4.Visibility = Visibility.Collapsed;

            MyBtns.Chal_remove_att(line);
            MyLbls.Chal_quest_id(line, 2);
            MyBtns.Chal_disable_quest(line);

            return line;
        }

        private static StackPanel BuildSenChal(ChalLine line, Microsoft.Office.Interop.Word.Application wordApp)
        {
            var stk_sentence = line.Chal.Row_2;
            stk_sentence.Orientation = Orientation.Horizontal;
            stk_sentence.VerticalAlignment = VerticalAlignment.Center;
            stk_sentence.HorizontalAlignment = HorizontalAlignment.Center;

            if (line.Quest.Type == Model.Voc)
                line.Chal.Cb_Answer = CreateSentence(line, stk_sentence, wordApp) as ComboChallenge;
            else if (line.Quest.Type == Model.Spell)
                line.Chal.Txt_Spell = CreateSentence(line, stk_sentence, wordApp) as TextBox;

            return stk_sentence;
        }

        private static Control CreateSentence(ChalLine line, StackPanel parent, Microsoft.Office.Interop.Word.Application wordApp)
        {
            var ctrl = new Control();

            string sentence = Sentences.GetSentenceToQuestion(line.Quest);

            var invalid_synonyms = GetInvalidSynonyms(line.Quest);

            //Console.WriteLine(sentence);

            var found = false;

            foreach (var word in sentence.SplitSentence())
            {
                if (found)
                {
                    CreateDefaultLabel(line, parent, word);
                    continue;
                }

                if (line.Quest.Type == Model.Voc)
                {
                    string answer = (line.Quest as VocVM).Answer;
                    var answers_compatibility = Sentences.GetCompatibleWord(answer, word);

                    if (answers_compatibility.Length > 0)
                    {
                        ctrl = new ComboChallenge();
                        MyCbBxs.BuildSynonyms(answers_compatibility, invalid_synonyms, ctrl as ComboChallenge,
                                              parent, char.IsUpper(word[1]), wordApp);
                        found = true;
                    }
                    else CreateDefaultLabel(line, parent, word);
                }
                else if (line.Quest.Type == Model.Spell)
                {
                    string text = (line.Quest as SpellVM).Text;


                    if (word.ContainsInsensitive(text))
                    {
                        ctrl = new TextBox();
                        ctrl.VerticalContentAlignment = VerticalAlignment.Center;
                        ctrl.Margin = new Thickness(1, 0, 1, 0);
                        ctrl.Width = text.Length * 5 + 26;

                        ctrl.GotFocus += (source, e) =>
                        {
                            var hasAudio = FileHtmlControls.PlayPronunciation(text, ctrl);
                            //if (!hasAudio) ctrl.BorderBrush = Brushes.LightSalmon;
                        };
                        ctrl.KeyDown += (source, e) =>
                        {
                            if (e.Key == System.Windows.Input.Key.Enter && line.Quest.Type == Model.Spell)
                                FileHtmlControls.PlayPronunciation(text, ctrl);
                        };

                        if (!text.EqualsNoCase(word))
                        {
                            //Console.WriteLine("answer was " + text);
                            var dif = word.ReplaceIgnoreCase(text, "");
                            (ctrl as TextBox).Text = dif;
                        }

                        parent.Children.Add(ctrl);
                        found = true;
                    }
                    else
                    {
                        CreateDefaultLabel(line, parent, word);
                    }
                }
            }

            return ctrl;
        }

        //private static void PlayPronunciationOrScramble(ChalLine line, Control ctrl, string text)
        //{
        //    var hasPlayedAudio = FileHtmlControls.PlayPronunciation(text, ctrl);
        //    if (!hasPlayedAudio)
        //    {
        //        (ctrl as TextBox).Background = Brushes.Snow;
        //        (ctrl as TextBox).ToolTip = "No pronunciation found!";
        //        (ctrl as TextBox).Text = line.Quest.Text.Scramble();
        //    }
        //}

        private static void CreateDefaultLabel(ChalLine line, StackPanel parent, string word)
        {
            var lbl = new Label();
            lbl.Margin = new Thickness(-3, 0, -3, 0);
            lbl.Content = word;
            parent.Children.Add(lbl);
            line.Chal.Quest_words.Add(lbl);
        }

        private static List<string> GetInvalidSynonyms(IQuest quest)
        {
            var invalid_synonyms = new List<string>();

            if (quest.Type == Model.Voc)
            {
                foreach (VocVM Voc in QuestControl.Get(Model.Voc))
                {
                    if (Voc.Text.Contains((quest as VocVM).Answer))
                    {
                        var words = Voc.Text.SplitSentence();
                        invalid_synonyms.AddRange(words);
                    }
                }


            }

            return invalid_synonyms;
        }

        public static void Verify(ChalLine line, Button btn_verify, Button btn_next)
        {
            line.Chal.Cb_Answer.IsEnabled = false;

            int score = 0;
            bool isCorrect = false;

            if (line.Quest.Type == Model.Voc)
                isCorrect = line.Chal.Cb_Answer.IsCorrect();
            else if (line.Quest.Type == Model.Spell)
                isCorrect = line.Chal.Txt_Spell.Text.ContainsInsensitive(line.Quest.Text);

            if (isCorrect)
            {
                line.Chal.Grid_chal.Background = UtilWPF.Colour_Correct;
                score = 10;
            }
            else
                line.Chal.Grid_chal.Background = UtilWPF.Colour_Incorrect;

            var att = new AttemptVM(line.Quest.Id, score, DateTime.Now, line.Quest.Type);
            AttemptsControl.Insert(att);

            var updated_quest = QuestControl.Get(line.Quest.Type).First(x => x.Id == line.Quest.Id);

            line.Chal.Avg_w.Content = updated_quest.Avg_week + "% (w)";
            line.Chal.Avg_m.Content = updated_quest.Avg_month + "% (m)";
            line.Chal.Avg_all.Content = updated_quest.Avg_all + "% (all)";
            line.Chal.Tries.Content = updated_quest.Tries.Count + " tries";

            foreach (var lbl in line.Chal.Quest_words)
            {
                if (line.Quest.Text.SplitSentence().Contains(lbl.Content.ToString()))
                    lbl.FontWeight = FontWeights.Bold;
            }

            TurnElemsVisible(line);
            btn_next.IsEnabled = true;
            btn_verify.IsEnabled = false;
        }

        private static void TurnElemsVisible(ChalLine line)
        {
            line.Chal.Row_1.Visibility = Visibility.Visible;
            line.Chal.Row_3.Visibility = Visibility.Visible;
            line.Chal.Row_4.Visibility = Visibility.Visible;
        }

        public static void PopulateRows(Grid parent, Model type, List<ChalLine> lines, List<int> actual_chosen,  Microsoft.Office.Interop.Word.Application wordApp = null)
        {
            var watcher = new Stopwatch();
            watcher.Start();

            Footer.Log("Loading...");

            lines.Clear();

            for (int row = 0; row < 4; row++)
            {
                var quest = QuestControl.GetRandomAvailableQuestion(type, actual_chosen);
                actual_chosen.Add(quest.Id);

                var item = CreateChalLine(quest, row, parent, wordApp);
                lines.Add(item);
                Footer.Log("Loading... Challenge " + (row + 1) + " was loaded in " + watcher.Elapsed.TotalSeconds + " seconds.");
            };

            //var firstTxt = parent.GetChildren<TextBox>().FirstOrDefault();
            //firstTxt.Focus();

            Footer.Log("4 challenges loaded in " + watcher.Elapsed.TotalSeconds + " seconds.");
        }

        private static string Scramble(this string s)
        {
            return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
        }
    }

    public class ChalLine
    {
        public ChalWpfItem Chal { get; set; }
        public IQuest Quest { get; set; }

        public ChalLine()
        {
            Chal = new ChalWpfItem();
        }
    }
}
