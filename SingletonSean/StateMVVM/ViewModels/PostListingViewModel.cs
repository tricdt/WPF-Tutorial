using MVVMEssentials.ViewModels;
using StateMVVM.Commands;
using StateMVVM.Models;
using StateMVVM.Stores;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
namespace StateMVVM.ViewModels
{
    public class PostListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PostViewModel> _posts;
        private readonly PostStore _postStore;
        public IEnumerable<PostViewModel> Posts => _posts;

        public bool HasPosts => _posts.Count > 0;
        public ICommand LoadPostsCommand { get; }
        public PostListingViewModel(PostStore postStore, MessageStore messageStore)
        {
            _postStore = postStore;
            _posts = new ObservableCollection<PostViewModel>();
            _posts.CollectionChanged += Posts_CollectionChanged;
            _postStore.PostCreated += PostStore_PostCreated;
            _postStore.PostsLoaded += UpdatePosts;
            //LoadPostsCommand = new LoadPostsCommand(_postStore);
            LoadPostsCommand = new LoadPostsCommand(_postStore, messageStore);
            UpdatePosts();
        }

        private void UpdatePosts()
        {
            _posts.Clear();

            foreach (Post post in _postStore.Posts
                .OrderByDescending(p => p.DateCreated))
            {
                PostViewModel postViewModel = new PostViewModel(post);
                _posts.Add(postViewModel);
            }
        }

        private void PostStore_PostCreated(Models.Post post)
        {
            PostViewModel postViewModel = new PostViewModel(post);
            _posts.Insert(0, postViewModel);
        }

        private void Posts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasPosts));
        }
        public async Task LoadPosts()
        {
            try
            {
                await _postStore.LoadPosts();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static PostListingViewModel LoadViewModel(PostStore postStore, MessageStore messageStore)
        {
            PostListingViewModel viewModel = new PostListingViewModel(postStore, messageStore);

            //viewModel.LoadPosts().ContinueWith(t => onLoaded?.Invoke(t));
            viewModel.LoadPostsCommand.Execute(null);
            return viewModel;
        }
        public override void Dispose()
        {
            _postStore.PostCreated -= PostStore_PostCreated;
            _postStore.PostsLoaded -= UpdatePosts;
            base.Dispose();
        }
    }
}
