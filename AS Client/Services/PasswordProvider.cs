using System;
using System.Windows.Controls;

namespace AS_Client.Services
{
    public class PasswordProvider : IPasswordProvider
    {
        public PasswordBox PasswordBox { get; set; }

        public string GetPassword()
        {
            return PasswordBox != null ? PasswordBox.Password : throw new InvalidOperationException("The password has already been discarded.");
        }
    }
}