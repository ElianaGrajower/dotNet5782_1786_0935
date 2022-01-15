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
    /// Shows the information of indevidual parcels, and actions can be done on the parcel.
    /// </summary>
    public partial class ParcelWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        Parcel p;
        string cName;

        /// <summary>
        /// constructor for a new parcel
        /// </summary>
        /// <param name="customerName">the users name</param>
        public ParcelWindow(IBL parcel, string customerName) 
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
            deleteButton.Visibility = Visibility.Hidden;
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
        /// <summary>
        /// constructor for an existing parcel
        /// </summary>
        /// <param name="parcel">the existing oarcel</param>
        /// <param name="customerName">the users name</param>
        public ParcelWindow(IBL b, Parcel parcel, string customerName)
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
                if (statusParcel != null) 
                if (!(statusParcel.parcelStatus == (ParcelStatus)2 || statusParcel.parcelStatus == (ParcelStatus)3))
                {
                    droneButton.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                droneButton.Visibility = Visibility.Hidden;
            }
            if (parcel.scheduled != null)   
            {
                deleteButton.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// closes a window
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)  
        {
            Close();
        }
        /// <summary>
        /// opens a window of a drone thats matched to the current parcel
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)  
        {
            Drone openDrone = new Drone();
            openDrone = Bl.getDrone(Convert.ToInt32(droneIdText.Text));
            new DroneWindow(Bl, openDrone, cName).ShowDialog();
        }
        /// <summary>
        /// opens the window of the sender of the parcel
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)  
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(senderSelect.SelectedItem));
            new CustomerWindow(Bl, openCustomer, cName).ShowDialog();
        }
        /// <summary>
        /// opens the window of the target of the parcel
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e) 
        {
            Customer openCustomer = new Customer();
            openCustomer = Bl.getCustomer(Convert.ToInt32(targetSelect.SelectedItem));
            new CustomerWindow(Bl, openCustomer, cName).ShowDialog();
        }
        /// <summary>
        /// adds a new parcel
        /// </summary>
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
        /// <summary>
        /// confirms a delete action
        /// </summary>
        private void deleteButton_Click(object sender, RoutedEventArgs e)  
        {
            checkDelete.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// deletes a parcel
        /// </summary>
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
        /// <summary>
        /// closes delete confirmation
        /// </summary>
        private void cancel_Click(object sender, RoutedEventArgs e)  
        {
            checkDelete.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// logs out of the account
        /// </summary>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  
        {
            new UserWindow().Show();
            Bl.releaseAllFromCharge();
            Close();
        }
    }
}
