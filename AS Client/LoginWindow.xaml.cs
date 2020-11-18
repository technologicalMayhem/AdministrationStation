using AS_Client.Services;

namespace AS_Client
{
    public partial class LoginWindow
    {
        public LoginWindow(IPasswordProvider passwordProvider)
        {
            InitializeComponent();
            ((PasswordProvider) passwordProvider).PasswordBox = PassBox;
        }
    }
}