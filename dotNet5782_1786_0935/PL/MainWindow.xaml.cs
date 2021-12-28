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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();  ///supposed to be IBL
        public MainWindow()
        {
            InitializeComponent();
           
        }
        public void IBL()
        {
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
        }

        private void stationList_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(bl).Show();
        }

        private void customerList_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(bl).Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(bl).Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new UserWindow().Show();
            Close();
        }
    }
}
