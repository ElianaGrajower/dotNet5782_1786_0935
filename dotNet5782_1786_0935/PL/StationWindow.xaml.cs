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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        public StationWindow(IBL Station) //new
        {
            InitializeComponent();
            this.Bl = Station;
            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            delete.Visibility = Visibility.Hidden;
            inUseRead.Visibility = Visibility.Hidden;
            inUseText.Visibility = Visibility.Hidden;
            stationIdText.IsReadOnly = false;
            lattitudeText.IsReadOnly = false;
            longitudeText.IsReadOnly = false;
            inUseText.IsReadOnly = false;
        }

        public StationWindow(IBL b, BO.StationToList station) //update
        {
            InitializeComponent();
            this.Bl = b;
            add.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Visible;
            delete.Visibility = Visibility.Visible;
            inUseRead.Visibility = Visibility.Visible;
            inUseText.Visibility = Visibility.Visible;
            ////switch to data binding
            stationIdText.Text = station.stationId.ToString();


            ////////
            stationIdText.IsReadOnly = true;
            lattitudeText.IsReadOnly = true;
            longitudeText.IsReadOnly = true;
            inUseText.IsReadOnly = true;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Station newStation = new Station();
            try
            {
                ///not allowed!!!!!!!!!!!!!!!!! FIX!!!!!!!!!!!!!!
                newStation.stationId = Convert.ToInt32(stationIdText.Text);
                newStation.name = nameText.Text;
                newStation.location.lattitude = Convert.ToInt32(lattitudeText.Text);
                newStation.location.longitude = Convert.ToInt32(longitudeText.Text);
                newStation.chargeSlots = Convert.ToInt32(slotsText.Text);
                Bl.addStation(newStation);
                MessageBox.Show("station added succesfully");
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR invalid input");
            }
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                int stationId = Convert.ToInt32(stationIdText.Text);
                int chargeSlots = Convert.ToInt32(slotsText.Text);
                string name = nameText.Text;
                Bl.updateStation(stationId, chargeSlots, name);
                MessageBox.Show("station updated succesfully");
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
            Close();
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
                MessageBox.Show("ERROR invalid input");
            }
        }
    }
}
