using System.Windows;

namespace AS_Client.ViewModels
{
    public class WorkAreaViewModelBase : ViewModelBase
    {
        public string Text { get; set; }
        public Visibility Visibility => IsActive ? Visibility.Visible : Visibility.Hidden;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(Visibility));
            }
        }

        private bool _isActive;

        public WorkAreaViewModelBase(string text)
        {
            Text = text;
        }
    }
}