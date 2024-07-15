﻿using AsyncCommands.ViewModels;
using System.Windows;
namespace AsyncCommands
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new LoginViewModel()
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
