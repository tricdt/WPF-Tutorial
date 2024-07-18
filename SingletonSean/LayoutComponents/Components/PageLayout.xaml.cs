using System.Windows;
using System.Windows.Controls;

namespace LayoutComponents.Components
{
    /// <summary>
    /// Interaction logic for PageLayout.xaml
    /// </summary>
    public partial class PageLayout : UserControl
    {


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(PageLayout), new PropertyMetadata(string.Empty));


        public PageLayout()
        {
            InitializeComponent();
        }
    }
}
