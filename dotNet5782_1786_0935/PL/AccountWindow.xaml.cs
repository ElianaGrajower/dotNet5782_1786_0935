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
        }

        private void viewAccount_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, c).ShowDialog();
        }

        private void parcels_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(bl).ShowDialog();
        }
    }
}
