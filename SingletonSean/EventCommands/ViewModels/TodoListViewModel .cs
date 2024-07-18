using EventCommands.Commands;
using EventCommands.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EventCommands.ViewModels
{
    public class TodoListViewModel : ObservableObject
    {
        private ObservableCollection<TodoItem> _todoItems;

        public ObservableCollection<TodoItem> TodoItems
        {
            get { return _todoItems; }
            set
            {
                _todoItems = value;
                OnPropertyChanged(nameof(TodoItems));
            }
        }
        public ICommand LoadTodoItemsCommand { get; set; }

        public ICommand SelectedTodoItemsChangedCommand { get; set; }
        public TodoListViewModel()
        {
            LoadTodoItemsCommand = new LoadTodoItemsCommand(this);
            SelectedTodoItemsChangedCommand = new SelectedTodoItemsChangedCommand();
        }

    }
}
