﻿using Syncfusion.Windows.Shared;
using System.Windows.Media.Imaging;

namespace syncfusion.gridcontroldemos.wpf
{
    public class MovieInfo : NotificationObject
    {

        BitmapImage poster;
        /// <summary>
        /// Gets or sets the index of the image.
        /// </summary>
        /// <value>The index of the image.</value>
        public BitmapImage Poster
        {
            get
            {
                return poster;
            }
            set
            {
                poster = value;
                RaisePropertyChanged("Poster");
            }
        }

        private string movieName;
        /// <summary>
        /// Gets or sets the name of the movie.
        /// </summary>
        /// <value>The name of the movie.</value>
        public string MovieName
        {
            get
            {
                return movieName;
            }
            set
            {
                movieName = value;
                RaisePropertyChanged("MovieName");
            }
        }

        private string theatre;
        /// <summary>
        /// Gets or sets the theatre.
        /// </summary>
        /// <value>The theatre.</value>
        public string Theatre
        {
            get
            {
                return theatre;
            }
            set
            {
                theatre = value;
                RaisePropertyChanged("Theatre");
            }
        }

        private string city;
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
                RaisePropertyChanged("City");
            }
        }

        private bool isTicketAvailable;
        /// <summary>
        /// Gets or sets the isTicketAvailable.
        /// </summary>
        /// <value>The city.</value>
        public bool IsTicketAvailable
        {
            get
            {
                return isTicketAvailable;
            }
            set
            {
                isTicketAvailable = value;
                RaisePropertyChanged("IsTicketAvailable");
            }
        }
    }
}
