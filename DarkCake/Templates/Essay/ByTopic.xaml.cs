using AussieCake.Util;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AussieCake.Templates
{
    /// <summary>
    /// Interaction logic for ByTopic.xaml
    /// </summary>
    public partial class ByTopic : UserControl
    {
        public ByTopic()
        {
            InitializeComponent();

            LoadAnswers();
            LoadTemplate();
        }

        string Answer_original = string.Empty;
        string Answer_cause_solution = string.Empty;

        List<Statment> Statements = new List<Statment>()
        {
            new Statment("Some say that music is as important as other subjects in schools, especially at the preschool level. Do you agree or disagree? Give your opinion.", 1),
            new Statment("Do you think English will remain a global language despite globalisation? Discuss your views with examples", 1),
            new Statment("Should Universities Impose Penalties on Students for late submission of assignments? Discuss.", 1),
            new Statment("Most high-level jobs are done by men. Should governments encourage that a certain percentage of these jobs be reserved for women? What is your opinion?", 1),
            new Statment("More and more wild animals are on the verge of extinction and many are endangered. What are the causes of it and what measures can be taken to solve it?", 2),
            new Statment("Involvement of youth in crimes is increasing at an alarming rate. Throw some light on the causes and possible solutions?", 2),
            new Statment("Large shopping malls are replacing small shops. Discuss.", 1),
            new Statment("In recent years, more and more people are choosing to read eBooks rather than paper books. Do the advantages outweigh disadvantages?", 1),
            new Statment("Work experience is more important than the academic qualification, do you agree?", 1),
            new Statment("Whether design of building will have a positive or negative impact on people's life and work, discuss.", 1),
        };

        Statment Actual_statment; 

        private void LoadAnswers()
        {
            Answer_original = string.Join(" ", TemplateEssay.Words.Select(z => z.Text))
                                    .Replace(Templates.Template.Paragraph + " ", "\n")
                                    .Replace(" ,", ",")
                                    .Replace(" .", ".")
                                    .Replace("importance", "effects")
                                    .Replace("reasons why", "reasons for")
                                    .Replace("only for", "only to")
                                    .Replace("i2-so", "i2-does");

            Answer_cause_solution = Answer_original.Replace("effects", "causes")
                                                   .Replace("some favourable", "some debatable")
                                                   .Replace("how i2", "how by i2 this can be resolved")
                                                   .Replace("believe that topic has", "believe that topic can be addressed with")
                                                   .Replace("not only to", "not only beneficial to")
                                                   .Replace("adopt an opposing view and", "");
        }

        private void LoadTemplate()
        {
            Actual_statment = Statements.PickRandom();
            if (Actual_statment.Answer != 2)
                Actual_statment = Statements.PickRandom();
            if (Actual_statment.Answer != 2)
                Actual_statment = Statements.PickRandom();

            lblTopic.Text = Actual_statment.Question;
            txtTemplate.Text = Answer_original;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (Actual_statment.Answer == 1)
                CreateResult(Answer_original);
            else if (Actual_statment.Answer == 2)
                CreateResult(Answer_cause_solution);
        }

        private void CreateResult(string answer)
        {
            if (txtTemplate.Text == answer)
                txtTemplate.Background = Brushes.LightGreen;
            else
            {
                txtTemplate.Background = Brushes.LightSalmon;
                txtTemplate.Text += "\n\n effects -> causes" +
                                    "\n some favourable -> some debatable" +
                                    "\n how i2 -> how by i2 this can be resolved" +
                                    "\n believe that topic has -> believe that topic can be addressed with" +
                                    "\n not only to -> not only beneficial to";

                var missing = txtTemplate.Text.SplitSentence().Intersect(answer.SplitSentence());
                txtTemplate.Text += "\n\n" + string.Join(";", missing);
            } 

            txtTemplate.IsReadOnly = true;
            btnCheck.IsEnabled = false;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            LoadTemplate();

            txtTemplate.IsReadOnly = false;
            btnCheck.IsEnabled = true;
            txtTemplate.Background = Brushes.White;
        }

        internal class Statment
        {
            public string Question { get; set; }
            public int Answer { get; set; }

            public Statment(string text, int answer)
            {
                Question = text;
                Answer = answer;
            }
        }
    }
}
