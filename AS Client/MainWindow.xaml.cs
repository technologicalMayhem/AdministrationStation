using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AdministrationStation.Communication.Models.Client;
using AdministrationStation.Communication.Models.Shared;

namespace AS_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<StatusResultModel> AgentData { get; set; }

        private readonly DispatcherTimer _timer;
        
        public MainWindow()
        {
            InitializeComponent();
            
            AgentData = new ObservableCollection<StatusResultModel>();
            ListBox.ItemsSource = AgentData;
            
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += UpdateAgentData;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5);
            dispatcherTimer.Start();
            UpdateAgentData();
        }

        private void UpdateAgentData(object? sender, EventArgs e)
        {
            UpdateAgentData();
        }

        private void UpdateAgentData()
        {
            var result = ServerService.Instance.GetAgentStatuses().Result;
            AgentData.Clear();
            
            foreach (var resultModel in result)
            {
                AgentData.Add(resultModel);
            }
        }
    }
}
