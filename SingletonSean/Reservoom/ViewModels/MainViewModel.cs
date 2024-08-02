﻿using Reservoom.Models;

namespace Reservoom.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }

        public MainViewModel(Hotel hotel)
        {
            //CurrentViewModel = new ReservationListingViewModel();
            CurrentViewModel = new MakeReservationViewModel(hotel);
        }
    }
}
