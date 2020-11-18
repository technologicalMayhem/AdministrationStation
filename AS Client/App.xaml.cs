using System;
using System.Windows;
using AS_Client.Services;
using AS_Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AS_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var collection = new ServiceCollection();
            ConfigureServices(collection);
            
            _serviceProvider = collection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var viewModelProvider = _serviceProvider.GetService<IViewModelProvider>();
            viewModelProvider.PrepareViewModel<LoginViewModel>();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPasswordProvider, PasswordProvider>();
            services.AddViewModelProvider(options =>
            {
                options.Register<LoginViewModel, LoginWindow>();
                options.Register<MainWindowViewModel, MainWindow>();
            });
        }
    }
}