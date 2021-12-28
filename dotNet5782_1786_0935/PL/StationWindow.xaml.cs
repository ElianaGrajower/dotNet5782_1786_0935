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
using BlApi;
using BO;
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        ObservableCollection<DroneInCharging> droneAtStationObservableCollection;
        Station s;

        public StationWindow(IBL Station) //new
        {
            InitializeComponent();
            this.Bl = Station;
            s = new Station();
            s.location = new Location();
            DataContext = s;

            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            delete.Visibility = Visibility.Hidden;
            inUseRead.Visibility = Visibility.Hidden;
            inUseText.Visibility = Visibility.Hidden;
            listOfDronesAtStation.Visibility = Visibility.Hidden;
            dronesAtStationRead.Visibility = Visibility.Hidden;
            locationRead.Visibility = Visibility.Hidden;
            locationText.Visibility = Visibility.Hidden;
        }

        public StationWindow(IBL b, BO.Station station) //update
        {
            InitializeComponent();
            this.Bl = b;
            //s = new Station();
            //s.location = new Location();
            //DataContext = s;
            s = station;

            add.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Visible;
            delete.Visibility = Visibility.Visible;
            inUseRead.Visibility = Visibility.Visible;
            inUseText.Visibility = Visibility.Visible;
            latitudeText.Visibility = Visibility.Hidden;
            latitudeRead.Visibility = Visibility.Hidden;
            longitudeText.Visibility = Visibility.Hidden;
            longitudeRead.Visibility = Visibility.Hidden;
            ////switch to data binding?????????
            //   stationIdText.Text = station.stationId.ToString();
            //   nameText.Text = station.name.ToString();
            //   slotsText.Text = station.chargeSlots.ToString();
            //   inUseText.Text = station.numberOfSlotsInUse.ToString();
            //   allChargeText.Text = (station.chargeSlots + station.numberOfSlotsInUse).ToString();
            //   locationText.Text = station.location.ToString();  // it will work when we make it a station
            ////   Station realStation = new Station();
            ////   realStation = Bl.getStation(station.stationId);
            //   listOfDronesAtStation.ItemsSource = station.dronesAtStation;
            ////////
            ///
            this.DataContext = station;
            droneAtStationObservableCollection = new ObservableCollection<DroneInCharging>(station.dronesAtStation);
            listOfDronesAtStation.DataContext = droneAtStationObservableCollection;

            stationIdText.IsReadOnly = true;
            latitudeText.IsReadOnly = true;
            longitudeText.IsReadOnly = true;
            inUseText.IsReadOnly = true;
            locationText.IsReadOnly = true;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            //Station newStation = new Station();
            //try
            //{
            //    ///not allowed???????????FIX????????????
            //    newStation.stationId = Convert.ToInt32(stationIdText.Text);
            //    newStation.name = nameText.Text;
            //    Location newLocation = new Location(Convert.ToDouble(latitudeText.Text), Convert.ToDouble(longitudeText.Text));
            //    newStation.location = newLocation;
            //    newStation.chargeSlots = Convert.ToInt32(allChargeText.Text);
            //    Bl.addStation(newStation);
            //    MessageBox.Show("station added succesfully");
            //    addAnotherStation.Visibility = Visibility.Visible;
            //}
            //catch
            //{
            //    MessageBox.Show("ERROR invalid input");
            //}
            try
            {
                Bl.addStation(s);
                MessageBox.Show("station added succesfully");
                s = new Station();
                s.location = new Location();
                DataContext = s;
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR can not add station");
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                int stationId = Convert.ToInt32(stationIdText.Text);
                int chargeSlots = Convert.ToInt32(allChargeText.Text);
                string name = nameText.Text;
                Bl.updateStation(stationId, chargeSlots, name);
                MessageBox.Show("station updated succesfully");
                DataContext = s;
            }
            catch
            {
                MessageBox.Show("ERROR invalid input");
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            checkDelete.Visibility = Visibility.Visible;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            checkDelete.Visibility = Visibility.Collapsed;
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int stationId = Convert.ToInt32(stationIdText.Text);
                Bl.deleteStation(stationId);
                MessageBox.Show("station deleted succesfully");
                checkDelete.Visibility = Visibility.Collapsed;
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listOfDronesAtStation_MouseDoubleClick(object sender, MouseButtonEventArgs e) //write this!!
        {
            //something here is veryyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy wrong
            //throwing a catch
            DroneInCharging updateDrone = new DroneInCharging();
            updateDrone = (DroneInCharging)listOfDronesAtStation.SelectedItem;
            Drone realDrone = new Drone();
            realDrone = Bl.getDrone(updateDrone.droneId);
            new DroneWindow(Bl, realDrone).ShowDialog();
            //droneAtStationObservableCollection = new ObservableCollection<DroneInCharging>(s.dronesAtStation);
            //listOfDronesAtStation.DataContext = droneAtStationObservableCollection;
            DataContext = s;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new StationWindow(Bl).ShowDialog();
        }

        private void listOfDronesAtStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void stationIdText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
