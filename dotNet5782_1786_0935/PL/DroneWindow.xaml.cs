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
            //checkVisibility(1);
            status.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            add.Visibility = Visibility.Hidden;
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
            matchUpParcel.Visibility = Visibility.Visible;    //////change these 3 lines!!!
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;

            if(modelText.Text!=null)
            {
                Bl.UpdateDroneName(Convert.ToInt32(idText.Text), modelText.Text);
            }
           // if()


        }
        public DroneWindow(BL.BLImp drone)//new drone
        {
            InitializeComponent();
            this.Bl = drone;
            status.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //checkVisibility(2);
            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            chargeDrone.Visibility = Visibility.Visible;
            releaseDrone.Visibility = Visibility.Hidden;
            matchUpParcel.Visibility = Visibility.Visible;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void checkVisibility(int i)
        //{
        //    if (i == 1) 
        //    {
        //        add.Visibility = Visibility.Hidden;
        //        update.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        add.Visibility = Visibility.Visible;
        //        update.Visibility = Visibility.Hidden;
        //    }
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Drone newDrone = new Drone();
            newDrone.DroneId = Convert.ToInt32(idText.Text);
            newDrone.battery = Convert.ToInt32(batteryText.Text); //???
            // newDrone.MaxWeight = (weight)(WeightCategories.SelectedItem);    
            // newDrone.droneStatus =
            if ((IBL.BO.WeightCategories)weight.SelectedItem != null)
                newDrone.MaxWeight = (IBL.BO.WeightCategories)weight.SelectedItem;
            newDrone.Model = modelText.Text;
            newDrone.droneStatus = (IBL.BO.DroneStatus)status.SelectedItem;  ///???
            //whats delivery??????????
            newDrone.location.Lattitude = Convert.ToDouble(lattitudeText.Text);
            newDrone.location.Longitude = Convert.ToDouble(longitudeText.Text);
            Location newLocation = new Location(Convert.ToDouble(lattitudeText.Text), Convert.ToDouble(longitudeText.Text));
            int stationId = Bl.FindStation(newLocation);
            try
            { 
                Bl.AddDrone(newDrone, stationId);
                MessageBox.Show("ERROR drone not added");
            }
            catch 
            { MessageBox.Show("added drone succesfully"); }


        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {

        }

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  status.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }

        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }
    }
}
