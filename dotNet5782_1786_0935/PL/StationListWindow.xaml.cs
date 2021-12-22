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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    public partial class StationListWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        public StationListWindow(IBL b) 
        {
            InitializeComponent();
            this.bl = b;
            StationsListView.ItemsSource = b.getStationsList();
            ShowInfo();

        }
        private void ShowInfo()
        {
            IEnumerable<StationToList> d = new List<StationToList>();
            d = bl.getStationsList();
            //make this for a station!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!**************************************************

            //if (availableChargesSelector.Text != "")
            //    d = this.bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter);
            //if (weightSelector.Text != "")
            //    d = bl.allDrones(x => x.weight == (weightCategories)weightFilter);
            //if (weightSelector.Text != "" && StatusSelector.Text != "")
            //    d = bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter && x.weight == (weightCategories)weightFilter);
            //DronesListView.ItemsSource = d;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl).Show();
            ShowInfo();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StationsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StationToList updateStation = new StationToList();
            updateStation = (StationToList)StationsListView.SelectedItem;
            new StationWindow(bl, updateStation).ShowDialog();
            ShowInfo();
        }
    }
}
