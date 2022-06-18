using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public class SpellFilter : QuestionFilter, IFilter
    {
        public SpellFilter() : base (QuestControl.Get(Model.Spell))
        {

        }

        public void SetSort(SortLbl sort, StackPanel stk_items)
        {
            base.SetSort(sort);

            switch (sort)
            {
                case SortLbl.Words:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.Cast<SpellVM>().OrderBy(q => q.Text.FirstOrDefault());
                    else
                        Filtered_quests = Filtered_quests.Cast<SpellVM>().OrderByDescending(q => q.Text);
                    break;
            }

            BuildStack(stk_items);
        }

        public new void Filter(IQuestWpfHeader header)
        {
            var wpf_header = header as SpellWpfHeader;

            base.Filter(wpf_header);

            var filtered_Spells = Filtered_quests.Cast<SpellVM>();

            if (!wpf_header.Txt_words.IsEmpty())
            {
                if (wpf_header.Txt_words.Text.IsDigitsOnly() && filtered_Spells.Any(x => x.Id == Convert.ToInt16(wpf_header.Txt_words.Text)))
                    filtered_Spells = filtered_Spells.Where(q => q.Id == Convert.ToInt16(wpf_header.Txt_words.Text));
                else
                    filtered_Spells = filtered_Spells.Where(q => q.Text.Contains(wpf_header.Txt_words.Text));
            }

            Filtered_quests = filtered_Spells;

            BuildStack(wpf_header.Stk_items);
        }

        public void BuildStack(StackPanel stk_items)
        {
            var watcher = new Stopwatch();
            watcher.Start();

            stk_items.Children.Clear();

            foreach (SpellVM Spell in Filtered_quests.Take(30))
                SpellWpfController.AddIntoItems(stk_items, Spell, false);

            Footer.Log("Showing " + Filtered_quests.Take(30).Count() + " spellings of a total of " + Filtered_quests.Count() + 
                       ". Loaded in " + watcher.Elapsed.TotalSeconds + " seconds.");
        }

    }
}
