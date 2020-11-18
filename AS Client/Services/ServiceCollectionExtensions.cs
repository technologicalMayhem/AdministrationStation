using System;
using System.Collections.Generic;
using System.Windows;
using AS_Client.Services;
using AS_Client.ViewModels;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddViewModelProvider(this IServiceCollection collection,
            Action<ViewModelProviderOptions> configure = null)
        {
            if (configure != null)
            {
                collection.Configure(configure);
            }

            collection.AddSingleton<IViewModelProvider, ViewModelProvider>();
        }
    }

    public class ViewModelProviderOptions
    {
        public Dictionary<Type, Type> ViewModelViewDictionary { get; } = new Dictionary<Type, Type>();

        public void Register<TViewModel, TView>()
            where TViewModel : ViewModelBase
            where TView : Window
        {
            ViewModelViewDictionary.Add(typeof(TViewModel), typeof(TView));
        }
    }
}