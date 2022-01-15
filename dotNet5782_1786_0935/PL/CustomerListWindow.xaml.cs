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
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// This is the customers list, where the list of customers are and it leads to indavidual customers. 
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        bool checkIsCustomer;
        string cName;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="_isCustomer">checks if the user is a customer or employee</param>
        /// <param name="customerName">the users name</param>
        public CustomerListWindow(IBL b, bool _isCustomer, string customerName) 
        {
            InitializeComponent();
            this.bl = b;
            this.checkIsCustomer = _isCustomer;
            if (checkIsCustomer) //the list of costomers
            {
                CustomersListView.ItemsSource = b.getUsersList();
                addEmployeeButton.Visibility = Visibility.Hidden;
            }
            else //the list of employees
            {
                CustomersListView.ItemsSource = b.getEmployeesList();
                addCustomerButton.Visibility = Visibility.Hidden;
            }
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// closes the window
        /// </summary>
        private void closeButton_Click(object sender, RoutedEventArgs e)  
        {
            Close();
        }
        /// <summary>
        /// updates the list to show the new changes
        /// </summary>
        private void ShowInfo()  
        {
            IEnumerable<CustomerToList> d = new List<CustomerToList>();
            if (checkIsCustomer)
                CustomersListView.ItemsSource = bl.getUsersList();
            else
                CustomersListView.ItemsSource = bl.getEmployeesList();
        }
        /// <summary>
        /// opens a window to add a new customer
        /// </summary>
        private void addNewButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, true, true, cName).ShowDialog();
            ShowInfo();
        }
        /// <summary>
        /// opens a window that shows an existing user
        /// </summary>
        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)  
        {
            CustomerToList updateCustomer = new CustomerToList();
            updateCustomer = (CustomerToList)CustomersListView.SelectedItem;
            Customer realCustomer = new Customer();
            try
            {
                if (updateCustomer == null)
                    throw new Exception("clicked wrong area");
                realCustomer = bl.getCustomer(updateCustomer.customerId);
                new CustomerWindow(bl, realCustomer, cName).ShowDialog();
                ShowInfo();
            }
            catch (Exception exc)
            {
                
            }
            
        }
        /// <summary>
        /// opens a window to add a new employee
        /// </summary>
        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, false, true, cName).ShowDialog();
            ShowInfo();
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
