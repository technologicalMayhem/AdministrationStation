using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private ObservableCollection<AgentResult> AgentData { get; set; }

        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            AgentData = new ObservableCollection<AgentResult>();
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
                AgentData.Add(new AgentResult(resultModel));
            }
        }
        
    }
    
    public class AgentResult : StatusResultModel
    {
        public AgentResult(StatusResultModel model)
        {
            Agent = model.Agent;
            Status = model.Status;
        }

        public ImageSource ImageSource
        {
            get
            {
                var biImg = new BitmapImage();
                var ms = new MemoryStream(Status.Screenshot);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                var imgSrc = (ImageSource) biImg;

                return imgSrc;
            }
        }
    }
}