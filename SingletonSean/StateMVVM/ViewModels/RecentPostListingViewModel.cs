using MVVMEssentials.ViewModels;
using StateMVVM.Stores;
using System.Collections.ObjectModel;
namespace StateMVVM.ViewModels
{
    public class RecentPostListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PostViewModel> _posts;
        private readonly PostStore _postStore;
        public IEnumerable<PostViewModel> Posts => _posts;
        public bool HasPosts => _posts.Count > 0;
        public RecentPostListingViewModel(PostStore postStore)
        {
            _postStore = postStore;
            _posts = new ObservableCollection<PostViewModel>();
            _posts.CollectionChanged += Posts_CollectionChanged;
            _postStore.PostCreated += PostStore_PostCreated;
            _postStore.PostsLoaded += UpdatePosts;
            UpdatePosts();
        }

        private void UpdatePosts()
        {
            _posts.Clear();

            foreach (Models.Post post in _postStore.Posts
                .OrderByDescending(p => p.DateCreated)
                .Take(5))
            {
                PostViewModel postViewModel = new PostViewModel(post);
                _posts.Add(postViewModel);
            }
        }

        private void PostStore_PostCreated(Models.Post post)
        {
            PostViewModel postViewModel = new PostViewModel(post);
            _posts.Insert(0, postViewModel);

            if (_posts.Count > 5)
            {
                _posts.RemoveAt(5);
            }
        }

        private void Posts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Posts));
        }

        public static RecentPostListingViewModel LoadViewModel(PostStore postStore, Action<Task> onLoaded = null)
        {
            RecentPostListingViewModel viewModel = new RecentPostListingViewModel(postStore);

            viewModel.LoadPosts().ContinueWith(t => onLoaded?.Invoke(t));

            return viewModel;
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
        public override void Dispose()
        {
            _postStore.PostCreated -= PostStore_PostCreated;
            _postStore.PostsLoaded -= UpdatePosts;
            base.Dispose();
        }
    }
}
