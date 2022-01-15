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
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// This is the station lists, where the list of stations are and it leads to indavidual stations. 
    /// </summary>
    public partial class StationListWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        string cName;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customerName">the users name</param>
        public StationListWindow(IBL b, string customerName)  
        {
            InitializeComponent();
            this.bl = b;
            StationsListView.ItemsSource = b.getStationsList();
            ShowInfo();
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// updates the list to show the new changes
        /// </summary>
        private void ShowInfo()  
        {
            if (filterSlots.Text != "")
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));
            else
            {
                if (availableChargesSelector.SelectedIndex == 0)
                    StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);
                else
                    StationsListView.ItemsSource = bl.getStationsList();
            }
        }
        /// <summary>
        /// a button to adds a new station
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)  
        {
            new StationWindow(bl, cName).ShowDialog();
            ShowInfo();
        }
        /// <summary>
        /// a close button
        /// </summary>
        private void closeButton_Click(object sender, RoutedEventArgs e)  
        {
            Close();
        }
        /// <summary>
        /// opens a station from the list of stations
        /// </summary>
        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)  
        {
            StationToList updateStationList = new StationToList();
            updateStationList = (StationToList)StationsListView.SelectedItem;
            Station updateStation = new Station();
            try
            {
                if (updateStationList == null)  //makes sure the double click is on a station 
                    throw new Exception("clicked wrong area");
                updateStation = bl.getStation(updateStationList.stationId);
                new StationWindow(bl, updateStation, cName).ShowDialog();
                ShowInfo();
            }
            catch (Exception exc)
            {
               
            }
            
        }
        /// <summary>
        /// only shows the stations with available charge slots.
        /// </summary>
        private void availableChargesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {
            int saveIndex = availableChargesSelector.SelectedIndex;
            filterSlots.Text = "";  //clears the rest of the filter options.
            availableChargesSelector.SelectedIndex = saveIndex;
            if (availableChargesSelector.SelectedIndex == 0)
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();
            }
        }
        /// <summary>
        /// only shows the stations that have the inputed number of slots.
        /// </summary>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)  
        {
            availableChargesSelector.SelectedIndex = -1;  //clears the rest of the filter options.
            if (filterSlots.Text != "")
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();
            }
        }
        /// <summary>
        /// refresh button
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)  
        {
            StationsListView.ItemsSource = bl.allStations();
            availableChargesSelector.SelectedIndex = -1;  //clears the rest of the filter options.
            filterSlots.Text = "";
        }
        /// <summary>
        /// sorts the stations by smallest to largest amount of slots.
        /// </summary>
        private void Button_Click_4(object sender, RoutedEventArgs e)  
        {
            filterSlots.Text = "";
            availableChargesSelector.SelectedIndex = -1;
            IEnumerable<StationToList> query = bl.allStations().OrderBy(x => x.numberOfAvailableSlots);
            StationsListView.ItemsSource = query;
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
      
    }
}
