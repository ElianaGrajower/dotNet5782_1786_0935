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
        ObservableCollection<StationToList> stationObservableCollection;

        public StationListWindow(IBL b) 
        {
            InitializeComponent();
            this.bl = b;
            //  StationsListView.ItemsSource = b.getStationsList();
            //  ShowInfo();
            stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
            DataContext = stationObservableCollection;

        }
        //private void ShowInfo()  /////add filter
        //{
        //    IEnumerable<StationToList> d = new List<StationToList>();
        //    if (filterSlots.Text != "")
        //        StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));
        //    else
        //    {
        //        if (availableChargesSelector.SelectedItem == "available charges slots")
        //            StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);
        //        else
        //            StationsListView.ItemsSource = bl.getStationsList();
        //    }
        //}
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl).ShowDialog();
            stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
            DataContext = stationObservableCollection;
            // ShowInfo();
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
            updateStation = bl.getStation(updateStationList.stationId);
            new StationWindow(bl, updateStation).ShowDialog();
            // ShowInfo();
            if (filterSlots.Text != "")
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text)));
                StationsListView.DataContext = stationObservableCollection;
            }
            if (availableChargesSelector.SelectedIndex == 0)
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots > 0));
                StationsListView.DataContext = stationObservableCollection;
            }  
            else
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                StationsListView.DataContext = stationObservableCollection;
            }
        }

        private void availableChargesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableChargesSelector.SelectedIndex == 0)
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots > 0));
                StationsListView.DataContext = stationObservableCollection;
            }
            else
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                StationsListView.DataContext = stationObservableCollection;
            }
            filterSlots.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filterSlots.Text != "")
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text)));
                StationsListView.DataContext = stationObservableCollection;
            }
            else
            {
                stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                StationsListView.DataContext = stationObservableCollection;
            }
            availableChargesSelector.SelectedIndex = 1;
        }

        private void StationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
