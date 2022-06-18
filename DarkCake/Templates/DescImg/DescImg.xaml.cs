using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AussieCake.Templates
{
    /// <summary>
    /// Interaction logic for Essay.xaml
    /// </summary>
    public partial class DescImg : UserControl
    {
        List<CellTemplate> CellList;
        bool openVariablesForFreeAnswer = false;

        double percentageTxt = 100;

        public DescImg()
        {
            InitializeComponent();

            slider.Value = 100;

            CellList = TemplateWPF.BuildTemplate(TemplateDescImg.Words, 0, StkTemplate);
            TemplateWPF.ShowScoredTemplate(CellList, 7);
            //HighlightEssayType();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            btnStart.Content = "Start " + e.NewValue + "%";
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            int lastDays = 99;

            if (!txtFilter.Text.IsEmpty() && txtFilter.Text.IsDigitsOnly() && txtFilter.Text != "0")
                lastDays = Convert.ToInt32(txtFilter.Text);

            CellList = TemplateWPF.BuildTemplate(TemplateDescImg.Words, 0, StkTemplate);
            TemplateWPF.ShowScoredTemplate(CellList, lastDays);

            btnFinish.IsEnabled = false;
            lblScore.Visibility = Visibility.Hidden;
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            lblScore.Visibility = Visibility.Visible;
            if (!openVariablesForFreeAnswer)
            {
                var result = TemplateWPF.CheckAnswers(CellList);
                lblScore.Content = result.Item1;
                lblScore.Foreground = UtilWPF.GetAvgColor(result.Item2);
            }
            else
            {
                var result = TemplateWPF.CheckAnswers(CellList);
                lblScore.Content = result.Item1;
                lblScore.Foreground = UtilWPF.GetAvgColor(result.Item2);
            }

            btnFinish.IsEnabled = false;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            //if (cb_33.IsChecked.Value)
            //    percentageTxt = 33;
            //else
                percentageTxt = 100 - slider.Value;

            imgTemplateType.Source = new BitmapImage(new Uri(TemplateDescImg.GetRndType1Img()));

            CellList = TemplateWPF.BuildTemplate(TemplateDescImg.Words, percentageTxt, StkTemplate);
            //HighlightEssayType();

            btnFinish.IsEnabled = true;
            lblScore.Visibility = Visibility.Hidden;

            openVariablesForFreeAnswer = false;
        }

        private void BtnStart_ClickInit(object sender, RoutedEventArgs e)
        {
            CellList = TemplateWPF.BuildInitTemplate(StkTemplate, TemplateDescImg.Words);

            Start();
        }

        private void BtnStart_ClickStressed(object sender, RoutedEventArgs e)
        {
            CellList = TemplateWPF.BuildStressedTemplate(StkTemplate);

            Start();
        }

        private void Start()
        {
            btnFinish.IsEnabled = true;
            lblScore.Visibility = Visibility.Hidden;
        }

        //private void Cb_33_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Convert.ToBoolean(((CheckBox)sender).IsChecked.Value) == true)
        //    {
        //        slider.IsEnabled = false;
        //        btnStart.Content = "Start 33%";
        //    }
        //    else
        //    {
        //        slider.IsEnabled = true;
        //        percentageTxt = 100;
        //        btnStart.Content = "Start " + slider.Value + "%";
        //    }

        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //btnStart.Content = "Start 33%";
            btnStart.Content = "Start 100%";
        }
    }
}
