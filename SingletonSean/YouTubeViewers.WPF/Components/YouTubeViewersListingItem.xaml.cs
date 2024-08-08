using System.Windows;
using System.Windows.Controls;

namespace YouTubeViewers.WPF.Components
{
    /// <summary>
    /// Interaction logic for YouTubeViewersListingItem.xaml
    /// </summary>
    public partial class YouTubeViewersListingItem : UserControl
    {
        public YouTubeViewersListingItem()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dropdown.IsOpen = false;
        }
    }
}
