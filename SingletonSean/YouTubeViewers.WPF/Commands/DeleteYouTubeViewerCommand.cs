﻿using YouTubeViewers.Domain.Models;
using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;

namespace YouTubeViewers.WPF.Commands
{
    public class DeleteYouTubeViewerCommand : AsyncCommandBase
    {
        private readonly YouTubeViewersListingItemViewModel _youTubeViewersListingItemViewModel;
        private readonly YouTubeViewersStore _youTubeViewersStore;

        public DeleteYouTubeViewerCommand(YouTubeViewersListingItemViewModel youTubeViewersListingItemViewModel, YouTubeViewersStore youTubeViewersStore)
        {
            _youTubeViewersListingItemViewModel = youTubeViewersListingItemViewModel;
            _youTubeViewersStore = youTubeViewersStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _youTubeViewersListingItemViewModel.IsDeleting = true;
            YouTubeViewer youTubeViewer = _youTubeViewersListingItemViewModel.YouTubeViewer;
            try
            {
                await Task.Delay(5000);
                await _youTubeViewersStore.Delete(youTubeViewer.Id);
            }
            catch (Exception)
            {
            }
            finally
            {
                _youTubeViewersListingItemViewModel.IsDeleting = false;
            }
        }
    }
}