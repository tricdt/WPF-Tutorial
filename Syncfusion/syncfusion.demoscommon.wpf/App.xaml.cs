using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            base.OnStartup(e);
        }
    }

}
