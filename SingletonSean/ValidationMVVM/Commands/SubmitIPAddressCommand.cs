using System.Net;
using System.Windows;
using ValidationMVVM.ViewModels;

namespace ValidationMVVM.Commands
{
    public class SubmitIPAddressCommand : CommandBase
    {
        private readonly IPAddressViewModel _viewModel;
        public SubmitIPAddressCommand(IPAddressViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            if (IPAddress.TryParse(_viewModel.IPAddressInput, out IPAddress ipAddress))
            {
                MessageBox.Show($"Valid IP: {ipAddress}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Invalid IP address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
