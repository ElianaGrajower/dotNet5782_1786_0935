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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Drone d;
        ParcelInTransit p = new();
        string cName;
        DroneListWindow droneList;
       
        BackgroundWorker worker;
        bool checkRun;
        //bool isClose = false;
        private void updateDrone() => worker.ReportProgress(1);
        private bool checkStop() => worker.CancellationPending;

        public DroneWindow(IBL b, Drone drone, string customerName, DroneListWindow dlw = null)//update drone
        {
            InitializeComponent();
            //ShowInfo();
            this.bl = b;
            textBlocks.DataContext = drone;
            d = drone;
            
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
            }
            if (drone.droneStatus == BO.DroneStatus.maintenance)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Hidden;
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


        public DroneWindow(IBL drone, string customerName)//new drone
        {
            InitializeComponent();
            this.bl = drone;
            d = new Drone();
            d.location = new Location();
            d.parcel = new ParcelInTransit();
            textBlocks.DataContext = d;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s => s.numberOfAvailableSlots > 0).Select(s=>s.stationId);
            //  stationIdCombo.Items.Add(Bl.getStationsList());
            updateButton.Visibility = Visibility.Hidden;
         //   add.Visibility = Visibility.Visible;
            chargeDrone.Visibility = Visibility.Hidden;
            releaseDrone.Visibility = Visibility.Hidden;
            matchUpParcel.Visibility = Visibility.Hidden;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            latitudeRead.Visibility = Visibility.Hidden;
        //    stationIdCombo.Visibility = Visibility.Visible;
            latitudeText.Visibility = Visibility.Hidden;
            longitudeRead.Visibility = Visibility.Hidden;
            longitudeText.Visibility = Visibility.Hidden;
         //   stationRead.Visibility = Visibility.Visible;

            //weightText.Visibility = Visibility.Hidden;

         //   weight.Visibility = Visibility.Visible;
            batteryRead.Visibility = Visibility.Hidden;
            batteryGrid.Visibility = Visibility.Hidden;
            statusRead.Visibility = Visibility.Hidden;
            statusText.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            parcelButton.Visibility = Visibility.Hidden;
            //   weightText.IsReadOnly = false;
            //   idText.IsReadOnly = false;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
          //  droneList.updateListView();
           // isClose = true;
        }

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
        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {
            //int time;
            //try
            //{
            //    if (Convert.ToInt32(releaseTime.Text) == null)
            //        throw new BO.InvalidInputException("Invalid input!\n");
            //}

            //catch (InvalidInputException exc)
            //{
            //    MessageBox.Show(exc.Message);
            //}

            //time = Convert.ToInt32(releaseTime.Text);
            try
            {
                int droneId = Convert.ToInt32(idText.Text);
                bl.releaseDroneFromCharge(droneId);
                textBlocks.DataContext = bl.getDrone(d.droneId);
                MessageBox.Show("drone released succesfully");
                releaseDrone.Visibility = Visibility.Hidden;    
                chargeDrone.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Visible;
                ///////////////////////up to here
                //////////binde for battery,status......
            }
            catch (BO.DoesntExistException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }

        }

        private void deliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveredParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone delivered succesfully");
                textBlocks.DataContext = bl.getDrone(d.droneId);
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

        private void matchUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.matchDroneWithPacrel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone matched up succesfully");
                textBlocks.DataContext = bl.getDrone(d.droneId);
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

        private void pickupParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.pickUpParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone pickedUp succesfully");
                textBlocks.DataContext = bl.getDrone(d.droneId);
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

        private void chargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToCharge(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone charging succesfully");
                textBlocks.DataContext = bl.getDrone(d.droneId);
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
                //MessageBox.Show("ERROR");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
            new DroneWindow(bl, cName).Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

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

          //  new ParcelWindow(bl, updateParcel, cName).ShowDialog();
            //ShowInfo(); have a way to update the info in the window
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }

        public void updateDroneView()
        {
            d = bl.getDrone(d.droneId);
            if (d.droneStatus == DroneStatus.delivery)
            {
                parcel.Visibility = Visibility.Visible;
                weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
                prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
                p = d.parcel;
                if (p.parcelStatus == false)
                    p.distance = bl.distance(d.location, p.pickupLocation);
                if (p.parcelStatus == true)
                    p.distance = bl.distance(p.pickupLocation, p.targetLocation);
                parcel.DataContext = p;
            }
            else
                parcel.Visibility = Visibility.Hidden;
            textBlocks.DataContext = d;
            droneList.updateListView();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            d = bl.getDrone(d.droneId);
            d = bl.getDrone(d.droneId);
            if (d.droneStatus == DroneStatus.delivery)
            {
                parcel.Visibility = Visibility.Visible;
                weightSelect.ItemsSource = Enum.GetValues(typeof(weightCategories));
                prioritySelect.ItemsSource = Enum.GetValues(typeof(Priorities));
                p = d.parcel;
                if (p.parcelStatus == false)
                    p.distance = bl.distance(d.location, p.pickupLocation);
                if (p.parcelStatus == true)
                    p.distance = bl.distance(p.pickupLocation, p.targetLocation);
                parcel.DataContext = p;
            }
            else
                parcel.Visibility = Visibility.Hidden;
            textBlocks.DataContext = d;
            droneList.updateListView();
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                // e.Result throw System.InvalidOperationException
                MessageBox.Show("The simulator is ended");
            }
            else if (e.Error != null)
            {
                // e.Result throw System.Reflection.TargetInvocationException
                MessageBox.Show("Error"); //Exception Message
            }


            //Auto = false;
            if (d.droneStatus == DroneStatus.delivery)
            {
                if (d.parcel.parcelStatus == true)
                    bl.deliveredParcel(d.droneId);
                else
                    bl.pickUpParcel(d.droneId);
                d = bl.getDrone(d.droneId);
                textBlocks.DataContext = d;
            }
            MessageBox.Show("The simulator is ended");
        }

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

        private void manualButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
                worker.CancelAsync();


            buttons.Visibility = Visibility.Visible;
            automticButton.Visibility = Visibility.Visible;
            manualButton.Visibility = Visibility.Hidden;  // i think...
            parcelButton.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            logOut.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Visible;
          
        }
    }
}



 