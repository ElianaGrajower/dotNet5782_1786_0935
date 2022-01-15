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
    /// Shows the information of indevidual stations, and actions can be done on the stations.
    /// </summary>
    public partial class StationWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        Station s;
        string cName;

        /// <summary>
        /// constructor for a new station
        /// </summary>
        /// <param name="customerName">the users name</param>
        public StationWindow(IBL Station, string customerName) 
        {
            InitializeComponent();
            this.Bl = Station;
            s = new Station();
            s.location = new Location();
            DataContext = s;
            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            inUseRead.Visibility = Visibility.Hidden;
            inUseText.Visibility = Visibility.Hidden;
            listOfDronesAtStation.Visibility = Visibility.Hidden;
            dronesAtStationRead.Visibility = Visibility.Hidden;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// constructor for an existing station 
        /// </summary>
        /// <param name="station">the existing station</param>
        /// <param name="customerName">the users name</param>
        public StationWindow(IBL b, Station station, string customerName) 
        {
            InitializeComponent();
            this.Bl = b;
            s = station;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            add.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Visible;
            inUseRead.Visibility = Visibility.Visible;
            inUseText.Visibility = Visibility.Visible;
            ourLOGO.Visibility = Visibility.Hidden;
            this.DataContext = station;
            listOfDronesAtStation.ItemsSource = station.dronesAtStation;
            stationIdText.IsEnabled = false;
            latitudeText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            inUseText.IsEnabled = false;
        }
        /// <summary>
        /// closes the window
        /// </summary>
        private void close_Click(object sender, RoutedEventArgs e)  
        {
            Close();
        }
        /// <summary>
        /// adds a new station
        /// </summary>
        private void add_Click(object sender, RoutedEventArgs e)  
        {
            try
            {
                Bl.addStation(s);
                MessageBox.Show("station added succesfully");
                s = new Station();
                s.location = new Location();
                DataContext = s;
                Close();
            }
            catch(InvalidInputException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch
            {
                MessageBox.Show("ERROR can not add station");
            }
        }
        /// <summary>
        /// updates a station
        /// </summary>
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
        /// <summary>
        /// opens a drone that is at the current station.
        /// </summary>
        private void listOfDronesAtStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)  
        {
            DroneInCharging updateDrone = new DroneInCharging();
            updateDrone = (DroneInCharging)listOfDronesAtStation.SelectedItem;
            try
            {
                if (updateDrone == null)
                    throw new Exception("wrong click");
                Drone realDrone = new Drone();
                realDrone = Bl.getDrone(updateDrone.droneId);
                new DroneWindow(Bl, realDrone, cName).ShowDialog();
                DataContext = s;
                listOfDronesAtStation.ItemsSource = s.dronesAtStation;
                Close();
            }
            catch(Exception exc)
            {
                
            }
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new StationWindow(Bl, cName).ShowDialog();
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
