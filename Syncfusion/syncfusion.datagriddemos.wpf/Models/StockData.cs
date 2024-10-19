﻿using Syncfusion.Windows.Shared;

namespace syncfusion.datagriddemos.wpf
{
    public class StockData : NotificationObject
    {
        #region Private Members

        private string symbol;
        private string account;
        private double lastTrade;
        private double change;
        private double previousClose;
        private double open;
        private long volume;

        #endregion

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {

                symbol = value;
                RaisePropertyChanged("Symbol");
            }
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public string Account
        {
            get
            {
                return account;
            }
            set
            {

                account = value;
                RaisePropertyChanged("Account");
            }
        }

        /// <summary>
        /// Gets or sets the last trade.
        /// </summary>
        /// <value>The last trade.</value>
        public double LastTrade
        {
            get
            {
                return lastTrade;
            }
            set
            {

                lastTrade = value;
                RaisePropertyChanged("LastTrade");
            }
        }

        /// <summary>
        /// Gets or sets the change.
        /// </summary>
        /// <value>The change.</value>
        public double Change
        {
            get
            {
                return change;
            }
            set
            {

                change = value;
                RaisePropertyChanged("Change");
            }
        }

        /// <summary>
        /// Gets or sets the previous close.
        /// </summary>
        /// <value>The previous close.</value>
        public double PreviousClose
        {
            get
            {
                return previousClose;
            }
            set
            {

                previousClose = value;
                RaisePropertyChanged("PreviousClose");
            }
        }

        /// <summary>
        /// Gets or sets the open.
        /// </summary>
        /// <value>The open.</value>
        public double Open
        {
            get
            {
                return open;
            }
            set
            {

                open = value;
                RaisePropertyChanged("Open");
            }
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public long Volume
        {
            get
            {
                return volume;
            }
            set
            {

                volume = value;
                RaisePropertyChanged("Volume");
            }
        }

        /// <summary>
        /// Initializes the on.
        /// </summary>
        /// <param name="other">The other.</param>
        public void InitializeOn(StockData other)
        {
            this.Symbol = other.Symbol;
            this.LastTrade = other.LastTrade;
            this.Change = other.Change;
            this.PreviousClose = other.PreviousClose;
            this.Open = other.Open;
            this.Volume = other.Volume;
        }
    }
}
