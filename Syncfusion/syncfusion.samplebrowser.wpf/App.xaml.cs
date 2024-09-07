using syncfusion.demoscommon.wpf;
using System.Windows;
namespace syncfusion.samplebrowser.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow(new SamplesViewModel());
            window.Show();
            base.OnStartup(e);
        }
    }

}
