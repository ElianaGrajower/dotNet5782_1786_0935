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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BL.BLImp Bl;
        public DroneWindow(IBL.BO.DroneToList drone)//update drone
        {
            InitializeComponent();
            //ShowInfo();
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
           // stationIdCombo.ItemsSource = Bl.GetStationsList();/////////////////////
            update.Visibility = Visibility.Visible;
            if (drone.droneStatus==IBL.BO.DroneStatus.available)
            {
                chargeDrone.Visibility = Visibility.Visible;
                releaseDrone.Visibility = Visibility.Hidden;
            }
            if (drone.droneStatus == IBL.BO.DroneStatus.maintenance)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Visible;
            }
            if (drone.droneStatus == IBL.BO.DroneStatus.delivery)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Hidden;
            }
            matchUpParcel.Visibility = Visibility.Visible;    
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            LongitudeText.Visibility = Visibility.Visible;
            lattitudeText.Visibility = Visibility.Visible;
            stationIdText.Visibility = Visibility.Hidden;
            longitudeRead.Visibility = Visibility.Visible;
            lattitudeRead.Visibility = Visibility.Visible;
            stationRead.Visibility = Visibility.Hidden;
            weightText.Visibility = Visibility.Visible;
            weight.Visibility = Visibility.Hidden;
            idText.Text = drone.droneId.ToString();
            modelText.Text = drone.Model.ToString();
            lattitudeText.Text = drone.location.Lattitude.ToString();
            LongitudeText.Text = drone.location.Longitude.ToString();
            weightText.Text = drone.weight.ToString();

        }
        public DroneWindow(BL.BLImp drone)//new drone
        {
            InitializeComponent();
            this.Bl = drone;
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
    //        stationIdCombo.Items.Add(Bl.GetStationsList());
            update.Visibility = Visibility.Hidden;
            chargeDrone.Visibility = Visibility.Visible;
            releaseDrone.Visibility = Visibility.Hidden;
            matchUpParcel.Visibility = Visibility.Visible;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            LongitudeText.Visibility = Visibility.Hidden;
            lattitudeText.Visibility = Visibility.Hidden;
            stationIdText.Visibility = Visibility.Visible;
            longitudeRead.Visibility = Visibility.Hidden;
            lattitudeRead.Visibility = Visibility.Hidden;
            stationRead.Visibility = Visibility.Visible;
            weightText.Visibility = Visibility.Hidden;
            weight.Visibility = Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.UpdateDroneName(Convert.ToInt32(idText.Text), modelText.Text);
                MessageBox.Show("drone model updated succesfuly");
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Drone newDrone = new Drone();
            newDrone.DroneId = Convert.ToInt32(idText.Text);
            newDrone.MaxWeight = (WeightCategories)weight.SelectedItem;
            newDrone.Model = modelText.Text;
            int stationId = Convert.ToInt32(stationIdText.Text);
            try
            { 
                Bl.AddDrone(newDrone, stationId);
                MessageBox.Show("added drone succesfully");
                addAnotherDrone.Visibility = Visibility.Visible;
            }
            catch 
            { MessageBox.Show("ERROR drone not added"); }
        }
        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {
            releaseCanvas.Visibility = Visibility.Visible;
        }
        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            int time = Convert.ToInt32(releaseTime.Text);
            try
            {
                Bl.ReleaseDroneFromCharge(Convert.ToInt32(idText.Text), time);
                MessageBox.Show("drone released succesfully");
                releaseDrone.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
            releaseCanvas.Visibility = Visibility.Collapsed;

        }

        private void deliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.DeliveredParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone delivered succesfully");
                deliverParcel.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Hidden;
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void matchUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.MatchDroneWithPacrel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone matched up succesfully");
                matchUpParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void pickupParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.PickUpParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone pickedup succesfully");
                pickupParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void chargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bl.SendDroneToCharge(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone charging succesfully");
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
            addAnotherDrone.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new DroneWindow(Bl).Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
