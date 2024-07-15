using System.Windows.Input;

namespace AsyncCommands.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await ExcuteAsync(parameter);
        }

        protected abstract Task ExcuteAsync(object parameter);
    }
}
