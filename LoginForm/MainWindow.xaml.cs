using System.Configuration;
using System.Diagnostics;
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
        bool suppressFocusText = false;


        public MainWindow()
        {
            InitializeComponent();
            manager = new UserManager();
            // manager.Register("Marlies", "test"); //simulatie registratie eerste gebruiker
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
                ShowInfo((roundsLeft == 1) ? $"Ongeldige gebruikersnaam of wachtwoord (nog {roundsLeft} poging te gaan)." :
                $"Ongeldige gebruikersnaam of wachtwoord (nog {roundsLeft} pogingen te gaan).", Brushes.Red);

                ClearScreen();                

                if (roundsLeft == 0)
                {
                    loginButton.IsEnabled = false;
                    resetButton.Visibility = Visibility.Visible;
                }
            }                     
        }        

        private void OnUsernameFocus(object sender, RoutedEventArgs e)
        {
            if (suppressFocusText)
                return;
            
            if (string.IsNullOrEmpty(usernameTextBox.Text))
                ShowInfo("Geef je gebruikersnaam", Brushes.Gray);            
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
            if (suppressFocusText)
                return;

            if (string.IsNullOrEmpty(usernameTextBox.Text))
                ShowInfo("Geef je wachtwoord", Brushes.Gray);                
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
            ClearScreen(true);
            loginButton.IsEnabled = true;
            resetButton.Visibility = Visibility.Hidden;            
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

            bool isValidRegistration = manager.Register(usernameTextBox.Text, passwordPasswordBox.Password);
            if (!isValidRegistration)
            {
                ShowInfo("Gebruikersnaam bestaat al.", Brushes.Red);                
            }
            else
            {
                ShowInfo("Registratie geslaagd!", Brushes.Green);
                ClearScreen();                
            }
        }

        private void ShowInfo(string text, Brush color)
        {
            statusTextBlock.Text = text;
            statusTextBlock.Foreground = color;
        }

        private void ClearScreen(bool ResetCounter = false)
        {
            suppressFocusText = true;
            
            usernameTextBox.Clear();
            passwordPasswordBox.Clear();
            if (ResetCounter)
            {
                roundsLeft = 3;
            }
            usernameTextBox.Focus();

            suppressFocusText = false;
        }
    }
}