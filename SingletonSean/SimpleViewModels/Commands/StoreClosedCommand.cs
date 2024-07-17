using SimpleViewModels.ViewModels;

namespace SimpleViewModels.Commands
{
    public class StoreClosedCommand : BaseCommand
    {
        private readonly BuyViewModel _buyViewModel;

        public StoreClosedCommand(BuyViewModel buyViewModel)
        {
            _buyViewModel = buyViewModel;
        }

        public override void Execute(object? parameter)
        {
            _buyViewModel.ErrorMessage = "The store is currently closed.";
        }
    }
}
