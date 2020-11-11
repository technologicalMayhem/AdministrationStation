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
        
        public MessageBarViewModel MessageBar { get; private set; }

        public WorkAreaViewModelBase Dashboard => _dashboard ??= new WorkAreaViewModelBase("Dashboard");
        public AgentWorkAreaViewModel Agents => _agents ??= new AgentWorkAreaViewModel();
        public WorkAreaViewModelBase Modules => _modules ??= new WorkAreaViewModelBase("Modules");

        public ICommand AddToMessageBar { get; private set; }

        public MainWindowViewModel()
        {
            Sidebar = new ObservableCollection<SidebarElementModel>
            {
                new SidebarElementModel("Dashboard", new ActionCommand(SelectDashboard)),
                new SidebarElementModel("Agents", new ActionCommand(SelectAgents)),
                new SidebarElementModel("Modules", new ActionCommand(SelectModules))
            };
            Toolbar = new ObservableCollection<CommandModel>
            {
                new CommandModel("File", new ActionCommand(() => Console.WriteLine("File"))),
                new CommandModel("Edit", new ActionCommand(() => Console.WriteLine("Edit"))),
                new CommandModel("View", new ActionCommand(() => Console.WriteLine("View")))
            };
            MessageBar = new MessageBarViewModel();

            SelectedSidebarElement = Sidebar.FirstOrDefault()?.Id;
            AddToMessageBar = new ActionCommand(AddRandomMessage);
        }

        private void AddRandomMessage()
        {
            var messages = new[]
            {
                "Server XYZ died. The funeral is next Wednesday.",
                "The ASS is currently busy... please call back later.",
                "A bug has occured. This is clearly the programmers fault.",
                "Some things crashed. Don't even bother making a issue. WontFix.",
                "Here is another silly and stupid message to pretend a error occured."
            };
            var rng = new Random();
            
            MessageBar.AddMessage(messages[rng.Next(messages.Length)]);
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