using AussieCake.Challenge;
using AussieCake.Context;
using AussieCake.Question;
using AussieCake.Templates;
using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AussieCake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SqLiteHelper.InitializeDB();

            ShowWelcomeCake();
        }

        private void ShowWelcomeCake()
        {
            var stk = new StackPanel();
            stk.HorizontalAlignment = HorizontalAlignment.Center;
            stk.VerticalAlignment = VerticalAlignment.Center;

            var welcome_cake = new Image();
            welcome_cake.Source = new BitmapImage(new Uri(CakePaths.WelcomeCake));
            welcome_cake.Height = 256;
            welcome_cake.Width = 256;
            stk.Children.Add(welcome_cake);

            var lbl_welcome = new TextBlock();
            lbl_welcome.HorizontalAlignment = HorizontalAlignment.Center;
            lbl_welcome.FontSize = 30;
            lbl_welcome.Margin = new Thickness(0, 10, 0, 0);
            lbl_welcome.Text = "The cake is a lie!";
            stk.Children.Add(lbl_welcome);

            frame_content.Content = stk;
        }

        private void btnVocabulary_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new Vocabularies();
            ReStyleButtons(sender as Button);
        }

        private void btnVocChallenge_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new VocChallenge();
            ReStyleButtons(sender as Button);
        }

        private void btnEssay_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new Essay();
            ReStyleButtons(sender as Button);
        }

        private void BtnEssayByTopic_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new ByTopic();
            ReStyleButtons(sender as Button);
        }

        private void btnSumRetell_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new SumRetell();
            ReStyleButtons(sender as Button);
        }

        private void btnDescImg_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new DescImg();
            ReStyleButtons(sender as Button);
        }

        private void BtnPronunciation_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new Pronunciations();
            ReStyleButtons(sender as Button);
        }

        private void BtnPronunPract_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new PronunPract();
            ReStyleButtons(sender as Button);
        }

        private void BtnSpelling_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new Spellings();
            ReStyleButtons(sender as Button);
        }

        private void BtnSpellChallenge_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new SpellChallenge();
            ReStyleButtons(sender as Button);
        }

        private void BtnPearsonFIB_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new P_FIB_Challenge();
            ReStyleButtons(sender as Button);
        }

        private void BtnPearsonDit_Click(object sender, RoutedEventArgs e)
        {
            frame_content.Content = new P_Dit_Challenge();
            ReStyleButtons(sender as Button);
        }

        private void ReStyleButtons(Button newBtn)
        {
            if (Menu.GetChildren<Button>().Any(b => !b.IsEnabled))
            {
                var oldBtn = Menu.GetChildren<Button>().FirstOrDefault(b => !b.IsEnabled);
                oldBtn.IsEnabled = true;
                //oldBtn.Background = Brushes.White;
            }

            //newBtn.Background = UtilWPF.GetBrushFromHTMLColor("#ffe6cc");
            newBtn.IsEnabled = false;

            Util.Footer.Log(string.Empty);
        }

        private void BtnLastAudio_Click(object sender, RoutedEventArgs e)
        {
            var originalDir = new DirectoryInfo("C:\\Users\\Juketo\\Downloads");
            var destinationDir = new DirectoryInfo("C:\\Users\\Juketo\\Dropbox\\PTE 2022\\The_AussieCake\\AussieCake\\Resources\\Pearson");

            var lastAudioFile = originalDir.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            if (!lastAudioFile.Name.StartsWith("Write From"))
            {
                Util.Footer.LogError("!lastAudioFile.Name.StartsWith(\"Repeat Sentence\")");
                return;
            }                

            var senFromClipboard = Clipboard.GetText();

            if (senFromClipboard.Length < 30)
            {
                Util.Footer.LogError("senFromClipboard.Length < 30");
                return;
            }

            if (!(senFromClipboard.EndsWith(".") || senFromClipboard.EndsWith("?")))
            {
                Util.Footer.LogError("Sentence doesn't end with '.' or '?'. Try again");
                return;
            }

            var fileWords = senFromClipboard.Split(' ');
            var newName = PearsonTypeHelper.PersonModelToFileAbvMp3(PearsonType.WFD) + "_"
                           + fileWords[0] + "_" + fileWords[1] + "_"
                           + fileWords[fileWords.Count() - 2] + "_" + fileWords.Last();
            newName = new Regex("[^a-zA-Z0-9_]").Replace(newName, "").ToLower();

            var newFullName = destinationDir + "\\" + newName + ".mp3";

            if (File.Exists(newFullName))
            {
                Util.Footer.Log("There is already a file with this name. The file was not created.");
                return;
            }            
                        
            File.Move(lastAudioFile.FullName, newFullName);
            Util.Footer.Log(newFullName);
        }
    }
}
