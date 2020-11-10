using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AdministrationStation.Communication.Models.Client;

namespace AS_Client.ViewModels
{
    public class AgentWorkAreaViewModel : WorkAreaViewModelBase
    {
        public ObservableCollection<StatusResultModel> Agents { get; }

        private bool _isInitialized;

        public AgentWorkAreaViewModel() : base("text")
        {
            Agents = new ObservableCollection<StatusResultModel>();
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            new Thread(UpdateUi).Start();
            _isInitialized = true;
        }

        private void UpdateUi()
        {
            
            while (true)
            {
                var statuses = ServerService.Instance.GetAgentStatuses().Result;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Agents.Clear();
                    foreach (var statusResultModel in statuses)
                    {
                        Agents.Add(statusResultModel);
                    }
                }, DispatcherPriority.Background);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}