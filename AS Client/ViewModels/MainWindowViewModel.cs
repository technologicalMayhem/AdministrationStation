using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;

namespace AS_Client.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<SidebarElementModel> _sidebar;
        private ObservableCollection<CommandModel> _toolbar;
        private string _selectedSidebarElement;
        private AgentWorkAreaViewModel _agents;
        private WorkAreaViewModelBase _dashboard;
        private WorkAreaViewModelBase _modules;

        private ICommand _test;

        public string SelectedSidebarElement
        {
            get => _selectedSidebarElement;
            set
            {
                _selectedSidebarElement = value;
                OnPropertyChanged(nameof(SelectedSidebarElement));
                OnPropertyChanged(nameof(Sidebar));
            }
        }

        public ObservableCollection<SidebarElementModel> Sidebar
        {
            get => _sidebar;
            set
            {
                _sidebar = value;
                OnPropertyChanged(nameof(Sidebar));
            }
        }

        public ObservableCollection<CommandModel> Toolbar
        {
            get => _toolbar;
            set
            {
                _toolbar = value;
                OnPropertyChanged(nameof(Toolbar));
            }
        }

        public WorkAreaViewModelBase Dashboard => _dashboard ??= new WorkAreaViewModelBase("Dashboard");
        public AgentWorkAreaViewModel Agents => _agents ??= new AgentWorkAreaViewModel();
        public WorkAreaViewModelBase Modules => _modules ??= new WorkAreaViewModelBase("Modules");

        public object Test => _test ??= new ActionCommand(() => Console.WriteLine("Test!"));

        public MainWindowViewModel()
        {
            Sidebar = new ObservableCollection<SidebarElementModel>
            {
                new SidebarElementModel("Dashboard", new RelayCommand(_ => SelectDashboard())),
                new SidebarElementModel("Agents", new RelayCommand(_ => SelectAgents())),
                new SidebarElementModel("Modules", new RelayCommand(_ => SelectModules()))
            };
            Toolbar = new ObservableCollection<CommandModel>
            {
                new CommandModel("File", new RelayCommand(_ => Console.WriteLine("File"))),
                new CommandModel("Edit", new RelayCommand(_ => Console.WriteLine("Edit"))),
                new CommandModel("View", new RelayCommand(_ => Console.WriteLine("View")))
            };

            SelectedSidebarElement = Sidebar.FirstOrDefault()?.Id;
        }

        private void SelectDashboard()
        {
            ClearMenu();
            Dashboard.IsActive = true;
        }
        
        private void SelectAgents()
        {
            _agents.Initialize();
            ClearMenu();
            Agents.IsActive = true;
        }
        
        private void SelectModules()
        {
            ClearMenu();
            Modules.IsActive = true;
        }

        private void ClearMenu()
        {
            Dashboard.IsActive = false;
            Agents.IsActive = false;
            Modules.IsActive = false;
            foreach (var model in Sidebar)
            {
                model.IsActive = false;
            }
        }
    }
}