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
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();

        public UserWindow()
        {
            InitializeComponent();
        }

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

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            userPassword.Visibility = Visibility.Visible;
            white.Visibility = Visibility.Visible;
            blackBackground.Visibility = Visibility.Visible;
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, true, false).Show();
        }

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
