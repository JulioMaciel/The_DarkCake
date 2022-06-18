using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AussieCake.Util.WPF
{
    public static class UtilWPF
    {
        public static Brush Vocour_header { get; } = GetBrushFromHTMLColor("#6f93c3"); // azul forte
        public static Brush Vocour_row_on { get; } = GetBrushFromHTMLColor("#cad7e8"); // azul claro
        public static Brush Vocour_row_off { get; } = GetBrushFromHTMLColor("#a5bcd9"); // azul médio
        public static Brush Vocour_new_row_off { get; } = GetBrushFromHTMLColor("#95CAE4"); // azul claro médio
        public static Brush Colour_Incorrect { get; } = GetBrushFromHTMLColor("#e6b3b3");
        public static Brush Colour_Correct { get; } = GetBrushFromHTMLColor("#2fb673");

        public static Brush GetVocourLine(bool is_on, bool isGridUpdate)
        {
            if (is_on)
                return Vocour_row_on;
            else
                return isGridUpdate ? Vocour_new_row_off : Vocour_row_off;
        }

        public static Image GetIconButton(string iconFile)
        {
            var btn_icon = new Image();
            btn_icon.Source = new BitmapImage(new Uri(CakePaths.GetIconPath(iconFile)));
            return btn_icon;
        }

        public static SolidColorBrush GetAvgColor(double percentage)
        {
            if (percentage > 100)
                percentage = 100;

            var red = Color.FromRgb(255, 0, 0);
            var yellow = Color.FromRgb(255, 255, 0);
            var lime = Color.FromRgb(0, 255, 0);

            //var darkRed = Brushes.LightSalmon.Color;
            //var goldenrod = Brushes.LightYellow.Color;
            //var seaGreen = Brushes.LightSeaGreen.Color;
            var darkRed = Color.FromRgb(139, 0, 0);
            var goldenrod = Color.FromRgb(179, 143, 0);
            var seaGreen = Color.FromRgb(26, 101, 64);

            if (percentage < 50)
                return Interpolate(darkRed, goldenrod, percentage / 50.0);

            return Interpolate(goldenrod, seaGreen, (percentage - 50) / 50.0);
        }

        public static Brush GetBrushFromHTMLColor(string hexadecimal)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(hexadecimal));
        }

        private static SolidColorBrush Interpolate(Color Color1, Color Color2, double fraction)
        {
            var c1 = Color1.ColorContext;

            double r = Interpolate(Color1.R, Color2.R, fraction);
            double g = Interpolate(Color1.G, Color2.G, fraction);
            double b = Interpolate(Color1.B, Color2.B, fraction);
            var dColor = System.Drawing.Color.FromArgb((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
            return new SolidColorBrush(Color.FromArgb(dColor.A, dColor.R, dColor.G, dColor.B));
        }

        private static double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d2 - d1) * fraction;
        }

        public static void SetGridPosition(FrameworkElement child, int row, int Column, Grid parent)
        {
            if (parent.Children.Contains(child))
                return;

            Grid.SetRow(child, row);
            Grid.SetColumn(child, Column);
            parent.Children.Add(child);
        }

        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        public static int RandomNumber(int min, int max) => random.Value.Next(min, max);        
    }

    public class ThreadSafeRandom
    {
        private static readonly Random _global = new Random();
        [ThreadStatic] private static Random _local;

        public bool Maybe()
        {
            if (_local == null)
            {
                lock (_global)
                {
                    if (_local == null)
                    {
                        int seed = _global.Next();
                        _local = new Random(seed);
                    }
                }
            }

            return _local.Next(2) == 1;
        }
    }
}
