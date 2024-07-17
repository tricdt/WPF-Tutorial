using System.Windows.Input;

namespace SimpleViewModels.Commands
{
    public delegate ICommand CreateCommand<TViewModel>(TViewModel viewModel);
}
