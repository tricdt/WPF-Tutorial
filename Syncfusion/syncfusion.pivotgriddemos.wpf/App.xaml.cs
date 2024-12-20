﻿using syncfusion.demoscommon.wpf;
using System.Windows;

namespace syncfusion.pivotgriddemos.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow(new PivotGridDemosViewModel());
            window.Show();
            base.OnStartup(e);
        }
    }

}
