using System.Windows;
using syncfusion.demoscommon.wpf;
namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new MainWindow(new DemoBrowserViewModel());
            window.Show();
            base.OnStartup(e);
        }
    }

}
