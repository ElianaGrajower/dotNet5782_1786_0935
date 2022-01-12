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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Customer c = new Customer();
       

        public AccountWindow(Customer customer)
        {
            InitializeComponent();
            c = customer;
            expanderHeader.Text = " " + c.name;
           
        }

        private void viewAccount_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, c, c.name).ShowDialog();
        }

        private void parcels_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(bl, c, c.name).ShowDialog();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            confirmationCanvas.Visibility = Visibility.Visible;
        }

        private void pickUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var parcels = bl.confirmPickUp(c.customerId);
                string s = string.Join(Environment.NewLine, parcels);
                if (s == null)
                    throw new DoesntExistException();
                MessageBox.Show(s + "\nPress OK to approve the parcels above");
            }
            catch(Exception ex)
            {
                MessageBox.Show("No parcels awaiting confirmations");
            }
        }

        private void recivedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var parcels = bl.confirmDelivery(c.customerId);
                string s = string.Join(Environment.NewLine, parcels);
                if (s == null)
                    throw new DoesntExistException();
                MessageBox.Show(s + "\nPress OK to approve the parcels above");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No parcels awaiting confirmations");
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            confirmationCanvas.Visibility = Visibility.Hidden;
        }
    }
}
