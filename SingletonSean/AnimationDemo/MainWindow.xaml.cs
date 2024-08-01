using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Duration _openCloseDuration = new Duration(TimeSpan.FromSeconds(0.5));

        public MainWindow()
        {
            InitializeComponent();

            dropdownContent.Height = 0;
        }

        private void OpenDropdown(object sender, RoutedEventArgs e)
        {
            dropdownInnerContent.Measure(new Size(dropdownContent.MaxWidth, dropdownContent.MaxHeight));
            DoubleAnimation heightAnimation = new DoubleAnimation(0, dropdownInnerContent.DesiredSize.Height, _openCloseDuration);
            dropdownContent.BeginAnimation(HeightProperty, heightAnimation);
        }

        private void CloseDropdown(object sender, RoutedEventArgs e)
        {
            DoubleAnimation heightAnimation = new DoubleAnimation(0, _openCloseDuration);
            dropdownContent.BeginAnimation(HeightProperty, heightAnimation);
        }
    }
}