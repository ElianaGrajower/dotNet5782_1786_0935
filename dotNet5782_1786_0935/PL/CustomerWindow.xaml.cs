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

using System.Net;
using System.Net.Mail;


namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>

    public partial class CustomerWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        Customer c;
        Customer cutomerParcel;
        string cName;
        
        //ObservableCollection<ParcelinCustomer>deliverObservableCollection;
        //ObservableCollection<ParcelinCustomer>receiveObservableCollection;

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
        public CustomerWindow(IBL customer, bool checkIsCustomer, bool isLogout, string custumerName="")//add new
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
        public CustomerWindow(IBL b, BO.Customer customer, string customerName)//update
        {
            InitializeComponent();
            this.bl = b;
            this.DataContext = customer;
            cutomerParcel = customer;
            sentParcelsList.ItemsSource = customer.parcelsdelivered;
            receivedParcelsList.ItemsSource = customer.parcelsOrdered;
            //deliverObservableCollection = new ObservableCollection<ParcelinCustomer>(customer.parcelsdelivered);
            //sentParcelsList.DataContext = deliverObservableCollection;
            //receiveObservableCollection = new ObservableCollection<ParcelinCustomer>(customer.parcelsOrdered);
            //receivedParcelsList.DataContext = receiveObservableCollection;
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

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            //checkDelete.Visibility = Visibility.Visible;
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
                //DataContext = c;
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
        private void sentParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e) /////////fix this!!!!!!!!!!!
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
        private void receivedParcelsList_MouseDoubleClick(object sender, MouseButtonEventArgs e) /////////fix this!!!!!!!!!!!
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

        private void passwordText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

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

        private void checkBoxTerms_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            bl.releaseAllFromCharge();
            this.Close();
        }
        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            termesConditions.Visibility = Visibility.Hidden;
        }

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
