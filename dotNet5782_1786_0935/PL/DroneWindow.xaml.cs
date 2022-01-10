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
//using System.Linq;
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
        string cName;
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
        BackgroundWorker simulation;
        bool isRun;

        public DroneWindow(IBL b, Drone drone, string customerName)//update drone
        {
            InitializeComponent();
            //ShowInfo();
            this.bl = b;
            this.DataContext = drone;
            d = drone;
            simulation = new BackgroundWorker();
            isRun = true;
            simulation.DoWork += Simulation_DoWork;
            simulation.ProgressChanged += Simulation_ProgressChanged;
            simulation.WorkerReportsProgress = true;
            simulation.RunWorkerCompleted += Simulation_RunWorkerCompleted;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s=>s.numberOfAvailableSlots>0).Select(s=>s.stationId);
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
                if(drone.parcel.parcelStatus==ParcelStatus.matched)
                    deliverParcel.Visibility = Visibility.Hidden;
                if (drone.parcel.parcelStatus == ParcelStatus.pickedUp)
                    pickupParcel.Visibility = Visibility.Hidden;
            }
            stationIdCombo.Visibility = Visibility.Hidden;
            stationRead.Visibility = Visibility.Hidden;
            weight.Visibility = Visibility.Hidden;
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
            //to remove close box from window
            Loaded += ToolWindow_Loaded;
        }

        private void Simulation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Simulation_ProgressChanged(object sender, ProgressChangedEventArgs e) //changes what we see
        {
            if (d.droneStatus == DroneStatus.available) 
            {
                bl.matchDroneWithPacrel(d.droneId);
            }
            DataContext = bl.getDrone(d.droneId);
        }

        private void Simulation_DoWork(object sender, DoWorkEventArgs e) //when it runs, 
        {
            while(this.isRun)
            {
                this.simulation.ReportProgress(1);
                Thread.Sleep(1000); //one secound
                //if(bl.getParcelsList())

            }
        }

        public DroneWindow(IBL drone, string customerName)//new drone
        {
            InitializeComponent();
            this.bl = drone;
            d = new Drone();
            d.location = new Location();
            d.parcel = new ParcelInTransit();
            DataContext = d;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s => s.numberOfAvailableSlots > 0).Select(s=>s.stationId);
            //  stationIdCombo.Items.Add(Bl.getStationsList());
            update.Visibility = Visibility.Hidden;
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
            weightText.Visibility = Visibility.Hidden;
         //   weight.Visibility = Visibility.Visible;
            batteryRead.Visibility = Visibility.Hidden;
            batteryText.Visibility = Visibility.Hidden;
            statusRead.Visibility = Visibility.Hidden;
            statusText.Visibility = Visibility.Hidden;
            parcelIdRead.Visibility = Visibility.Hidden;
            parcelIdText.Visibility = Visibility.Hidden;
            parcelButton.Visibility = Visibility.Hidden;
            //   weightText.IsReadOnly = false;
            //   idText.IsReadOnly = false;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            //Drone newDrone = new Drone();
            ////try
            ////{
            ////    newDrone.droneId = Convert.ToInt32(idText.Text);
            ////    newDrone.Maxweight = (weightCategories)weight.SelectedItem;
            ////    newDrone.Model = modelText.Text;
            ////    int stationId = Convert.ToInt32(stationIdText.Text);
            ////}
            ////catch
            ////{ MessageBox.Show("input incurect"); }
            //try
            //{
            //    newDrone.droneId = Convert.ToInt32(idText.Text);
            //    newDrone.maxWeight = (weightCategories)weight.SelectedItem;
            //    newDrone.model = modelText.Text;
            //    int stationId = Convert.ToInt32(stationIdCombo.SelectedItem);
            //    bl.addDrone(newDrone, stationId);
            //    MessageBox.Show("added drone succesfully");
            //    addAnotherDrone.Visibility = Visibility.Visible;
            //}
            //catch(BO.AlreadyExistsException exc)
            //{ MessageBox.Show(exc.Message); }
            //catch (BO.InvalidInputException exc)
            //{ MessageBox.Show(exc.Message); }
            //catch (BO.UnableToCompleteRequest exc)
            //{ MessageBox.Show(exc.Message); }
            //catch(Exception exc)
            //{
            //    MessageBox.Show("invalid input");
            //}

            try
            {
                int stationId = Convert.ToInt32(stationIdCombo.SelectedItem);
                bl.addDrone(d,stationId);
                MessageBox.Show("added drone succesfully");
                d = new Drone();
                d.location = new Location();
                d.parcel = new ParcelInTransit();
                DataContext = d;
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
        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }

        private void deliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveredParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone delivered succesfully");
                deliverParcel.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Visible;
                pickupParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Visible;
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
                matchUpParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Visible;
                deliverParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Hidden;
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
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
            updateParcel = bl.getParcel(Convert.ToInt32(parcelIdText.Text));
            new ParcelWindow(bl, updateParcel, cName).ShowDialog();
            //ShowInfo(); have a way to update the info in the window
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }

        private void simulationButton_Click(object sender, RoutedEventArgs e)
        {
            this.simulation.RunWorkerAsync();
        }
    }
}
