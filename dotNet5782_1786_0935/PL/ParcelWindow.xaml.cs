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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        Parcel p;

        public ParcelWindow(IBL parcel) //add
        {
            InitializeComponent();
            this.Bl = parcel;
            p = new Parcel();
            p.sender = new CustomerInParcel();
            p.target = new CustomerInParcel();
            DataContext = p;
            weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
            prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            targetSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            weightText.Visibility = Visibility.Hidden;
            priorityText.Visibility = Visibility.Hidden;
            senderText.Visibility = Visibility.Hidden;
            targetText.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;

            

        }
        public ParcelWindow(IBL b, Parcel parcel) //update
        {
            InitializeComponent();
            this.Bl = b;
            this.DataContext = parcel;
            p = parcel;
            weightSelect.Visibility = Visibility.Hidden;
            prioritySelect.Visibility = Visibility.Hidden;
            senderSelect.Visibility = Visibility.Hidden;
            targetSelect.Visibility = Visibility.Hidden;
            addButton.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Drone openDrone = new Drone();
            openDrone = Bl.getDrone(Convert.ToInt32(droneIdText.Text));
            new DroneWindow(Bl, openDrone).ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(senderText.Text));
            new CustomerWindow(Bl, openCustomer).ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(targetText.Text));
            new CustomerWindow(Bl, openCustomer).ShowDialog();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.addParcel(p);
                MessageBox.Show("parcel added succesfully");
                p = new Parcel();
                p.sender = new CustomerInParcel();
                p.target = new CustomerInParcel();
                DataContext = p;
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR can not add parcel");
            }
        }

        private void prioritySelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //ParcelToList deleteParcel = new ParcelToList();
            //deleteParcel = (ParcelToList)Bl.allParcels(x => x.parcelId == Convert.ToInt32()).Select(x => x);  ///read the text if this doesnt work
            //if (deleteParcel.parcelStatus == (ParcelStatus)1)
            //{
            //    try
            //    {
            //        //int customerId = Convert.ToInt32(idText.Text);
            //        Bl.deleteCustomer(p.parcelId);
            //        MessageBox.Show("parcel deleted succesfully");
            //        //  checkDelete.Visibility = Visibility.Collapsed;
            //        Close();
            //    }
            //    catch
            //    {
            //        MessageBox.Show("ERROR");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("parcel can not be deleted at this stage");
            //}

        }
    }
}
