using StateMVVM.Commands;
using System.Windows.Input;
namespace StateMVVM.ViewModels.Posts
{
    public class CreatePostViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public ICommand CreatePostCommand { get; }

        public CreatePostViewModel()
        {
            CreatePostCommand = new CreatePostCommand(this);
        }
    }
}
