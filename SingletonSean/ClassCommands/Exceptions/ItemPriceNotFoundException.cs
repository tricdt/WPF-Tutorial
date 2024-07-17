﻿namespace ClassCommands.Exceptions
{
    public class ItemPriceNotFoundException : Exception
    {
        public string ItemName { get; }
        public ItemPriceNotFoundException(string itemName)
        {
            ItemName = itemName;
        }
        public ItemPriceNotFoundException(string message, string itemName) : base(message)
        {
            ItemName = itemName;
        }

        public ItemPriceNotFoundException(string message, Exception innerException, string itemName) : base(message, innerException)
        {
            ItemName = itemName;
        }
    }
}
