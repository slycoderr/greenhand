using System.Windows.Input;

namespace GreenHand.Client.Windows.Views
{
    public partial class LoginView
    {
        public LoginView()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                UserNameTextBox.Focus();
            }

            else
            {
                Keyboard.Focus(PasswordTextBox);
            }
        }
    }
}