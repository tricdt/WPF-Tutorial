﻿using MVVMEssentials.ViewModels;

namespace CustomObservableCollections.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        public int Id { get; }
        public string Item { get; }
        public DateTime DateCreated { get; }

        public OrderViewModel(int id, string item, DateTime dateCreated)
        {
            Id = id;
            Item = item;
            DateCreated = dateCreated;
        }
    }
}
