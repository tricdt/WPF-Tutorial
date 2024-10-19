using syncfusion.demoscommon.wpf;
using System.Windows;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow(new GridControlDemosViewModel());
            window.Show();
            base.OnStartup(e);
        }
    }

}
