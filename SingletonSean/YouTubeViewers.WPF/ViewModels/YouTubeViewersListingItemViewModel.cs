﻿using System.Windows.Input;
using YouTubeViewers.Domain.Models;
using YouTubeViewers.WPF.Commands;
using YouTubeViewers.WPF.Stores;
namespace YouTubeViewers.WPF.ViewModels
{
    public class YouTubeViewersListingItemViewModel : ViewModelBase
    {
        private YouTubeViewer _youTubeViewer;

        public YouTubeViewer YouTubeViewer
        {
            get { return _youTubeViewer; }
            set { _youTubeViewer = value; }
        }

        public string Username => _youTubeViewer.Username;
        private bool _isDeleting;
        public bool IsDeleting
        {
            get
            {
                return _isDeleting;
            }
            set
            {
                _isDeleting = value;
                OnPropertyChanged(nameof(IsDeleting));
            }
        }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public YouTubeViewersListingItemViewModel(YouTubeViewer youTubeViewer, ModalNavigationStore modalNavigationStore)
        {
            EditCommand = new OpenEditYouTubeViewerCommand(this, modalNavigationStore);
            _youTubeViewer = youTubeViewer;
            IsDeleting = false;
        }
    }
}