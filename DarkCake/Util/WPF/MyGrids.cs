using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AussieCake.Util.WPF
{
    public static class MyGrids
    {
        public static Grid GetRowItem(List<int> ColumnSizes, StackPanel parent)
        {
            var rowGrid = GetRow(ColumnSizes, parent);
            rowGrid.Margin = new Thickness(1, 2, 1, 0);

            return rowGrid;
        }

        public static Grid GetRow(List<int> ColumnSizes, StackPanel parent)
        {
            var rowGrid = Get(ColumnSizes, 1, parent);

            return rowGrid;
        }

        public static Grid GetRow(int row, int Column, Grid parent, List<int> ColumnSizes)
        {
            var rowGrid = Get(row, Column, parent, ColumnSizes, 1);

            return rowGrid;
        }

        public static Grid GetRow(Grid reference, int row, int Column, Grid parent, List<int> ColumnSizes)
        {
            SetColumns(reference, ColumnSizes);
            UtilWPF.SetGridPosition(reference, row, Column, parent);
            reference.RowDefinitions.Add(new RowDefinition());

            return reference;
        }

        private static void SetColumns(Grid reference, List<int> ColumnSizes)
        {
            foreach (var size in ColumnSizes)
            {
                reference.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(size, GridUnitType.Star)
                });
            }
        }

        public static Grid Get(int row, int Column, Grid parent, List<int> ColumnSizes, int rowQuantity)
        {
            var grid = Get(ColumnSizes, rowQuantity);
            UtilWPF.SetGridPosition(grid, row, Column, parent);

            return grid;
        }

        public static Grid Get(int row, int Column, Grid parent, int rowQuantity)
        {
            return Get(row, Column, parent, new List<int>() { 1 }, rowQuantity);
        }

        public static Grid Get(List<int> ColumnSizes, int rowQuantity, StackPanel parent)
        {
            var grid = Get(ColumnSizes, rowQuantity);
            parent.Children.Add(grid);

            return grid;
        }

        public static Grid Bulk_Insert(Grid reference, Grid parent)
        {
            SetColumns(reference, new List<int>() { 9, 1 });
            reference.RowDefinitions.Add(new RowDefinition());
            reference.RowDefinitions.Add(new RowDefinition());
            reference.RowDefinitions.Add(new RowDefinition());
            reference.Margin = new Thickness(2, 0, 2, 0);
            UtilWPF.SetGridPosition(reference, 0, 0, parent);
            reference.Visibility = Visibility.Collapsed;
            reference.Background = UtilWPF.Vocour_header;

            return reference;
        }

        private static Grid Get(List<int> ColumnSizes, int rowQuantity)
        {
            var grid = new Grid();

            SetColumns(grid, ColumnSizes);

            for (int i = 1; i <= rowQuantity; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            return grid;
        }

        public static Grid GetChallenge(int row, Grid parent)
        {
            var grid = Get(row, 0, parent, 4);
            grid.Background = UtilWPF.Vocour_row_off;
            grid.SnapsToDevicePixels = true;
            grid.Margin = new Thickness(0, 2, 2, 0);

            return grid;
        }
    }
}
