using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AS_Client.ViewModels
{
    public class CommandModel : ViewModelBase
    {
        public CommandModel(string displayName, ICommand command)
        {
            DisplayName = displayName;
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected CommandModel()
        {
            
        }

        public string DisplayName { get; protected set; }
        public ICommand Command { get; protected set; }
    }
}
