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
            new MainWindow().Show();
            Close();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            userPassword.Visibility = Visibility.Visible;
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl).Show();
        }
    }
}
