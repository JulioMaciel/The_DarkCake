using AussieCake.Question;
using AussieCake.Util;
using System;
using System.Linq;
using System.Windows.Controls;

namespace AussieCake
{
    public static class QuestWpfUtil
    {
        public static void InsertClick(StackPanel stk_items, IQuestWpfHeader wpf_header)
        {
            if (wpf_header is VocWpfHeader voc)
                InsertClick(stk_items, voc);
            else if (wpf_header is PronWpfHeader pron)
                InsertClick(stk_items, pron);
            else if (wpf_header is SpellWpfHeader spell)
                InsertClick(stk_items, spell);

            Footer.Log("The question has been inserted.");
        }

        private static void InsertClick(StackPanel stk_items, VocWpfHeader wpf_header)
        {
            var Voc = new VocVM(wpf_header.Txt_words.Text,
                wpf_header.Txt_answer.Text,
                wpf_header.Txt_def.Text,
                wpf_header.Txt_ptbr.Text,
                wpf_header.Btn_isActive.IsActived);

            if (QuestControl.Insert(Voc))
            {
                wpf_header.Txt_words.Text = string.Empty;
                wpf_header.Txt_answer.Text = string.Empty;
                wpf_header.Txt_def.Text = string.Empty;
                wpf_header.Txt_ptbr.Text = string.Empty;

                InsertQuestion(stk_items, Model.Voc);
            }
        }

        private static void InsertClick(StackPanel stk_items, PronWpfHeader wpf_header)
        {
            if (wpf_header.Txt_words.Text.IsEmpty() || wpf_header.Txt_phonemes.Text.IsEmpty())
                return;

            var pron = new PronVM(wpf_header.Txt_words.Text,
                wpf_header.Txt_phonemes.Text,
                wpf_header.Btn_isActive.IsActived);

            if (QuestControl.Insert(pron))
            {
                wpf_header.Txt_words.Text = string.Empty;
                wpf_header.Txt_phonemes.Text = string.Empty;

                InsertQuestion(stk_items, Model.Pron);
            }
        }

        private static void InsertClick(StackPanel stk_items, SpellWpfHeader wpf_header)
        {
            var spell = new SpellVM(wpf_header.Txt_words.Text,
                wpf_header.Btn_isActive.IsActived);

            if (QuestControl.Insert(spell))
            {
                wpf_header.Txt_words.Text = string.Empty;
                InsertQuestion(stk_items, Model.Spell);
            }
        }

        private static void InsertQuestion(StackPanel stk_items, Model type)
        {
            var added = QuestControl.Get(type).Last();
            added.LoadCrossData();

            AddWpfItem(stk_items, added);
        }

        public  static void EditClick(VocVM voc, VocWpfItem wpf_item, StackPanel item_line)
        {
            var edited = new VocVM(voc.Id,
                wpf_item.Words.Text,
                wpf_item.Answer.Text,
                wpf_item.Def.Text,
                wpf_item.Ptbr.Text,
                wpf_item.IsActive.IsActived);

            EditQuestion(voc, edited, item_line);
        }

        public static void EditClick(PronVM voc, PronWpfItem wpf_item, StackPanel item_line)
        {
            var edited = new PronVM(voc.Id,
                wpf_item.Words.Text,
                wpf_item.Phonemes.Text,
                wpf_item.IsActive.IsActived);

            EditQuestion(voc, edited, item_line);
        }

        public static void EditClick(SpellVM voc, SpellWpfItem wpf_item, StackPanel item_line)
        {
            var edited = new SpellVM(voc.Id,
                wpf_item.Words.Text,
                wpf_item.IsActive.IsActived);

            EditQuestion(voc, edited, item_line);
        }

        private static void EditQuestion(IQuest quest, IQuest edited, StackPanel item_line)
        {
            if (!QuestControl.Update(edited))
                return;

            edited = QuestControl.Get(quest.Type).Where(q => q.Id == quest.Id).First();
            edited.LoadCrossData();

            UpdateWpfItem(item_line, edited);

            Footer.Log("The question has been edited.");
        }

        public static void AddWpfItem(StackPanel stk_items, IQuest vm)
        {
            if (vm.Type == Model.Voc)
                VocWpfController.AddIntoItems(stk_items, vm as VocVM, true);
            else if (vm.Type == Model.Pron)
                PronWpfController.AddIntoItems(stk_items, vm as PronVM, true);
            else if (vm.Type == Model.Spell)
                SpellWpfController.AddIntoItems(stk_items, vm as SpellVM, true);
        }

        private static void UpdateWpfItem(StackPanel item_line, IQuest vm)
        {
            item_line.Children.Clear();

            if (vm.Type == Model.Voc)
                VocWpfController.AddIntoThis(vm as VocVM, item_line);
            else if (vm.Type == Model.Pron)
                PronWpfController.AddIntoThis(vm as PronVM, item_line);
            else if (vm.Type == Model.Spell)
                SpellWpfController.AddIntoThis(vm as SpellVM, item_line);
        }
    }

}
