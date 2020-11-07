using System;
using System.Collections.ObjectModel;

namespace AS_Client.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<CommandModel> _sidebar;
        private ObservableCollection<CommandModel> _toolbar;

        public ObservableCollection<CommandModel> Sidebar =>
            _sidebar ??= new ObservableCollection<CommandModel>
            {
                new CommandModel("Dashboard", new RelayCommand(o => { Console.WriteLine("Dashboard"); })),
                new CommandModel("Agents", new RelayCommand((o) => Console.WriteLine("Agents"))),
                new CommandModel("Modules", new RelayCommand((o) => Console.WriteLine("Modules")))
            };

        public ObservableCollection<CommandModel> Toolbar =>
            _toolbar ??= new ObservableCollection<CommandModel>
            {
                new CommandModel("File", new RelayCommand(o => Console.WriteLine("File"))),
                new CommandModel("Edit", new RelayCommand(o => Console.WriteLine("Edit"))),
                new CommandModel("View", new RelayCommand(o => Console.WriteLine("View")))
            };
    }
}