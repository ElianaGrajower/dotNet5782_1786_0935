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
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        string cName;
        
        public StationListWindow(IBL b, string customerName) 
        {
            InitializeComponent();
            this.bl = b;
            StationsListView.ItemsSource = b.getStationsList();
            ShowInfo();
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        private void ShowInfo() 
        {
            IEnumerable<StationToList> d = new List<StationToList>();
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl, cName).ShowDialog();
            ShowInfo();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList updateStationList = new StationToList();
            updateStationList = (StationToList)StationsListView.SelectedItem;
            Station updateStation = new Station();
            try
            {
                if (updateStationList == null)
                    throw new Exception("clicked wrong area");
                updateStation = bl.getStation(updateStationList.stationId);
                new StationWindow(bl, updateStation, cName).ShowDialog();
                ShowInfo();
                if (filterSlots.Text != "")
                {
                    StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));
                }
                if (availableChargesSelector.SelectedIndex == 0)
                {
                    StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);
                }
                else
                {
                    StationsListView.ItemsSource = bl.getStationsList();
                }
            }
            catch (Exception exc)
            {
               
            }
            
        }

        private void availableChargesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableChargesSelector.SelectedIndex == 0)
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();
            }
            filterSlots.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filterSlots.Text != "")
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();
            }
            availableChargesSelector.SelectedIndex = -1;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bl.allStations();
            availableChargesSelector.SelectedIndex = -1;
            filterSlots.Text = "";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            IEnumerable<StationToList> query = bl.allStations().OrderBy(x => x.numberOfAvailableSlots);
            StationsListView.ItemsSource = query;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            Close();
        }
      
    }
}
