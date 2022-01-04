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
       // ObservableCollection<CustomerToList> myObservableCollection;

        public CustomerListWindow(IBL b, bool isCustomer)
        {
            InitializeComponent();
            this.bl = b;
            this.checkIsCustomer = isCustomer;
            //myObservableCollection = new ObservableCollection<CustomerToList>(bl.getCustomersList());
            //DataContext = myObservableCollection;
            if (isCustomer)
                CustomersListView.ItemsSource = b.allCustomers().Where(x => x.isCustomer == true);
            else
                CustomersListView.ItemsSource = b.allCustomers().Where(x => x.isCustomer == false);
          //  ShowInfo();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowInfo()  /////add filter
        {
            IEnumerable<CustomerToList> d = new List<CustomerToList>();
            if (checkIsCustomer)
                CustomersListView.ItemsSource = bl.allCustomers().Where(x => x.isCustomer == true);
            else
                CustomersListView.ItemsSource = bl.allCustomers().Where(x => x.isCustomer == false);
        }

        private void addNewButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, true).ShowDialog();
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
                new CustomerWindow(bl, realCustomer).ShowDialog();
                //myObservableCollection = new ObservableCollection<CustomerToList>(bl.getCustomersList());
                //CustomersListView.DataContext = myObservableCollection;
                ShowInfo();
            }
            catch (Exception exc)
            {
                MessageBox.Show("System Malfunction please wait a moment and try again\n");
            }
            
        }

        private void CustomersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, false).ShowDialog();
            ShowInfo();
        }

        private void groupButton_Click(object sender, RoutedEventArgs e)
        {
            CustomersListView.ItemsSource = bl.allCustomers();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CustomersListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("isCustomer");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            Close();
        }
    }
}
