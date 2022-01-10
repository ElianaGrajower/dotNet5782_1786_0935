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
        //ObservableCollection<StationToList> stationObservableCollection;
        //to remove close box from window
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        public StationListWindow(IBL b, string customerName) 
        {
            InitializeComponent();
            this.bl = b;
            StationsListView.ItemsSource = b.getStationsList();
            ShowInfo();
            expanderHeader.Text = " " + customerName;
            cName = customerName;
            //stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
            //DataContext = stationObservableCollection;
            //to remove close box from window
            Loaded += ToolWindow_Loaded;

        }
        private void ShowInfo()  /////add filter
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
            //stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
            //DataContext = stationObservableCollection;
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

                    //stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text)));
                    //StationsListView.DataContext = stationObservableCollection;
                }
                if (availableChargesSelector.SelectedIndex == 0)
                {
                    StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);

                    //stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots > 0));
                    //StationsListView.DataContext = stationObservableCollection;
                }
                else
                {
                    StationsListView.ItemsSource = bl.getStationsList();

                    //stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                    //StationsListView.DataContext = stationObservableCollection;
                }
            }
            catch (Exception exc)
            {
               // MessageBox.Show("System malfunction please wait a moment and try again\n");
            }
            
        }

        private void availableChargesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (availableChargesSelector.SelectedIndex == 0)
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots > 0);

                //    stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots > 0));
                //    StationsListView.DataContext = stationObservableCollection;
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();

                //    stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                //StationsListView.DataContext = stationObservableCollection;
            }
            filterSlots.Text = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filterSlots.Text != "")
            {
                StationsListView.ItemsSource = bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text));

                //stationObservableCollection = new ObservableCollection<StationToList>(bl.allStations(x => x.numberOfAvailableSlots == Convert.ToInt32(filterSlots.Text)));
                //StationsListView.DataContext = stationObservableCollection;
            }
            else
            {
                StationsListView.ItemsSource = bl.getStationsList();

                //stationObservableCollection = new ObservableCollection<StationToList>(bl.getStationsList());
                //StationsListView.DataContext = stationObservableCollection;
            }
            availableChargesSelector.SelectedIndex = -1;
        }

        private void StationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StationsListView.ItemsSource = bl.allStations();
            availableChargesSelector.SelectedIndex = -1;
            filterSlots.Text = "";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

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
