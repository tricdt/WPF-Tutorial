
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Input;

namespace syncfusion.datagriddemos.wpf
{
    public static class CustomPrintCommands
    {
        static CustomPrintCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(PrintPreview, OnPrintGrid));
        }

        #region Print Preview Command

        public static RoutedCommand PrintPreview = new RoutedCommand("PrintPreview", typeof(SfDataGrid));

        private static void OnPrintGrid(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            if (dataGrid == null) return;
            try
            {
                var window = new PreviewWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };

                window.PrintPreviewArea.PrintManagerBase = new CustomPrintManager(dataGrid);
                window.ShowDialog();
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}
