using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public class PronFilter : QuestionFilter, IFilter
    {
        public PronFilter() : base (QuestControl.Get(Model.Pron))
        {

        }

        public void SetSort(SortLbl sort, StackPanel stk_items)
        {
            base.SetSort(sort);

            switch (sort)
            {
                case SortLbl.Words:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<PronVM>().OrderBy(q => q.Text.FirstOrDefault());
                    else
                        Filtered_quests = Filtered_quests.Cast<PronVM>().OrderByDescending(q => q.Text);
                    break;
                case SortLbl.Answer:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<PronVM>().OrderBy(q => q.Phonemes);
                    else
                        Filtered_quests = Filtered_quests.Cast<PronVM>().OrderByDescending(q => q.Phonemes);
                    break;
            }

            BuildStack(stk_items);
        }

        public new void Filter(IQuestWpfHeader header)
        {
            var wpf_header = header as PronWpfHeader;

            base.Filter(wpf_header);

            var filtered_Prons = Filtered_quests.Cast<PronVM>();

            if (!wpf_header.Txt_words.IsEmpty())
            {
                if (wpf_header.Txt_words.Text.IsDigitsOnly() && filtered_Prons.Any(x => x.Id == Convert.ToInt16(wpf_header.Txt_words.Text)))
                    filtered_Prons = filtered_Prons.Where(q => q.Id == Convert.ToInt16(wpf_header.Txt_words.Text));
                else
                    filtered_Prons = filtered_Prons.Where(q => q.Text.Contains(wpf_header.Txt_words.Text));
            }

            if (!wpf_header.Txt_phonemes.IsEmpty())
                filtered_Prons = filtered_Prons.Where(q => q.Phonemes.Contains(wpf_header.Txt_phonemes.Text));

            Filtered_quests = filtered_Prons;

            BuildStack(wpf_header.Stk_items);
        }

        public void BuildStack(StackPanel stk_items)
        {
            var watcher = new Stopwatch();
            watcher.Start();

            stk_items.Children.Clear();

            foreach (PronVM Pron in Filtered_quests.OrderBy(q => q.Text).Take(30))
                PronWpfController.AddIntoItems(stk_items, Pron, false);

            Footer.Log("Showing " + Filtered_quests.Take(30).Count() + " Pronunciation of a total of " + Filtered_quests.Count() + 
                       ". Loaded in " + watcher.Elapsed.TotalSeconds + " seconds.");
        }

    }
}
