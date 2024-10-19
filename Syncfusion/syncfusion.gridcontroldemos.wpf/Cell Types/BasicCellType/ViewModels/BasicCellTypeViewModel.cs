﻿
using System.Windows.Media.Imaging;

namespace syncfusion.gridcontroldemos.wpf
{
    public class BasicCellTypeViewModel
    {
        public BasicCellTypeViewModel()
        {
            movieDetails = GetMovieDetails();
        }


        private List<MovieInfo> movieDetails;

        public List<MovieInfo> MovieDetails
        {
            get { return movieDetails; }
            set { movieDetails = value; }
        }

        private List<MovieInfo>? GetMovieDetails()
        {
            List<MovieInfo> model = new List<MovieInfo>();

            model.Add(new MovieInfo()
            {
                MovieName = "Avatar",
                Theatre = "Lodo",
                City = "New Jersy",
                IsTicketAvailable = true,
                Poster = new BitmapImage(new Uri("pack://application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Movies/Avatar.jpeg", UriKind.Absolute))
            });

            model.Add(new MovieInfo()
            {
                MovieName = "Ice Age 3",
                Theatre = "Modern",
                City = "New York",
                IsTicketAvailable = true,
                Poster = new BitmapImage(new Uri("pack://application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Movies/Iceage3.jpeg", UriKind.Absolute))
            });

            model.Add(new MovieInfo()
            {
                MovieName = "Toy Story 3",
                Theatre = "Greek",
                City = "New York",
                IsTicketAvailable = true,
                Poster = new BitmapImage(new Uri("pack://application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Movies/Toystory3.jpeg", UriKind.Absolute))
            });

            model.Add(new MovieInfo()
            {
                MovieName = "Shrek",
                Theatre = "Lodo",
                City = "New Jersy",
                IsTicketAvailable = true,
                Poster = new BitmapImage(new Uri("pack://application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Movies/Shrek.jpeg", UriKind.Absolute))
            });

            model.Add(new MovieInfo()
            {
                MovieName = "Kung Fu Panda 2",
                Theatre = "Modern",
                City = "New York",
                IsTicketAvailable = true,
                Poster = new BitmapImage(new Uri("pack://application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Movies/Kungfupanda2.jpeg", UriKind.Absolute))
            });


            return model;
        }
    }
}
