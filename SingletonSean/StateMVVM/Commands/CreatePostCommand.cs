using MVVMEssentials.Commands;
using StateMVVM.Models;
using StateMVVM.Stores;
using StateMVVM.ViewModels;

namespace StateMVVM.Commands
{
    public class CreatePostCommand : CommandBase
    {
        private readonly CreatePostViewModel _viewModel;
        private readonly PostStore _postStore;
        private readonly MessageStore _messageStore;
        public CreatePostCommand(CreatePostViewModel viewModel, PostStore postStore, MessageStore messageStore)
        {
            _viewModel = viewModel;
            _postStore = postStore;
            _messageStore = messageStore;
        }

        public override void Execute(object parameter)
        {
            Post post = new Post()
            {
                Title = _viewModel.Title,
                Content = _viewModel.Content,
                DateCreated = DateTime.Now
            };
            _postStore.CreatePost(post);

            //MessageBox.Show("Successfully created post.", "Success",
            //    MessageBoxButton.OK, MessageBoxImage.Information);
            _messageStore.SetCurrentMessage("Successfully created the post.", MessageType.Status);
            _viewModel.Title = string.Empty;
            _viewModel.Content = string.Empty;
        }
    }
}
