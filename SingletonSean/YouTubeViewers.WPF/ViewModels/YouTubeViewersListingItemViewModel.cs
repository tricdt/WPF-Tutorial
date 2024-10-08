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
        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public YouTubeViewersListingItemViewModel(YouTubeViewer youTubeViewer, ModalNavigationStore modalNavigationStore, YouTubeViewersStore youTubeViewersStore)
        {
            _youTubeViewer = youTubeViewer;
            EditCommand = new OpenEditYouTubeViewerCommand(this, modalNavigationStore, youTubeViewersStore);
            DeleteCommand = new DeleteYouTubeViewerCommand(this, youTubeViewersStore);
        }
        public void Update(YouTubeViewer youTubeViewer)
        {
            YouTubeViewer = youTubeViewer;

            OnPropertyChanged(nameof(Username));
        }
    }
}
