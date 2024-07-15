using System.Windows.Input;

namespace AsyncCommands.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private bool _isExcuting;

        public bool IsExcuting
        {
            get { return _isExcuting; }
            set
            {
                _isExcuting = value;
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return !IsExcuting;
        }

        public async void Execute(object parameter)
        {
            IsExcuting = true;
            await ExcuteAsync(parameter);
            IsExcuting = false;
        }

        protected abstract Task ExcuteAsync(object parameter);
    }
}
