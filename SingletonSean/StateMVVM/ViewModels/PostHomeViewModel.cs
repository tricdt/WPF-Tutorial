using MVVMEssentials.ViewModels;
namespace StateMVVM.ViewModels
{
    public class PostHomeViewModel : ViewModelBase
    {
        public CreatePostViewModel CreatePostViewModel { get; }
        public RecentPostListingViewModel RecentPostListingViewModel { get; }

        public PostHomeViewModel(CreatePostViewModel createPostViewModel, RecentPostListingViewModel recentPostListingViewModel)
        {
            CreatePostViewModel = createPostViewModel;
            RecentPostListingViewModel = recentPostListingViewModel;
        }
    }
}
