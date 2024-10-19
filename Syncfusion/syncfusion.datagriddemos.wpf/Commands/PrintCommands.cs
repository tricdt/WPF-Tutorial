
using Syncfusion.UI.Xaml.Grid;
using System.Windows.Input;

namespace syncfusion.datagriddemos.wpf
{
    public static class PrintCommands
    {
        static PrintCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(PrintPreview, OnPrintGrid));
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(DirectPrint, OnDirectPrintGrid));
        }

        #region Print Preview Command

        public static RoutedCommand PrintPreview = new RoutedCommand("PrintPreview", typeof(SfDataGrid));

        private static void OnPrintGrid(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            if (dataGrid == null) return;
            try
            {
                dataGrid.ShowPrintPreview();
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Print Command

        public static RoutedCommand DirectPrint = new RoutedCommand("DirectPrint", typeof(SfDataGrid));

        private static void OnDirectPrintGrid(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            if (dataGrid == null) return;
            try
            {
                dataGrid.Print();
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}
