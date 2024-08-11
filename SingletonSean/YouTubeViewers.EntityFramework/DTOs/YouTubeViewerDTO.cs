namespace YouTubeViewers.EntityFramework.DTOs
{
    public class YouTubeViewerDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsMember { get; set; }
    }
}
