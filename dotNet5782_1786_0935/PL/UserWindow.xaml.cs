using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;


namespace PL
{
    /// <summary>
    /// This is the main window that opens when the project is started and here you can log into a users account
    /// </summary>
    public partial class UserWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        
        /// <summary>
        /// constructor
        /// </summary>
        public UserWindow() 
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// checks if the customer exists and checks the password to log into the custor or employees account.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e) 
        {
            try    
            {
                string user = userNameText.Text;
                string password = (string)passwordText.Password.ToString();
                Customer openCustomer = new Customer();
                int id = bl.searchCustomer(user);
                openCustomer = bl.getCustomer(id);
                if (bl.isEmployee(user, password))
                {
                    new MainWindow(openCustomer).Show();
                }
                else
                {
                    new AccountWindow(openCustomer).Show();
                }
                Close();
            }
            catch(DoesntExistException exp)
            {    
                MessageBox.Show(exp.Message);
            }
            catch(InvalidInputException exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        /// <summary>
        /// opens the log in option.
        /// </summary>
        private void logInButton_Click(object sender, RoutedEventArgs e) 
        {
            userPassword.Visibility = Visibility.Visible;
            white.Visibility = Visibility.Visible;
            blackBackground.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// opens the customer window to create a new account.
        /// </summary>
        private void signUpButton_Click(object sender, RoutedEventArgs e) 
        {
            new CustomerWindow(bl, true, false).Show();
        }
        /// <summary>
        /// closes the log in button
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e) 
        {
            userPassword.Visibility = Visibility.Collapsed;
            white.Visibility = Visibility.Collapsed;
            blackBackground.Visibility = Visibility.Collapsed;
            userNameText.Text = "";
            passwordText.Clear();
        }
    }
}
