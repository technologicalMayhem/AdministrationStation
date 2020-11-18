using System;
using System.Collections.Generic;
using System.Windows;
using AS_Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AS_Client.Services
{
    public class ViewModelProvider : IViewModelProvider
    {
        //Key: ViewModel  Value: View
        private readonly Dictionary<Type, Type> _viewModelViewDictionary;
        private readonly IServiceProvider _serviceProvider;

        public ViewModelProvider(IServiceProvider serviceProvider, IOptions<ViewModelProviderOptions> options)
        {
            _viewModelViewDictionary = options.Value.ViewModelViewDictionary;
            _serviceProvider = serviceProvider;
        }

        public T PrepareViewModel<T>() where T : ViewModelBase
        {
            var type = _viewModelViewDictionary[typeof(T)];
            var view = ActivatorUtilities.CreateInstance(_serviceProvider, type) as Window;
            var viewModel = ActivatorUtilities.CreateInstance<T>(_serviceProvider);
            
            if(view == null) throw new Exception($"The view corresponding to ${typeof(T)} could not be created.");
            view.DataContext = viewModel;
            view.Show();
            return viewModel;
        }
    }
}