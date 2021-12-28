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
using BO;
using BlApi;
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Customer c;
        ObservableCollection<ParcelinCustomer>deliverObservableCollection;
        ObservableCollection<ParcelinCustomer>receiveObservableCollection;

        //public CustomerWindow()
        //{
        //    InitializeComponent();
        //    c = new Customer();
        //    c.location = new Location();
        //    DataContext = c;
        //    sentParcelsList.Visibility = Visibility.Hidden;
        //    receivedParcelsList.Visibility = Visibility.Hidden;
        //    sentRead.Visibility = Visibility.Hidden;
        //    receivedRead.Visibility = Visibility.Hidden;

        //}
        public CustomerWindow(IBL customer)//add new
        {
            InitializeComponent();
            this.bl = customer;
            c = new Customer();
            c.location = new Location();
            DataContext = c;
            sentParcelsList.Visibility = Visibility.Hidden;
            receivedParcelsList.Visibility = Visibility.Hidden;
            sentRead.Visibility = Visibility.Hidden;
            receivedRead.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;
            deleteButton.Visibility = Visibility.Hidden;



        }
        public CustomerWindow(IBL b, BO.Customer customer)//update
        {
            InitializeComponent();
            this.bl = b;
            this.DataContext = customer;

            deliverObservableCollection = new ObservableCollection<ParcelinCustomer>(customer.parcelsdelivered);
            sentParcelsList.DataContext = deliverObservableCollection;
            receiveObservableCollection = new ObservableCollection<ParcelinCustomer>(customer.parcelsOrdered);
            receivedParcelsList.DataContext = receiveObservableCollection;
            idText.IsEnabled = false;
            latitudeText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            addButton.Visibility = Visibility.Hidden;
           

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            checkDelete.Visibility = Visibility.Visible;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(idText.Text);
                string name = nameText.Text;
                string phone = phoneText.Text;
                bl.UpdateCustomer(customerId, name, phone);
                MessageBox.Show("customer updated succesfully");
                DataContext = c;
            }
            catch
            {
                MessageBox.Show("ERROR invalid input");
            }
        }

        private void sentParcelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void idText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            termesConditions.Visibility = Visibility.Visible;
            continueButton.IsEnabled = false;
        }
        private void sentParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(bl).ShowDialog();
        }
        private void receivedParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(bl).ShowDialog();
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(idText.Text);
                bl.deleteCustomer(customerId);
                MessageBox.Show("customer deleted succesfully");
                checkDelete.Visibility = Visibility.Collapsed;
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            checkDelete.Visibility = Visibility.Collapsed;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            termesConditions.Visibility = Visibility.Collapsed;
            checkBoxTerms.IsChecked = false;
            try
            {
                bl.addCustomer(c);
                MessageBox.Show("added customer succesfully");
                c = new Customer();
                c.location = new Location();
                DataContext = c;
                Close();
            }
            catch
            {
                MessageBox.Show("ERROR can not add customer");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) //switch to binding!!!!!!!!!!!!!!!
        {
            continueButton.IsEnabled = true;
         //   checkBoxTerms.Foreground = "#FF268E75";

        }
    }
}