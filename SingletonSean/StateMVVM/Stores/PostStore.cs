using StateMVVM.Models;
using StateMVVM.Services;

namespace StateMVVM.Stores
{
    public class PostStore
    {
        private readonly PostService _postService;
        private readonly List<Post> _posts;
        private Lazy<Task> _loadPostsLazy;
        public IEnumerable<Post> Posts => _posts;
        public event Action PostsLoaded;
        public event Action<Post> PostCreated;
        public PostStore(PostService postService)
        {
            _postService = postService;
            _posts = new List<Post>();
            _loadPostsLazy = CreateLoadPostsLazy();
        }
        private Lazy<Task> CreateLoadPostsLazy()
        {
            return new Lazy<Task>(() => InitializePosts());
        }

        private async Task InitializePosts()
        {
            IEnumerable<Post> posts = await _postService.GetAllPosts();

            _posts.Clear();
            _posts.AddRange(posts);

            PostsLoaded?.Invoke();
        }

        public async Task LoadPosts()
        {
            await _loadPostsLazy.Value;
        }

        public async Task RefreshPosts()
        {
            _loadPostsLazy = CreateLoadPostsLazy();
            await LoadPosts();
        }

        public void CreatePost(Post post)
        {
            _posts.Add(post);
            PostCreated?.Invoke(post);
        }
    }


}
