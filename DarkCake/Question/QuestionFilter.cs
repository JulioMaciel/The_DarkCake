using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AussieCake.Question
{
    public abstract class QuestionFilter
    {
        private SortLbl Sort { get; set; }
        protected bool IsNextSortAsc { get; set; }

        protected IEnumerable<IQuest> Original_quests { get; set; }
        protected IEnumerable<IQuest> Filtered_quests { get; set; }

        protected QuestionFilter(IEnumerable<IQuest> original)
        {
            foreach (var quest in original)
                quest.LoadCrossData();

            Original_quests = original;
            Filtered_quests = original;

            Sort = SortLbl.Id;
            IsNextSortAsc = false;
        }

        protected void SetSort(SortLbl sort)
        {
            if (sort == Sort)
                IsNextSortAsc = !IsNextSortAsc;
            else
            {
                IsNextSortAsc = false;
                Sort = sort;
            }

            switch (sort)
            {
                case SortLbl.Score_w:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Avg_week).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Avg_week).ToList();
                    break;
                case SortLbl.Score_m:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Avg_month).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Avg_month).ToList();
                    break;
                case SortLbl.Score_all:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Avg_all).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Avg_all).ToList();
                    break;
                case SortLbl.Tries:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Tries.Count()).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Tries.Count()).ToList();
                    break;
                case SortLbl.Chance:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Chance).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Chance).ToList();
                    break;
                default:
                    if (IsNextSortAsc)
                        Filtered_quests = Filtered_quests.OrderBy(q => q.Id).ToList();
                    else
                        Filtered_quests = Filtered_quests.OrderByDescending(q => q.Id).ToList();
                    break;
            }
        }

        protected void Filter(IQuestWpfHeader wpf_header)
        {
            Filtered_quests = Original_quests;

            if (!wpf_header.Txt_avg_w.Text.IsEmpty())
            {
                if (!Errors.IsDigitsOnly(wpf_header.Txt_avg_w.Text))
                    return;

                Filtered_quests = Filtered_quests.Where(q => q.Avg_week == Convert.ToInt16(wpf_header.Txt_avg_w.Text)).ToList();
            }
            if (!wpf_header.Txt_avg_m.Text.IsEmpty())
            {
                if (!Errors.IsDigitsOnly(wpf_header.Txt_avg_m.Text))
                    return;

                Filtered_quests = Filtered_quests.Where(q => q.Avg_month == Convert.ToInt16(wpf_header.Txt_avg_m.Text)).ToList();
            }
            if (!wpf_header.Txt_avg_all.Text.IsEmpty())
            {
                if (!Errors.IsDigitsOnly(wpf_header.Txt_avg_all.Text))
                    return;

                Filtered_quests = Filtered_quests.Where(q => q.Avg_all == Convert.ToInt16(wpf_header.Txt_avg_all.Text)).ToList();
            }
            if (!wpf_header.Txt_tries.Text.IsEmpty())
            {
                if (!Errors.IsDigitsOnly(wpf_header.Txt_tries.Text))
                    return;

                Filtered_quests = Filtered_quests.Where(q => q.Tries.Count == Convert.ToInt16(wpf_header.Txt_tries.Text)).ToList();
            }
            if (!wpf_header.Txt_chance.Text.IsEmpty())
            {
                if (!Errors.IsDigitsOnly(wpf_header.Txt_chance.Text))
                    return;

                Filtered_quests = Filtered_quests.Where(q => q.Chance >= Convert.ToInt16(wpf_header.Txt_chance.Text)).ToList();
            }

            if (!wpf_header.Btn_isActive.IsActived)
                Filtered_quests = Filtered_quests.Where(q => q.IsActive == wpf_header.Btn_isActive.IsActived).ToList();

            //if (wpf_header.Stk_items.Parent is ScrollViewer scroll)
            //    scroll.ScrollToTop();
        }
    }
}
