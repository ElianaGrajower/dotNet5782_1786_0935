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
            matchUpParcel.Visibility = Visibility.Visible;    
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;



        }
        public DroneWindow(BL.BLImp drone)//new drone
        {
            InitializeComponent();
            this.Bl = drone;
            weight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
    //        stationIdCombo.Items.Add(Bl.GetStationsList());
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (modelText.Text != null)
            {
                Bl.UpdateDroneName(Convert.ToInt32(idText.Text), modelText.Text);
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
            }
            catch 
            { MessageBox.Show("ERROR drone not added"); }

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Enter drone charge time");
            int time = 78;
            try
            {
                Bl.ReleaseDroneFromCharge(Convert.ToInt32(idText.Text), time);
                MessageBox.Show("drone released succesfully");
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
