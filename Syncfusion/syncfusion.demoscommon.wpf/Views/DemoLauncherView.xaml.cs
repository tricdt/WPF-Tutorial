using System.Windows;
using System.Windows.Controls;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Interaction logic for DemoLauncherView.xaml
    /// </summary>
    public partial class DemoLauncherView : UserControl
    {
        Type DemoType;
        public DemoLauncherView(Type DemoViewType)
        {
            InitializeComponent();
            DemoType = DemoViewType;
        }

        public Style HyperLinkStyle
        {
            get { return (Style)GetValue(HyperLinkStyleProperty); }
            set { SetValue(HyperLinkStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HyperLinkStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HyperLinkStyleProperty =
            DependencyProperty.Register("HyperLinkStyle", typeof(int), typeof(DemoLauncherView), new PropertyMetadata(null));

    }
}
