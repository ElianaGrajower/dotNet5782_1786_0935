﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {

        internal readonly IBL bl = BlFactory.GetBl();
        static weightCategories? weightFilter;
        static DroneStatus? statusFilter;
        public DroneListWindow()
        {
            InitializeComponent();

        }
        private void ShowInfo()
        {
            IEnumerable<DroneToList> d = new List<DroneToList>();
            d = bl.getDronesList();
            if (StatusSelector.Text != "")
                d = this.bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter);
            if (weightSelector.Text != "")
                d = bl.allDrones(x => x.weight == (weightCategories)weightFilter);
            if (weightSelector.Text != "" && StatusSelector.Text != "")
                d = bl.allDrones(x => x.droneStatus == (DroneStatus)statusFilter && x.weight == (weightCategories)weightFilter);
            DronesListView.ItemsSource = d;
        }
        public DroneListWindow(IBL b)
        {
            InitializeComponent();
            this.bl = b;
            DronesListView.ItemsSource = b.getDronesList();
            ShowInfo();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weightSelector.ItemsSource = Enum.GetValues(typeof(weightCategories));
            
        }
        private void IBL(IBL b)
        {

        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //StatusSelector.ItemsSource = Enum.GetValues(typeof(weightCategories));     ////////****i think theyre supposed to be here

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

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weightSelector.SelectedIndex != -1)
            {
                weightFilter = (weightCategories)weightSelector.SelectedItem;
                if((int)weightFilter!=4)
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
         
         
        }
     
    }
}