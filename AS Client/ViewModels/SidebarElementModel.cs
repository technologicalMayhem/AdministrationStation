using System;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;

namespace AS_Client.ViewModels
{
    public class SidebarElementModel : CommandModel
    {
        public string Id { get; private set; }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        private bool _isActive;

        public SidebarElementModel(string displayName, ICommand command)
        {
            Id = Guid.NewGuid().ToString();
            _isActive = false;
            Command = new ActionCommand(() =>
            {
                command.Execute(new object());
                IsActive = true;
            });
            DisplayName = displayName;
        }
    }
}