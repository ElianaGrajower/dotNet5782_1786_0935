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
using BL;
using BO;
using BlApi;
using System.Threading;
using System.ComponentModel;




namespace PL
{
    /// <summary>
    /// Shows the information of indevidual drones, and actions can be done on the drone.
    /// </summary>
    public partial class DroneWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Drone d;
        ParcelInTransit p = new();
        string cName;
        DroneListWindow droneList;
       
        BackgroundWorker worker;
        int counter = 1;

        private void updateDrone() => worker.ReportProgress(1);
        private bool checkStop() => worker.CancellationPending;
        /// <summary>
        /// constructor for an existing drone
        /// </summary>
        /// <param name="drone">the existing drone</param>
        /// <param name="customerName">the users name</param>
        /// <param name="dlw">the droneListWindow it came from</param>
        public DroneWindow(IBL b, Drone drone, string customerName, DroneListWindow dlw = null)
        {
            InitializeComponent();
            this.bl = b;
            DataContext = drone;
            d = drone;
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            img3.Visibility = Visibility.Hidden;
            img4.Visibility = Visibility.Hidden;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s=>s.numberOfAvailableSlots>0).Select(s=>s.stationId);
            senderSelect.ItemsSource = bl.allCustomers().Select(c => c.customerId);
            targetSelect.ItemsSource = bl.allCustomers().Select(c => c.customerId);
            add.Visibility = Visibility.Hidden;
            ourLOGO.Visibility = Visibility.Hidden;
            if (drone.droneStatus == DroneStatus.available)
            {
                releaseDrone.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Hidden;
                parcelIdRead.Visibility = Visibility.Hidden;
                parcelIdText.Visibility = Visibility.Hidden;
            }
            if (drone.droneStatus == BO.DroneStatus.maintenance)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Hidden;
                parcelIdRead.Visibility = Visibility.Hidden;
                parcelIdText.Visibility = Visibility.Hidden;
            }
            if (drone.droneStatus == BO.DroneStatus.delivery)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Hidden;
                if(drone.parcel.parcelStatus==false)
                    deliverParcel.Visibility = Visibility.Hidden;
                if (drone.parcel.parcelStatus == true) 
                    pickupParcel.Visibility = Visibility.Hidden;
            }
            stationIdCombo.Visibility = Visibility.Hidden;
            stationRead.Visibility = Visibility.Hidden;
            weight.IsEnabled = false;
            if (drone.droneStatus == (DroneStatus)3)
                parcelIdText.Text = drone.parcel.ToString();
            else
            {
                parcelIdRead.Visibility = Visibility.Hidden;
                parcelIdText.Visibility = Visibility.Hidden;
                parcelButton.Visibility = Visibility.Hidden;
            }
            idText.IsEnabled = false;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            droneList = dlw;
        }
        /// <summary>
        /// constructor for a new drone
        /// </summary>
        /// <param name="customerName">the users name</param>
        public DroneWindow(IBL drone, string customerName)
        {
            InitializeComponent();
            this.bl = drone;
            d = new Drone();
            d.location = new Location();
            d.parcel = new ParcelInTransit();
            textBlocks.DataContext = d;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s => s.numberOfAvailableSlots > 0).Select(s => s.stationId);
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            img3.Visibility = Visibility.Hidden;
            img4.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;
            chargeDrone.Visibility = Visibility.Hidden;
            releaseDrone.Visibility = Visibility.Hidden;
            matchUpParcel.Visibility = Visibility.Hidden;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            latitudeRead.Visibility = Visibility.Hidden;
            latitudeText.Visibility = Visibility.Hidden;
            longitudeRead.Visibility = Visibility.Hidden;
            longitudeText.Visibility = Visibility.Hidden;
            batteryRead.Visibility = Visibility.Hidden;
            batteryGrid.Visibility = Visibility.Hidden;
            statusRead.Visibility = Visibility.Hidden;
            statusText.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            parcelButton.Visibility = Visibility.Hidden;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            automticButton.Visibility = Visibility.Hidden;
            manualButton.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// closes the window
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// updates the drones details
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            string name = modelText.Text.ToString();
            try
            {
                int id = Convert.ToInt32(idText.Text);
                bl.UpdateDronename(id, name);
                MessageBox.Show("drone model updated succesfuly");
            }
            catch(DoesntExistException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        /// <summary>
        /// adds a new drone
        /// </summary>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int stationId = Convert.ToInt32(stationIdCombo.SelectedItem);
                bl.addDrone(d,stationId);
                MessageBox.Show("added drone succesfully");
                d = new Drone();
                d.location = new Location();
                d.parcel = new ParcelInTransit();
                textBlocks.DataContext = d;
                Close();
            }
            catch(InvalidInputException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch
            {
                MessageBox.Show("ERROR can not add drone");
            }
        }
        /// <summary>
        /// releases a drone from charging
        /// </summary>
        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int droneId = Convert.ToInt32(idText.Text);
                bl.releaseDroneFromCharge(droneId);
                DataContext = bl.getDrone(d.droneId);
                MessageBox.Show("drone released succesfully");
                releaseDrone.Visibility = Visibility.Hidden;    
                chargeDrone.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Visible;
            }
            catch (BO.DoesntExistException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }

        }
        /// <summary>
        /// delivers a parcel
        /// </summary>
        private void deliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveredParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone delivered succesfully");
                DataContext = bl.getDrone(d.droneId);
                deliverParcel.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Visible;
                pickupParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Visible;
                parcelButton.Visibility = Visibility.Hidden;
                parcelIdRead.Visibility = Visibility.Hidden;
                parcelIdText.Visibility = Visibility.Hidden;
                statusText.Text = "available";
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }
        /// <summary>
        /// matches up a drone with a parcel
        /// </summary>
        private void matchUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.matchDroneWithPacrel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone matched up succesfully");
                DataContext = bl.getDrone(d.droneId);
                matchUpParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Visible;
                deliverParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Hidden;
                parcelButton.Visibility = Visibility.Visible;
                parcelIdRead.Visibility = Visibility.Visible;
                parcelIdText.Visibility = Visibility.Visible;
                statusText.Text = "delivery";
            }
            catch (BO.DoesntExistException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }
        /// <summary>
        /// the drone pickes up the parcel that its matched to
        /// </summary>
        private void pickupParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.pickUpParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone pickedUp succesfully");
                DataContext = bl.getDrone(d.droneId);
                pickupParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Hidden;
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }
        /// <summary>
        /// sends a drone to a charging station to charge
        /// </summary>
        private void chargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToCharge(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone charging succesfully");
                DataContext = bl.getDrone(d.droneId);
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Hidden;
                statusText.Text = "maintenance";
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
            new DroneWindow(bl, cName).Show();
        }
        /// <summary>
        /// opens a parcel that the drone is carrying
        /// </summary>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Parcel updateParcel = new Parcel();
            try
            {
                updateParcel = bl.getParcel(Convert.ToInt32(parcelIdText.Text));
                new ParcelWindow(bl, updateParcel, cName).ShowDialog();
            }
            catch(Exception exc)
            {
                MessageBox.Show("The drone is not connecetd to a parcel\n");
            }
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
        /// updates what the window shows while the simulator is running
        /// </summary>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            d = bl.getDrone(d.droneId);
           // d = bl.getDrone(d.droneId);
            if (d.droneStatus == DroneStatus.delivery)
            {
                parcel.Visibility = Visibility.Visible;
                weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
                prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
                p = d.parcel;
                if (p.parcelStatus == false)
                    p.distance = bl.distance(d.location, p.pickupLocation);  //the distance between the drone and the parcel
                if (p.parcelStatus == true)
                    p.distance = bl.distance(p.pickupLocation, p.targetLocation);  //the distance between the parcel and the target
                parcel.DataContext = p;
            }
            else
                parcel.Visibility = Visibility.Hidden;
            textBlocks.DataContext = d;
            droneList.updateListView();  //updates the droneListwindow
            if (checkStop())  //after the maual button is pressed
            {
                if (counter == 1)
                {
                    img4.Visibility = Visibility.Hidden;
                    img1.Visibility = Visibility.Visible;
                }
                if (counter == 2)
                {
                    img1.Visibility = Visibility.Hidden;
                    img2.Visibility = Visibility.Visible;
                }
                if (counter == 3)
                {
                    img2.Visibility = Visibility.Hidden;
                    img3.Visibility = Visibility.Visible;
                }
                if (counter == 4)
                {
                    img3.Visibility = Visibility.Hidden;
                    img4.Visibility = Visibility.Visible;
                    counter = 0;
                }
                counter++;
            }
        }
        /// <summary>
        /// ends the simulation once the manual button is pushed
        /// </summary>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // e.Result throw System.InvalidOperationException
                MessageBox.Show("The simulator has ended");
            }
            else if (e.Error != null)
            {
                // e.Result throw System.Reflection.TargetInvocationException
                MessageBox.Show("Error"); //Exception Message
            }
            if (d.droneStatus == DroneStatus.delivery)
            {
                if (d.parcel.parcelStatus == true)
                    bl.deliveredParcel(d.droneId);
                else
                    bl.pickUpParcel(d.droneId);
                d = bl.getDrone(d.droneId);
                DataContext = d;
            }
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            img3.Visibility = Visibility.Hidden;
            img4.Visibility = Visibility.Hidden;
            MessageBox.Show("The simulator has ended");
            buttons.Visibility = Visibility.Visible;
            automticButton.Visibility = Visibility.Visible;
            manualButton.Visibility = Visibility.Hidden;
            parcelButton.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            logOut.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Visible;
            releaseDrone.Visibility = Visibility.Hidden;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// starts the simulation
        /// </summary>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                bl.openSimulator(d.droneId, updateDrone, checkStop);
            }
            catch (Exception ex)  
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// starts the simulator
        /// </summary>
        private void simulationButton_Click(object sender, RoutedEventArgs e)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            buttons.Visibility = Visibility.Hidden;
            automticButton.Visibility = Visibility.Hidden;
            manualButton.Visibility = Visibility.Visible;
            logOut.Visibility = Visibility.Hidden;
            closeButton.Visibility = Visibility.Hidden;
            worker.RunWorkerAsync();
        }
        /// <summary>
        /// stops the simulator
        /// </summary>
        private void manualButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (worker.WorkerSupportsCancellation == true)
                    worker.CancelAsync();
            }
            catch(Exception exc)
            {

            }
        }
    }
}



 