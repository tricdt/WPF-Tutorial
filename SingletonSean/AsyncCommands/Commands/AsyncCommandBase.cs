using System.Windows.Input;

namespace AsyncCommands.Commands
{
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly Action<Exception> _onException;
        private bool _isExcuting;

        protected AsyncCommandBase(Action<Exception> onException)
        {
            _onException = onException;
        }


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
            try
            {
                await ExcuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException.Invoke(ex);
            }
            IsExcuting = false;
        }

        protected abstract Task ExcuteAsync(object parameter);
    }
}
