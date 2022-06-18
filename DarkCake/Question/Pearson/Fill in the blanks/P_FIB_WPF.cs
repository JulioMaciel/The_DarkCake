using AussieCake.Attempt;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AussieCake.Question
{
    public static class P_FIB_WPF
    {
        public static List<CellPearson> CreateChallengeFromPearsonText(IPearsonVM quest, StackPanel parent)
        {
            var components = quest.Text.Split(' ');
            var lineLenght = 140;

            var templateLines = new List<StackPanel>();
            var cellList = new List<CellPearson>();

            var actualLine = new StackPanel();
            templateLines.Add(actualLine);
            var actualLineLenght = 0.0;

            for (int i = 0; i < components.Count(); i++)
            {

                var item = components[i];
                var nextItem = item;

                if (i != components.Count() - 1)
                    nextItem = components[i + 1];

                actualLineLenght = actualLineLenght + item.Length * 1.5 + 0.5;
                var willNextBreakLine = (actualLineLenght + nextItem.Length * 1.5 + 1) > lineLenght;

                if (willNextBreakLine)
                {
                    actualLine = new StackPanel();
                    templateLines.Add(actualLine);
                    actualLineLenght = 0;
                }

                var cell = new CellPearson(item);
                cellList.Add(cell);
                actualLine.Children.Add(cell.Stk);
            }

            foreach (var stk in templateLines)
            {
                stk.Orientation = Orientation.Horizontal;
                stk.VerticalAlignment = VerticalAlignment.Center;
                parent.Children.Add(stk);
            }

            if (quest.PearsonType != PearsonType.FIB_R && quest.PearsonType != PearsonType.FIB_RW)
            {
                var cutter = 20;

                int totalToAnswer = cellList.Count / cutter;

                for (int i = 1; i <= totalToAnswer; i++)
                {
                    var thisOne = cellList.GetRange(i * cutter - cutter, cutter)
                                          .Where(x => x.Lbl.Text.All(Char.IsLetter) && x.Lbl.Text.Length > 3)
                                          .PickRandom();
                    thisOne.TurnIntoTxt();
                }

                if ((cellList.Count % cutter) > 10)
                    cellList.Last().TurnIntoTxt();
            }

            return cellList;
        }

        public static void CheckAnswers(List<CellPearson> cellList, IQuest quest)
        {
            var total = cellList.Where(x => x.Txt != null);
            var corrects = 0;

            foreach (var item in total)
            {
                if (item.Txt != null)
                {
                    item.Txt.IsReadOnly = true;
                    var answer = string.Empty;

                    if (!item.Txt.IsEmpty())
                        answer = item.Txt.Text;

                    var isCorrect = answer.Equals(item.Answer, StringComparison.OrdinalIgnoreCase);

                    if (isCorrect)
                    {
                        item.Txt.Background = Brushes.LightGreen;
                        corrects = corrects + 1;
                    }
                    else
                    {
                        item.Txt.Background = Brushes.LightSalmon;
                        item.Txt.ToolTip = item.Answer;
                    }
                }
            }

            var percent_corrects = 0;
            if (corrects != 0)
                percent_corrects = (int)Math.Round((double)(100 * corrects) / total.Count());

            var vm = new AttemptVM(quest.Id, percent_corrects, DateTime.Now, quest.Type);
            AttemptsControl.Insert(vm);
        }

        public class CellPearson
        {
            public StackPanel Stk { get; private set; }

            public TextBlock Lbl { get; private set; }
            public TextBox Txt { get; private set; }

            public string Answer { get; private set; }

            public CellPearson(string comp)
            {
                Stk = new StackPanel();
                Stk.Orientation = Orientation.Horizontal;

                Answer = new String(comp.Where(Char.IsLetter).ToArray());

                var rnd = new ThreadSafeRandom();
                if ((!comp.Contains('[') && !comp.Contains(']')) || (
                    ((comp.Contains('[') && !comp.Contains(']')) || (comp.Contains(']') && !comp.Contains('[')))
                        && rnd.Maybe()))
                {
                    Lbl = new TextBlock();
                    Lbl.Text = comp.Replace("[", "").Replace("]", "");
                    Lbl.Margin = new Thickness(0, 0, 3, 2);
                    Lbl.FontSize = 14;
                    Stk.Children.Add(Lbl);
                }
                else
                {
                    Txt = new TextBox();

                    Stk.Children.Add(Txt);

                    if (comp.Contains('.'))
                    {
                        Lbl = new TextBlock();
                        Lbl.Text = ".";
                        Lbl.Margin = new Thickness(-2, 0, 3, 2);
                        Stk.Children.Add(Lbl);
                    }
                    if (comp.Contains(','))
                    {
                        Lbl = new TextBlock();
                        Lbl.Text = ",";
                        Lbl.Margin = new Thickness(-2, 0, 3, 2);
                        Stk.Children.Add(Lbl);
                    }

                    Txt.Width = Answer.Length * 5 + 26;
                    Txt.Margin = new Thickness(0, 0, 3, 2);
                    Txt.PreviewKeyDown += (sender, e) =>
                    {
                        if (e.Key == Key.Space)
                        {
                            var focusDirection = FocusNavigationDirection.Next;
                            var request = new TraversalRequest(focusDirection);
                            var elementWithFocus = Keyboard.FocusedElement as UIElement;
                            if (elementWithFocus != null)
                                elementWithFocus.MoveFocus(request);
                            e.Handled = true;
                        }
                        else if (e.Key == Key.OemQuotes)
                        {
                            var focusDirection = FocusNavigationDirection.Previous;
                            var request = new TraversalRequest(focusDirection);
                            var elementWithFocus = Keyboard.FocusedElement as UIElement;
                            if (elementWithFocus != null)
                                elementWithFocus.MoveFocus(request);
                            e.Handled = true;
                        }
                    };
                }
            }

            public void TurnIntoTxt()
            {
                Lbl.Visibility = Visibility.Collapsed;

                Txt = new TextBox();
                Stk.Children.Add(Txt);

                if (Lbl.Text.Contains('.'))
                {
                    Lbl = new TextBlock();
                    Lbl.Text = ".";
                    Lbl.Margin = new Thickness(-2, 0, 3, 2);
                    Stk.Children.Add(Lbl);
                }
                if (Lbl.Text.Contains(','))
                {
                    Lbl = new TextBlock();
                    Lbl.Text = ",";
                    Lbl.Margin = new Thickness(-2, 0, 3, 2);
                    Stk.Children.Add(Lbl);
                }

                Txt.Width = Answer.Length * 5 + 26;
                Txt.Margin = new Thickness(0, 0, 3, 2);
                Txt.PreviewKeyDown += (sender, e) =>
                {
                    if (e.Key == Key.Space)
                    {
                        var focusDirection = FocusNavigationDirection.Next;
                        var request = new TraversalRequest(focusDirection);
                        var elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                            elementWithFocus.MoveFocus(request);
                        e.Handled = true;
                    }
                    else if (e.Key == Key.OemQuotes)
                    {
                        var focusDirection = FocusNavigationDirection.Previous;
                        var request = new TraversalRequest(focusDirection);
                        var elementWithFocus = Keyboard.FocusedElement as UIElement;
                        if (elementWithFocus != null)
                            elementWithFocus.MoveFocus(request);
                        e.Handled = true;
                    }
                };
            }
        }
    }
}
