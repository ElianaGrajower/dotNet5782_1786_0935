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

using System.Net;
using System.Net.Mail;


namespace PL
{
    /// <summary>
    /// Shows the information of indevidual cuatomers, and actions can be done on the customer.
    /// </summary>

    public partial class CustomerWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Customer c;
        Customer cutomerParcel;
        string cName;
        /// <summary>
        /// constructor for a new user
        /// </summary>
        /// <param name="checkIsCustomer">checks if the user is a customer or an employee</param>
        /// <param name="isLogout">checks if its logged into an account</param>
        /// <param name="custumerName">the users name</param>
        public CustomerWindow(IBL customer, bool checkIsCustomer, bool isLogout, string custumerName="")
        {
            InitializeComponent();
            this.bl = customer;
            c = new Customer();
            c.location = new Location();
            c.isCustomer = checkIsCustomer;
            DataContext = c;   
            sentParcelsList.Visibility = Visibility.Hidden;
            receivedParcelsList.Visibility = Visibility.Hidden;
            sentRead.Visibility = Visibility.Hidden;
            receivedRead.Visibility = Visibility.Hidden;
            updateButton.Visibility = Visibility.Hidden;
            if (isLogout)
            {
                expanderHeader.Text = " " + custumerName;
                cName = custumerName;
            }
            else
            {
                logout.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// constructor for an existing user
        /// </summary>
        /// <param name="customer">the existing customer</param>
        /// <param name="customerName">the users name</param>
        public CustomerWindow(IBL b, BO.Customer customer, string customerName)
        {
            InitializeComponent();
            this.bl = b;
            this.DataContext = customer;
            cutomerParcel = customer;
            sentParcelsList.ItemsSource = customer.parcelsdelivered;
            receivedParcelsList.ItemsSource = customer.parcelsOrdered;
            idText.IsEnabled = false;
            latitudeText.IsEnabled = false;
            longitudeText.IsEnabled = false;
            emailText.IsEnabled = false;
            ourLOGO.Visibility = Visibility.Hidden;
            addButton.Visibility = Visibility.Hidden;
            passwordRead.Visibility = Visibility.Hidden;
            passwordText.Visibility = Visibility.Hidden;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// closes the window
        /// </summary>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// updates the info of the user
        /// </summary>
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(idText.Text);
                string name = nameText.Text;
                string phone = phoneText.Text;
                bl.UpdateCustomer(customerId, name, phone);
                MessageBox.Show("customer updated succesfully");
            }
            catch(InvalidInputException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch
            {
                MessageBox.Show("ERROR invalid input");
            }
        }
        /// <summary>
        /// opens the terms and conditions
        /// </summary>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            termesConditions.Visibility = Visibility.Visible;
            continueButton.IsEnabled = false;
        }
        /// <summary>
        /// opens a parcel that the user sent
        /// </summary>
        private void sentParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            ParcelinCustomer updateParcel = new ParcelinCustomer();
            updateParcel = (ParcelinCustomer)sentParcelsList.SelectedItem;
            Parcel realParcel = new Parcel();  
            try
            {
                if (updateParcel == null)
                    throw new Exception("wrong button click");
                realParcel = bl.getParcel(updateParcel.parcelId);
                new ParcelWindow(bl, realParcel, cName).ShowDialog();
            }
            catch(Exception exc)
            {
                
            }
        }
        /// <summary>
        /// opens a parcel that the user received
        /// </summary>
        private void receivedParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            ParcelinCustomer updateParcel = new ParcelinCustomer();
            updateParcel = (ParcelinCustomer)receivedParcelsList.SelectedItem;
            Parcel realParcel = new Parcel();
            try
            {
                if(updateParcel==null)
                    throw new Exception("wrong button click");
                realParcel = bl.getParcel(updateParcel.parcelId);
                new ParcelWindow(bl, realParcel, cName).ShowDialog();
            }
            catch (Exception exc)
            {
               
            }
        }
        /// <summary>
        /// adds a new usert
        /// </summary>
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            SendEmail();
            termesConditions.Visibility = Visibility.Hidden;
            try
            {
                bl.addCustomer(c);       
                MessageBox.Show("added customer succesfully");
                c = new Customer();
                c.location = new Location();
                DataContext = c;
                Close();
            }
            catch (InvalidInputException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch
            {
                MessageBox.Show("ERROR can not add customer");
            }
        }
        /// <summary>
        /// logs out of the account
        /// </summary>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            this.Close();
        }
        /// <summary>
        /// closes the terms and conditions
        /// </summary>
        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            termesConditions.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// sends an email to the new user
        /// </summary>
        private void SendEmail()
        {
            // Create a System.Net.Mail.MailMessage object
            MailMessage message = new MailMessage();

            // Add a recipient
            message.To.Add(emailText.Text);

            // Add a message subject
            message.Subject = "Account activation";

            // Add a message body
            message.Body = "Hi " + c.name + "\n" +
                "Welcome to DroneDrop.\n" +
                "We are so happy you have chosen to join us  and  are sure you will enjoy our services.\n" +
                "For any question, problems or requests you can contact our customer service team " +
                "at customerService@DroneDrop.com and they will gladly assist you.\n\n" +
                "Thank you and have a great day,\n" +
                "Team DroneDrop";

            // Create a System.Net.Mail.MailAddress object and 
            // set the sender email address and display name.
            message.From = new MailAddress("dronedrop2021@gmail.com", "DroneDrop");

            // Create a System.Net.Mail.SmtpClient object
            // and set the SMTP host and port number
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            // If your server requires authentication add the below code
            // =========================================================
            // Enable Secure Socket Layer (SSL) for connection encryption
            smtp.EnableSsl = true;

            // Do not send the DefaultCredentials with requests
            smtp.UseDefaultCredentials = false;

            // Create a System.Net.NetworkCredential object and set
            // the username and password required by your SMTP account
            smtp.Credentials = new NetworkCredential("dronedrop2021@gmail.com", "ouiatjczzhkvtxnr");
            // =========================================================

            // Send the message
            smtp.Send(message);
        }
    }
}
