using System.Windows.Input;

namespace ClassCommands.Commands
{
    public class CallbackCommand : ICommand
    {
        private readonly Action _callback;
        private readonly Func<bool> _canExcute;

        public CallbackCommand(Action callback, Func<bool> canExcute)
        {
            _callback = callback;
            _canExcute = canExcute ?? (() => true);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            return _canExcute();
        }

        public void Execute(object? parameter)
        {
            _callback?.Invoke();
        }
    }
}
