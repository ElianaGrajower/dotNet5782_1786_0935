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
        string cName;
      
        public ParcelWindow(IBL parcel, string customerName) //add
        {
            InitializeComponent();
            this.Bl = parcel;
            p = new Parcel();
            p.sender = new CustomerInParcel();
            p.target = new CustomerInParcel();
            p.drone = new DroneInParcel();
            DataContext = p;
            weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
            prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            targetSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            //weightText.Visibility = Visibility.Hidden;
            //priorityText.Visibility = Visibility.Hidden;
            //senderText.Visibility = Visibility.Hidden;
            //targetText.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;
     //       updateButton.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            droneIdText.Visibility = Visibility.Hidden;
            droneIdRead.Visibility = Visibility.Hidden;
            requestedRead.Visibility = Visibility.Hidden;
            requestedText.Visibility = Visibility.Hidden;
            scheduleText.Visibility = Visibility.Hidden;
            scheduledRead.Visibility = Visibility.Hidden;
            pickedupText.Visibility = Visibility.Hidden;
            pickUpRead.Visibility = Visibility.Hidden;
            deliveredRead.Visibility = Visibility.Hidden;
            deliveryText.Visibility = Visibility.Hidden;
            droneButton.Visibility = Visibility.Hidden;
            senderButton.Visibility = Visibility.Hidden;
            targetButton.Visibility = Visibility.Hidden;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
          






        }
        public ParcelWindow(IBL b, Parcel parcel, string customerName) //update
        {
            InitializeComponent();
            weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
            prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
            senderSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            targetSelect.ItemsSource = Bl.allCustomers().Select(c => c.customerId);
            this.Bl = b;
            this.DataContext = parcel;
            p = parcel;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            weightSelect.IsEnabled = false;
            prioritySelect.IsEnabled = false;
            senderSelect.IsEnabled = false;
            targetSelect.IsEnabled = false;
            addButton.Visibility = Visibility.Hidden;
            ourLOGO.Visibility = Visibility.Hidden;
           
            try
            {
                ParcelToList statusParcel = new ParcelToList();
                int getParcelId = parcel.parcelId;
                statusParcel = Bl.getParcelsList().Where(x => x.parcelId == getParcelId).FirstOrDefault();
                if(statusParcel!=null)
                if (!(statusParcel.parcelStatus == (ParcelStatus)2 || statusParcel.parcelStatus == (ParcelStatus)3))
                {
                    droneButton.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                droneButton.Visibility = Visibility.Hidden;
            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Drone openDrone = new Drone();
            openDrone = Bl.getDrone(Convert.ToInt32(droneIdText.Text));
            new DroneWindow(Bl, openDrone, cName).ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(senderSelect.SelectedItem));
            new CustomerWindow(Bl, openCustomer, cName).ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(targetSelect.SelectedItem));
            new CustomerWindow(Bl, openCustomer, cName).ShowDialog();
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
            catch(InvalidInputException exc )
            {
                MessageBox.Show(exc.Message);
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
            checkDelete.Visibility = Visibility.Visible;
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            ParcelToList deleteParcel = new ParcelToList();
            int getParcelId = Convert.ToInt32(parcelIdText.Text);
            deleteParcel = Bl.getParcelsList().Where(p => p.parcelId == getParcelId).FirstOrDefault();
            if (deleteParcel.parcelStatus == (ParcelStatus)1)
            {
                try
                {
                    Bl.deleteParcel(getParcelId);
                    MessageBox.Show("parcel deleted succesfully");
                    checkDelete.Visibility = Visibility.Collapsed;
                    Close();
                }
                catch
                {
                    MessageBox.Show("ERROR");
                }
            }
            else
            {
                MessageBox.Show("parcel can not be deleted at this stage");
                checkDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            checkDelete.Visibility = Visibility.Collapsed;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            Bl.releaseAllFromCharge();
            Close();
        }
    }
}
