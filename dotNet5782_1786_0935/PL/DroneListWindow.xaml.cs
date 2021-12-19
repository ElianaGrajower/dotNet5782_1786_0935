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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BL.BLImp bl;
        static WeightCategories? weightFilter;
        static DroneStatus? statusFilter;
        public DroneListWindow()
        {
            InitializeComponent();

        }
        private void ShowInfo()
        {
            IEnumerable<DroneToList> d = new List<DroneToList>();
            d = bl.GetDronesList();
            if (StatusSelector.Text != "")
                d = this.bl.allDrones(x => x.weight == (WeightCategories)weightFilter);
            if (WeightSelector.Text != "")
                d = bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter);
            if (WeightSelector.Text != "" && StatusSelector.Text != "")
                d = bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter && x.weight == (WeightCategories)weightFilter);
            DronesListView.ItemsSource = d;
        }
        public DroneListWindow(BL.BLImp b)
        {
            InitializeComponent();
            this.bl = b;
            DronesListView.ItemsSource = b.GetDronesList();
            ShowInfo();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            
        }
        private void IBL(BLImp b)
        {

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));     ////////****i think theyre supposed to be here

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
            ShowInfo();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList updateDrone = new DroneToList();
            updateDrone = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(bl,updateDrone).ShowDialog();
            ShowInfo();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedIndex != -1)
            {
                weightFilter = (WeightCategories)WeightSelector.SelectedItem;
                if((int)weightFilter!=4)
                DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
                else
                    DronesListView.ItemsSource = bl.allDrones();
            }
            if (StatusSelector.SelectedIndex != -1 && WeightSelector.SelectedIndex != -1)
            {
                weightFilter = (WeightCategories)WeightSelector.SelectedItem;
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
            if (StatusSelector.SelectedIndex != -1 && WeightSelector.SelectedIndex != -1)
            {
                statusFilter = (DroneStatus)StatusSelector.SelectedItem;
                weightFilter = (WeightCategories)WeightSelector.SelectedItem;
                if ((int)weightFilter != 4 && (int)statusFilter != 4)
                    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter && x.weight == weightFilter);
                else
                    if((int)statusFilter != 4)
                        DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
                    else
                    {  
                        if ((int)weightFilter != 4 )
                        DronesListView.ItemsSource = bl.allDrones(x =>  x.weight == weightFilter);
                         else
                        DronesListView.ItemsSource = bl.allDrones();
                    }
                
            }
         
            //if (WeightSelector.SelectedIndex != -1)
            //{
            //    statusFilter = (DroneStatus)StatusSelector.SelectedItem;
            //    DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
            //}
        }
        //private void DoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DroneToList drtl = new DroneToList();
        //    drtl = (DroneToList)DronesListView.SelectedItem;
        //    new DroneWindow(bl, drtl).ShowDialog();
        //  //  fillListView();
        //}
    }
}