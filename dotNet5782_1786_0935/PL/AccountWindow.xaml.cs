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
    /// The main winow for a customer, that has all the actions that a customer can do.
    /// </summary>
    public partial class AccountWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Customer c = new Customer();

        /// <summary>
        /// costructor
        /// </summary>
        /// <param name="customer">the users name</param>
        public AccountWindow(Customer customer)  
        {
            InitializeComponent();
            c = customer;
            expanderHeader.Text = " " + c.name;
           
        }
        /// <summary>
        /// opens the customers window
        /// </summary>
        private void viewAccount_Click(object sender, RoutedEventArgs e)  
        {
            new CustomerWindow(bl, c, c.name).ShowDialog();
        }
        /// <summary>
        /// opens a list of the customers parcels
        /// </summary>
        private void parcels_Click(object sender, RoutedEventArgs e)  
        {
            new ParcelListWindow(bl, c, c.name).ShowDialog();
        }
        /// <summary>
        /// logs out of the account
        /// </summary>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }
        /// <summary>
        /// opens confirmations for the customer
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)  
        {
            confirmationCanvas.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// confirms the pickedUp parcels.
        /// </summary>
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
        /// <summary>
        /// confirms the delivered parcels
        /// </summary>
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
        /// <summary>
        /// closes the confirmations
        /// </summary>
        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)  
        {
            confirmationCanvas.Visibility = Visibility.Hidden;
        }
    }
}
