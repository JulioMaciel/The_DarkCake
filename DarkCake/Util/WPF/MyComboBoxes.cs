using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Util.WPF
{
    public static class MyCbBxs
    {
        public static ComboBox Get(ComboBox reference, int row, int Column, Grid parent)
        {
            var cb = Get(reference);
            UtilWPF.SetGridPosition(cb, row, Column, parent);

            return cb;
        }

        public static ComboBox Get(ComboBox reference, StackPanel parent)
        {
            var cb = Get(reference);
            parent.Children.Add(cb);

            return cb;
        }

        private static ComboBox Get(ComboBox reference)
        {
            reference.VerticalContentAlignment = VerticalAlignment.Center;
            reference.Margin = new Thickness(1, 0, 1, 0);

            return reference;
        }

        public static ComboChallenge BuildSynonyms(string word, List<string> invalid_synonyms, ComboChallenge reference, StackPanel parent, bool isFirstUp, Microsoft.Office.Interop.Word.Application wordApp)
        {
            reference.VerticalContentAlignment = VerticalAlignment.Center;
            reference.Margin = new Thickness(1, 0, 1, 0);
            parent.Children.Add(reference);

            var synonyms = FileHtmlControls.GetSynonyms(word, invalid_synonyms, wordApp).ToList();
            if (isFirstUp)
            {
                synonyms.ForEach(x => x.First().ToString().ToUpper());
                word = word.UpperFirst();
            }

            if (!synonyms.Any())
                Errors.ThrowErrorMsg(ErrorType.SynonymsNotFound, word);

            var service = PluralizationService.CreateService(System.Globalization.CultureInfo.CurrentCulture);
            if (service.IsPlural(word))
                for (int i = 0; i < synonyms.Count; i++)
                    synonyms[i] = service.Pluralize(synonyms[i]);

            synonyms.Add(word);

            var shuffled = synonyms.OrderBy(x => Guid.NewGuid()).ToList();

            reference.ItemsSource = shuffled;
            reference.SelectedIndex = 0;
            reference.CorrectIndex = reference.Items.IndexOf(word);

            return reference;
        }

        public static CbModelType ModelOptions (CbModelType reference, bool useAny)
        {
            var source = new List<ModalTypeCb>();

            if (useAny)
                source.Add(new ModalTypeCb("Any Questions"));

            var quests_types = Enum.GetValues(typeof(Model)).Cast<Model>()
                                                       .Where(x => x != Model.Sen && x != Model.Verb);
            foreach (var type in quests_types)
                source.Add(new ModalTypeCb(type, type.ToDesc()));

            reference.ItemsSource = source;
            reference.DisplayMemberPath = "Text";
            reference.SelectedIndex = 0;

            reference.SelectionChanged += (sender, e) => reference.SelectedModalType = ((ModalTypeCb)reference.SelectedValue).Type;

            return reference;
        }

        internal class ModalTypeCb
        {
            public Model Type { get; set; }
            public string Text { get; set; }

            public ModalTypeCb(Model type, string text)
            {
                Type = type;
                Text = text;
            }

            public ModalTypeCb(string text)
            {
                Text = text;
            }
        }
    }

    public class CbModelType : ComboBox
    {
        public Model SelectedModalType { get; set; }
    }

    public class ComboChallenge : ComboBox
    {
        public int CorrectIndex { get; set; }

        public bool IsCorrect()
        {
            return CorrectIndex == SelectedIndex;
        }
    }
}
