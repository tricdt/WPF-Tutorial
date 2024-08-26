﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavigationMVVM.Services;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();

                services.AddSingleton<HomeViewModel>(s => CreateHomeViewModel(s));
                services.AddSingleton<LoginViewModel>();
                services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
                services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
            });
            return hostBuilder;
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider s)
        {
            return new HomeViewModel(s.GetRequiredService<NavigationService<LoginViewModel>>());
        }
    }
}
