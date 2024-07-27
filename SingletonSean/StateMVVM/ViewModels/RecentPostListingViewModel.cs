using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;

namespace StateMVVM.ViewModels
{
    public class RecentPostListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PostViewModel> _posts;
        public IEnumerable<PostViewModel> Posts => _posts;
        public bool HasPosts => _posts.Count > 0;
        public RecentPostListingViewModel()
        {
            _posts = new ObservableCollection<PostViewModel>();
            _posts.CollectionChanged += Posts_CollectionChanged;
        }

        private void Posts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Posts));
        }
    }
}
