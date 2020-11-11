using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;

namespace AS_Client.ViewModels
{
    public class MessageBarViewModel : ViewModelBase
    {
        public string Message => CreateMessage();
        public bool IsActive => _messageCount > 0;
        public ICommand ClearCommand => _clear;

        private string _lastMessage;
        private int _messageCount;
        private readonly ICommand _clear;

        public MessageBarViewModel()
        {
            _clear = new ActionCommand(Clear);
        }

        private string CreateMessage()
        {
            return $"{_lastMessage}{(_messageCount > 1 ? $" (+{_messageCount - 1} others)" : "")}";
        }

        public void AddMessage(string message)
        {
            _lastMessage = message;
            _messageCount++;
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(IsActive));
        }

        private void Clear()
        {
            _lastMessage = "";
            _messageCount = 0;
            OnPropertyChanged(nameof(Message));
            OnPropertyChanged(nameof(IsActive));
        }
    }
}