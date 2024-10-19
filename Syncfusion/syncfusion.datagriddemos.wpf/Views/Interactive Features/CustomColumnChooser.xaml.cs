using Syncfusion.Windows.Shared;
using System.Windows;
//ControlNamespace###

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CustomColumnChooser.xaml
    /// </summary>
    public partial class CustomColumnChooser : ChromelessWindow
    {
        //ControlVariables###
        public CustomColumnChooser(OrderInfoViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            //ControlMethodCall###
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


    }
}
