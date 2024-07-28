using MVVMEssentials.Commands;
using StateMVVM.Stores;

namespace StateMVVM.Commands
{
    public class LoadPostsCommand : AsyncCommandBase
    {
        private readonly PostStore _postStore;
        private readonly MessageStore _messageStore;
        public LoadPostsCommand(PostStore postStore, MessageStore messageStore)
        {
            _postStore = postStore;
            _messageStore = messageStore;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _postStore.LoadPosts();
            }
            catch (Exception)
            {
                //MessageBox.Show("Failed to load posts.", "Error",
                //    MessageBoxButton.OK, MessageBoxImage.Error);
                _messageStore.SetCurrentMessage("Failed to load posts.", MessageType.Error);
            }
        }
    }
}
