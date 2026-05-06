using System.Windows;
using System.Windows.Input;

namespace VyrobySalaryApp
{
    public partial class LogInForm : Window
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void AuthCheck()
        {
            string login = logTextBox.Text;
            string password = passwordBox.Password;

            bool result = Authorization.LogCheck(login, password);

            if (result)
            {
                MessageBox.Show("Авторизація успішна. Роль: " + Authorization.CurrentRole);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Невірний логін або пароль.");
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            AuthCheck();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AuthCheck();
            }
        }
    }
}