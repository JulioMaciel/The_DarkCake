using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public class VocFilter : QuestionFilter, IFilter
    {
        public VocFilter() : base (QuestControl.Get(Model.Voc))
        {

        }

        public void SetSort(SortLbl sort, StackPanel stk_items)
        {
            base.SetSort(sort);

            switch (sort)
            {
                case SortLbl.Words:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderBy(q => q.Text.FirstOrDefault());
                    else
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderByDescending(q => q.Text);
                    break;
                case SortLbl.Answer:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderBy(q => q.Answer);
                    else
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderByDescending(q => q.Answer);
                    break;
                case SortLbl.Def:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderBy(q => q.Definition).ToList();
                    else
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderByDescending(q => q.Definition).ToList();
                    break;
                case SortLbl.PtBr:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderBy(q => q.PtBr).ToList();
                    else
                        Filtered_quests = Filtered_quests.Cast<VocVM>().OrderByDescending(q => q.PtBr).ToList();
                    break;
            }

            BuildStack(stk_items);
        }

        public new void Filter(IQuestWpfHeader header)
        {
            var wpf_header = header as VocWpfHeader;

            base.Filter(wpf_header);

            var filtered_Vocs = Filtered_quests.Cast<VocVM>();

            if (!wpf_header.Txt_words.IsEmpty())
            {
                if (wpf_header.Txt_words.Text.IsDigitsOnly() && filtered_Vocs.Any(x => x.Id == Convert.ToInt16(wpf_header.Txt_words.Text)))
                    filtered_Vocs = filtered_Vocs.Where(q => q.Id == Convert.ToInt16(wpf_header.Txt_words.Text));
                else
                    filtered_Vocs = filtered_Vocs.Where(q => q.Text.Contains(wpf_header.Txt_words.Text));
            }

            if (!wpf_header.Txt_answer.IsEmpty())
                filtered_Vocs = filtered_Vocs.Where(q => q.Answer.Contains(wpf_header.Txt_answer.Text));

            if (!wpf_header.Txt_def.Text.IsEmpty())
                filtered_Vocs = filtered_Vocs.Where(q => q.Definition.Contains(wpf_header.Txt_def.Text)).ToList();

            if (!wpf_header.Txt_ptbr.Text.IsEmpty())
                filtered_Vocs = filtered_Vocs.Where(q => q.PtBr.Contains(wpf_header.Txt_ptbr.Text)).ToList();

            Filtered_quests = filtered_Vocs;

            BuildStack(wpf_header.Stk_items);
        }

        public void BuildStack(StackPanel stk_items)
        {
            var watcher = new Stopwatch();
            watcher.Start();

            stk_items.Children.Clear();

            foreach (VocVM Voc in Filtered_quests.Take(30))
                VocWpfController.AddIntoItems(stk_items, Voc, false);

            Footer.Log("Showing " + Filtered_quests.Take(30).Count() + " Vocabulary of a total of " + Filtered_quests.Count() + 
                       ". Loaded in " + watcher.Elapsed.TotalSeconds + " seconds.");
        }

    }
}
