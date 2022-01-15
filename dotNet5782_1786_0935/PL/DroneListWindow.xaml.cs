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
using BO;
using BlApi;
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// This is the drone lists, where the list of drones are and it leads to indavidual drones. 
    /// </summary>
    public partial class DroneListWindow : Window
    {

        internal readonly IBL bl = BlFactory.GetBl();
        static weightCategories? weightFilter;
        static DroneStatus? statusFilter;
        string cName;

        /// <summary>
        /// updates the list to show the new changes
        /// </summary>
        private void ShowInfo()
        {
            IEnumerable<DroneToList> d = new List<DroneToList>();
            d = bl.getDronesList();
            if (weightSelector.SelectedIndex != 3)
            {
                if (StatusSelector.Text != "")
                    d = this.bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter);
                if (weightSelector.Text != "")
                    d = bl.allDrones(x => x.weight == (weightCategories)weightFilter);
                if (weightSelector.Text != "" && StatusSelector.Text != "")
                    d = bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter && x.weight == (weightCategories)weightFilter);
            }
            DronesListView.ItemsSource = d;
        }
        /// <summary>
        /// constructor
        /// </summary>
        public DroneListWindow(IBL b, string customerName)
        {
            InitializeComponent();
            this.bl = b;
            DronesListView.ItemsSource = b.getDronesList();
            ShowInfo();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weightSelector.ItemsSource = Enum.GetValues(typeof(weightCategories));
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        private void IBL(IBL b)
        {

        }
        /// <summary>
        /// opens a window to add a new drone
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl, cName).ShowDialog();
            ShowInfo();
        }
        /// <summary>
        /// closes the wwindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// opens an existing drone
        /// </summary>
        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList updateDrone = new DroneToList();
            updateDrone = (DroneToList)DronesListView.SelectedItem;
            Drone realDrone = new Drone();
            try
            {
                if (updateDrone == null)
                    throw new Exception("clicked wrong area");
                realDrone = bl.getDrone(updateDrone.droneId);
                new DroneWindow(bl, realDrone, cName, this).ShowDialog();
                ShowInfo();
            }
            catch (Exception exc)
            {
                
            }
        }
        /// <summary>
        /// filters the list by its weight
        /// </summary>
        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weightSelector.SelectedIndex != -1)
            {
                weightFilter = (weightCategories)weightSelector.SelectedItem;
                if ((int)weightFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
                else
                    DronesListView.ItemsSource = bl.allDrones();
            }
            if (StatusSelector.SelectedIndex != -1 && weightSelector.SelectedIndex != -1)
            {
                weightFilter = (weightCategories)weightSelector.SelectedItem;
                statusFilter = (DroneStatus)StatusSelector.SelectedItem;
                if ((int)weightFilter != 4 && (int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter && x.weight == weightFilter);
                else
                  if ((int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
                else
                {
                    if ((int)weightFilter != 4)
                        DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
                    else
                        DronesListView.ItemsSource = bl.allDrones();
                }
            }
        }
        /// <summary>
        /// filters the list by its status
        /// </summary>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex != -1)
            {
                statusFilter = (DroneStatus)StatusSelector.SelectedItem;
                if ((int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
                else
                    DronesListView.ItemsSource = bl.allDrones();
            }
            if (StatusSelector.SelectedIndex != -1 && weightSelector.SelectedIndex != -1)
            {
                statusFilter = (DroneStatus)StatusSelector.SelectedItem;
                weightFilter = (weightCategories)weightSelector.SelectedItem;
                if ((int)weightFilter != 4 && (int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter && x.weight == weightFilter);
                else
                    if ((int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
                else
                {
                    if ((int)weightFilter != 4)
                        DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
                    else
                        DronesListView.ItemsSource = bl.allDrones();
                }
            }
        }
        /// <summary>
        /// refreshes the list
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = bl.allDrones();
            StatusSelector.SelectedIndex = -1;
            weightSelector.SelectedIndex = -1;
        }
        /// <summary>
        /// groups the list according to its status
        /// </summary>
        private void groupButton_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = bl.allDrones();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("droneStatus");
            view.GroupDescriptions.Add(groupDescription);
            StatusSelector.SelectedIndex = -1;
            weightSelector.SelectedIndex = -1;
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
        /// <summary>
        /// updates the list of drones for the simulation
        /// </summary>
        public void updateListView()
        {
            ShowInfo();
        }

    }
}