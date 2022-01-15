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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// The main winow for an employee, that has all the actions that an employee can do.
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();  
        Customer c = new Customer();
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customer">the users name</param>
        public MainWindow(Customer customer)  
        {
            InitializeComponent();
            c = customer;
            expanderHeader.Text = " " + c.name;
        }

        public void IBL()
        {
        }
        /// <summary>
        /// opens the drone list window
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)  
        {
            new DroneListWindow(bl, c.name).Show();
        }
        /// <summary>
        /// opens the station list window
        /// </summary>
        private void stationList_Click(object sender, RoutedEventArgs e)  
        {
            new StationListWindow(bl, c.name).Show();
        }
        /// <summary>
        /// opens the customer list window
        /// </summary>
        private void customerList_Click(object sender, RoutedEventArgs e)  
        {
            new CustomerListWindow(bl, true, c.name).Show();
        }
        /// <summary>
        /// opens the parcel list window
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)  
        {
            new ParcelListWindow(bl, c.name).Show();
        }
        /// <summary>
        /// opens the employee list window
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e)  
        {
            new CustomerListWindow(bl, false, c.name).Show();
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
    }
}
