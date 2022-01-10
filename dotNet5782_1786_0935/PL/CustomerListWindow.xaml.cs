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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        bool checkIsCustomer;
        string cName;
        //  Customer c=new Customer();
        // ObservableCollection<CustomerToList> myObservableCollection;
        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public CustomerListWindow(IBL b, bool _isCustomer, string customerName)
        {
            InitializeComponent();
            this.bl = b;
            this.checkIsCustomer = _isCustomer;
            //       c = customer;
            //myObservableCollection = new ObservableCollection<CustomerToList>(bl.getCustomersList());
            //DataContext = myObservableCollection;
            if (checkIsCustomer)
            {
                CustomersListView.ItemsSource = b.getUsersList();
                addEmployeeButton.Visibility = Visibility.Hidden;
            }
            else
            {
                CustomersListView.ItemsSource = b.getEmployeesList();
                addCustomerButton.Visibility = Visibility.Hidden;
            }
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            //  ShowInfo();
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowInfo()  /////add filter
        {
            IEnumerable<CustomerToList> d = new List<CustomerToList>();
            if (checkIsCustomer)
                CustomersListView.ItemsSource = bl.getCustomersList();
            else
                CustomersListView.ItemsSource = bl.getEmployeesList();
        }

        private void addNewButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, true, true, cName).ShowDialog();
            ShowInfo();
            //myObservableCollection = new ObservableCollection<CustomerToList>(bl.getCustomersList());
            //DataContext = myObservableCollection;
        }

        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)  ////why does it earase???
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
                //myObservableCollection = new ObservableCollection<CustomerToList>(bl.getCustomersList());
                //CustomersListView.DataContext = myObservableCollection;
                ShowInfo();
            }
            catch (Exception exc)
            {
             //   MessageBox.Show("System Malfunction please wait a moment and try again\n");
            }
            
        }

        private void CustomersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, false, true, cName).ShowDialog();
            ShowInfo();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }
    }
}
