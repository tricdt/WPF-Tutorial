﻿using YouTubeViewers.Domain.Commands;
using YouTubeViewers.Domain.Models;
using YouTubeViewers.Domain.Queries;

namespace YouTubeViewers.WPF.Stores
{
    public class YouTubeViewersStore
    {
        private readonly IGetAllYouTubeViewersQuery _getAllYouTubeViewersQuery;
        private readonly ICreateYouTubeViewerCommand _createYouTubeViewerCommand;
        private readonly IUpdateYouTubeViewerCommand _updateYouTubeViewerCommand;
        private readonly IDeleteYouTubeViewerCommand _deleteYouTubeViewerCommand;

        private readonly List<YouTubeViewer> _youTubeViewers;
        public IEnumerable<YouTubeViewer> YouTubeViewers => _youTubeViewers;
        public event Action YouTubeViewersLoaded;
        public event Action<YouTubeViewer> YouTubeViewerAdded;

        public YouTubeViewersStore(IGetAllYouTubeViewersQuery getAllYouTubeViewersQuery,
            ICreateYouTubeViewerCommand createYouTubeViewerCommand,
            IUpdateYouTubeViewerCommand updateYouTubeViewerCommand,
            IDeleteYouTubeViewerCommand deleteYouTubeViewerCommand)
        {
            _getAllYouTubeViewersQuery = getAllYouTubeViewersQuery;
            _createYouTubeViewerCommand = createYouTubeViewerCommand;
            _updateYouTubeViewerCommand = updateYouTubeViewerCommand;
            _deleteYouTubeViewerCommand = deleteYouTubeViewerCommand;
            _youTubeViewers = new List<YouTubeViewer>();
        }
        public async Task Load()
        {
            IEnumerable<YouTubeViewer> youTubeViewers = await _getAllYouTubeViewersQuery.Execute();

            _youTubeViewers.Clear();
            _youTubeViewers.AddRange(youTubeViewers);

            YouTubeViewersLoaded?.Invoke();
        }

        public async Task Add(YouTubeViewer youTubeViewer)
        {
            await _createYouTubeViewerCommand.Execute(youTubeViewer);
            _youTubeViewers.Add(youTubeViewer);
            YouTubeViewerAdded?.Invoke(youTubeViewer);
        }
    }
}
