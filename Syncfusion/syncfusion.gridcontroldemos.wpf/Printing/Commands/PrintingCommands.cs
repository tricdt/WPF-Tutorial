﻿using Syncfusion.Windows.Controls.Grid;
using System.Windows.Input;

namespace syncfusion.gridcontroldemos.wpf
{
    public class PrintingCommands
    {

        public static RoutedCommand ShowPrintDialogCommand = new RoutedCommand("ShowPrintDialogCommand", typeof(GridControl));
        public static RoutedCommand PrintCommand = new RoutedCommand("PrintCommand", typeof(GridControl));

        static PrintingCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(GridControl), new CommandBinding(PrintCommand, OnExecutePrint, OnCanExecutePrint));
            CommandManager.RegisterClassCommandBinding(typeof(GridControl), new CommandBinding(ShowPrintDialogCommand, OnExecuteShowPrintDialog, OnCanExecuteShowPrintDialog));
        }

        #region print
        private static void OnExecutePrint(object sender, ExecutedRoutedEventArgs args)
        {
            ScalingOptions option = ScalingOptions.NoScaling;
            if (args.Parameter != null)
                option = GetScaling(args.Parameter.ToString());

            GridControl grid = args.Source as GridControl;
            grid.Model.ActiveGridView.ScalingOptions = option;
            grid.Print();
        }

        private static void OnCanExecutePrint(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
        #endregion

        #region ShowprintDialog
        private static void OnExecuteShowPrintDialog(object sender, ExecutedRoutedEventArgs args)
        {
            ScalingOptions option = ScalingOptions.NoScaling;
            if (args.Parameter != null)
                option = GetScaling(args.Parameter.ToString());

            GridControl grid = args.Source as GridControl;
            grid.ShowPrintDialog((p) =>
            {
                p.ScalingOptions = option; //ScalingOptions.FitAllColumnsonOnePage;
                grid.ScalingOptions = option;
                p.ShowDialog();
            });
        }

        private static void OnCanExecuteShowPrintDialog(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }
        #endregion

        #region Helper

        private static ScalingOptions GetScaling(string selectedItem)
        {
            ScalingOptions option = ScalingOptions.NoScaling;
            switch (selectedItem)
            {
                case "NoScaling":
                    option = ScalingOptions.NoScaling;
                    break;
                case "FitAllColumnsonOnePage":
                    option = ScalingOptions.FitAllColumnsonOnePage;
                    break;
                case "FitAllRowsonOnePage":
                    option = ScalingOptions.FitAllRowsonOnePage;
                    break;
                case "FitGridonOnePage":
                    option = ScalingOptions.FitGridonOnePage;
                    break;
            }
            return option;
        }

        #endregion

    }

}

