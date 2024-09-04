using Syncfusion.SfSkinManager;
using System.Windows.Controls;
namespace syncfusion.demoscommon.wpf
{
    public class DemoControl : UserControl, IDisposable
    {

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        public DemoControl()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(DemoControl), new FrameworkPropertyMetadata(typeof(DemoControl)));
            this.DefaultStyleKey = typeof(DemoControl);
        }
        public DemoControl(string themename) : this()
        {
            SfSkinManager.SetTheme(this, new Theme() { ThemeName = themename });
        }
    }
}
