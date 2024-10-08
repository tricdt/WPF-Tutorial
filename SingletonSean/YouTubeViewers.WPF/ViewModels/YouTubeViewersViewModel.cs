﻿using System.Windows.Input;
using YouTubeViewers.WPF.Commands;
using YouTubeViewers.WPF.Stores;
namespace YouTubeViewers.WPF.ViewModels
{
    public class YouTubeViewersViewModel : ViewModelBase
    {
        public YouTubeViewersListingViewModel YouTubeViewersListingViewModel { get; }
        public YouTubeViewersDetailsViewModel YouTubeViewersDetailsViewModel { get; }
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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
        public ICommand AddYouTubeViewersCommand { get; }
        public ICommand LoadYouTubeViewersCommand { get; }
        public YouTubeViewersViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore,
            ModalNavigationStore modalNavigationStore,
            YouTubeViewersStore youTubeViewersStore)
        {
            YouTubeViewersListingViewModel = new YouTubeViewersListingViewModel(selectedYouTubeViewerStore, modalNavigationStore, youTubeViewersStore);
            YouTubeViewersDetailsViewModel = new YouTubeViewersDetailsViewModel(selectedYouTubeViewerStore, youTubeViewersStore);
            AddYouTubeViewersCommand = new OpenAddYouTubeViewerCommand(modalNavigationStore, youTubeViewersStore);
            LoadYouTubeViewersCommand = new LoadYouTubeViewersCommand(this, youTubeViewersStore);
        }
        public static YouTubeViewersViewModel LoadViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore,
            ModalNavigationStore modalNavigationStore,
            YouTubeViewersStore youTubeViewersStore)
        {
            YouTubeViewersViewModel viewModel = new YouTubeViewersViewModel(selectedYouTubeViewerStore, modalNavigationStore, youTubeViewersStore);

            viewModel.LoadYouTubeViewersCommand.Execute(null);

            return viewModel;
        }
    }
}
