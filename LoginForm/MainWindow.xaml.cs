using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LoginForm.Models;
using LoginForm.Services;

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int roundsLeft = 3;
        UserManager manager;
        Dictionary<string, string> users = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            manager = new UserManager();
            manager.Register("Marlies", "test"); //simulatie registratie eerste gebruiker
            usernameTextBox.Focus();
            
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            Registration credentials = new Registration(usernameTextBox.Text, passwordPasswordBox.Password);
            if (manager.TryLogin(credentials))
            {
                statusTextBlock.Foreground = Brushes.Green;
                statusTextBlock.Text = "Login geslaagd!";
                
            }
            else
            {
                roundsLeft--;
                
                statusTextBlock.Foreground = Brushes.Red;
                statusTextBlock.Text = (roundsLeft == 1) ? $"Ongeldige gebruikersnaam of wachtwoord (nog {roundsLeft} poging te gaan)." :
                $"Ongeldige gebruikersnaam of wachtwoord (nog {roundsLeft} pogingen te gaan).";
                usernameTextBox.Clear();
                passwordPasswordBox.Clear();
                //usernameTextBox.Focus();

                if (roundsLeft == 0)
                {
                    loginButton.IsEnabled = false;
                    resetButton.Visibility = Visibility.Visible;
                }
            }                       
                        
        }        

        private void OnFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text))
            statusTextBlock.Foreground = Brushes.Gray;
            statusTextBlock.Text = "Geef je gebruikersnaam";
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(usernameTextBox.Text))
            {
                statusTextBlock.Text = string.Empty;
            }            
        }

        private void OnPasswordFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text))
                ShowInfo("Geef je wachtwoord", Brushes.Gray);
                //statusTextBlock.Foreground = Brushes.Gray;
            //statusTextBlock.Text = "Geef je wachtwoord";
        }        

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordPasswordBox.Password))
            {
                statusTextBlock.Text = string.Empty;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            roundsLeft = 3;
            usernameTextBox.Clear();
            passwordPasswordBox.Clear();
            loginButton.IsEnabled = true;
            resetButton.Visibility = Visibility.Hidden;
            usernameTextBox.Focus();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text) || string.IsNullOrEmpty(passwordPasswordBox.Password))
            {
                ShowInfo("Vul een gebruikersnaam én een wachtwoord in!", Brushes.Red);
            }

            if (users.ContainsKey(usernameTextBox.Text))
            {
                ShowInfo("Deze gebruikersnaam bestaat al", Brushes.Red);
            }

            users.Add(usernameTextBox.Text, passwordPasswordBox.Password);
        }

        private void ShowInfo(string text, Brush color)
        {
            statusTextBlock.Text = text;
            statusTextBlock.Foreground = color;
        }
    }
}