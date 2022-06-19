﻿using AussieCake.Util;
using AussieCake.Util.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AussieCake.Templates
{
    /// <summary>
    /// Interaction logic for Essay.xaml
    /// </summary>
    public partial class DescImg : UserControl
    {
        List<CellTemplate> CellList;

        public DescImg()
        {
            InitializeComponent();

            slider.Value = 100;

            CellList = TemplateWPF.BuildTemplate(TemplateDescImg.Words, 0, StkTemplate);
            TemplateWPF.ShowScoredTemplate(CellList, 7);
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

            var result = TemplateWPF.CheckAnswers(CellList);
            lblScore.Content = result.Item1;
            lblScore.Foreground = UtilWPF.GetAvgColor(result.Item2);

            btnFinish.IsEnabled = false;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            imgTemplateType.Source = new BitmapImage(new Uri(TemplateDescImg.GetRndType1Img()));

            CellList = TemplateWPF.BuildTemplate(TemplateDescImg.Words, slider.Value, StkTemplate);

            btnFinish.IsEnabled = true;
            lblScore.Visibility = Visibility.Hidden;
        }

        private void BtnStart_ClickInit(object sender, RoutedEventArgs e)
        {
            imgTemplateType.Source = new BitmapImage(new Uri(TemplateDescImg.GetRndType1Img()));
            CellList = TemplateWPF.BuildInitTemplate(StkTemplate, TemplateDescImg.Words);
            Start();
        }

        private void BtnStart_ClickStressed(object sender, RoutedEventArgs e)
        {
            imgTemplateType.Source = new BitmapImage(new Uri(TemplateDescImg.GetRndType1Img()));
            CellList = TemplateWPF.BuildStressedTemplate(StkTemplate);
            Start();
        }

        private void Start()
        {
            btnFinish.IsEnabled = true;
            lblScore.Visibility = Visibility.Hidden;
        }
    }
}
