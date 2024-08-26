﻿using NavigationMVVM.ViewModels;
using System.Windows;
namespace NavigationMVVM
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
                DataContext = new MainViewModel(new Stores.NavigationStore())
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
