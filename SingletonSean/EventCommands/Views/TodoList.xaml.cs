using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EventCommands.Views
{
    /// <summary>
    /// Interaction logic for TodoList.xaml
    /// </summary>
    public partial class TodoList : UserControl
    {


        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.Register("LoadCommand", typeof(ICommand), typeof(TodoList), new PropertyMetadata(null));

        public ICommand SelectedTodoItemsChangedCommand
        {
            get { return (ICommand)GetValue(SelectedTodoItemsChangedCommandProperty); }
            set { SetValue(SelectedTodoItemsChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectedTodoItemsChangedCommandProperty =
            DependencyProperty.Register("SelectedTodoItemsChangedCommand", typeof(ICommand), typeof(TodoList), new PropertyMetadata(null));
        public TodoList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadCommand != null)
            {
                LoadCommand.Execute(null);
            }
        }

        private void lvTodoItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTodoItemsChangedCommand != null)
            {
                SelectedTodoItemsChangedCommand.Execute(lvTodoItems.SelectedItems);
            }
        }
    }
}
