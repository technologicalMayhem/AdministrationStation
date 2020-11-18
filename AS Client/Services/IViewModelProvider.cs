using AS_Client.ViewModels;

namespace AS_Client.Services
{
    public interface IViewModelProvider
    {
        T PrepareViewModel<T>() where T : ViewModelBase;
    }
}