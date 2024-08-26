﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                services.AddSingleton<HomeViewModel>();
                services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
            });
            return hostBuilder;
        }
    }
}
