using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AS_Client.Services;
using Microsoft.Xaml.Behaviors.Core;

namespace AS_Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _errorMessage;
        private LoginWindowState _state;
        private bool _isLoggedIn;

        private readonly IViewModelProvider _modelProvider;
        private readonly IPasswordProvider _passwordProvider;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public LoginWindowState State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(IsUiEnabled));
            }
        }
        
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsUiEnabled => _state != LoginWindowState.OperationPending;

        public ICommand SubmitCommand { get; }

        public LoginViewModel(IViewModelProvider modelProvider, IPasswordProvider passwordProvider)
        {
            _modelProvider = modelProvider;
            _passwordProvider = passwordProvider;
            SubmitCommand = new ActionCommand(async () => await Login());
        }

        private async Task Login()
        {
            ClientConfiguration.Username = Username;

            State = LoginWindowState.OperationPending;
            try
            {
                await ServerService.Instance.Login(_passwordProvider.GetPassword());
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                State = LoginWindowState.Failed;
                return;
            }

            _modelProvider.PrepareViewModel<MainWindowViewModel>();
            IsLoggedIn = true;
        }
    }

    public enum LoginWindowState
    {
        Default,
        OperationPending,
        Failed
    }
}