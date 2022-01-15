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
    /// This is the parcels lists, where the list of parcels are and it leads to indavidual parcels. 
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        static ParcelStatus? statusFilter;
        static Priorities? prioritiesFilter;
        string cName;
        bool isUser = false;
        Customer c = new Customer();


        /// <summary>
        /// constructor for an employee
        /// </summary>
        /// <param name="customerName">the users name</param>
        public ParcelListWindow(IBL parcel, string customerName) 
        {
            InitializeComponent();
            ParcelsListView.ItemsSource = Bl.getParcelsList();
            statusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            prioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// constructor for a customer (only shows the parcels that belog to the customer)
        /// </summary>
        /// <param name="customer">the customer</param>
        /// <param name="customerName">the users name</param>
        public ParcelListWindow(IBL parcel, Customer customer, string customerName) 
        {
            InitializeComponent();
            ParcelsListView.ItemsSource = Bl.allParcels().Where(x => x.sendername == customer.name || x.recivername == customer.name);  ///have to change the showinfo accordingly
            statusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            prioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            isUser = true;
            c = customer;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        }
        /// <summary>
        /// updates the list to show the new changes
        /// </summary>
        public void ShowInfo()  
        {
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            if (isUser)
            {
                ParcelsListView.ItemsSource = Bl.allParcels().Where(x => x.sendername == c.name || x.recivername == c.name);  ///have to change the showinfo accordingly
            }
            else
            {
                if (statusSelector.SelectedIndex != -1) 
                {
                    statusFilter = (ParcelStatus)statusSelector.SelectedItem;
                    if ((int)statusFilter != 5)
                        p = Bl.allParcels(x => x.parcelStatus == statusFilter);
                    else
                        p = Bl.allParcels();
                }
                if (dateRange.SelectedIndex != -1)
                {
                    p = Bl.getParcelsList();
                    if (dateRange.SelectedIndex == 0)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-1));
                    if (dateRange.SelectedIndex == 1)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-7));
                    if (dateRange.SelectedIndex == 2)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddMonths(-1));
                    if (dateRange.SelectedIndex == 3)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddYears(-1));
                }
                if (pickDate.Text != null)
                {
                    ParcelsListView.ItemsSource = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested >= pickDate.SelectedDate && Bl.getParcel(x.parcelId).requested < pickDate.SelectedDate.Value.AddDays(1));                    
                }
                if (SortBy.SelectedIndex != -1) 
                {
                    if (SortBy.SelectedIndex == 0)
                    {
                        ParcelsListView.ItemsSource = Bl.allParcels();
                        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                        PropertyGroupDescription groupDescription = new PropertyGroupDescription("sendername");
                        view.GroupDescriptions.Add(groupDescription);
                    }
                    else
                    {
                        ParcelsListView.ItemsSource = Bl.allParcels();
                        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                        PropertyGroupDescription groupDescription = new PropertyGroupDescription("recivername");
                        view.GroupDescriptions.Add(groupDescription);
                    }
                }
                if (prioritySelector.SelectedIndex != -1) 
                {
                    prioritiesFilter = (Priorities)prioritySelector.SelectedItem;
                    if ((int)prioritiesFilter != 4)
                        ParcelsListView.ItemsSource = Bl.allParcels(x => x.priority == prioritiesFilter);
                    else
                        ParcelsListView.ItemsSource = Bl.allParcels();
                }
                ParcelsListView.ItemsSource = p;
            }
        }
        /// <summary>
        /// filter list by date range
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {
            int saveIndex = dateRange.SelectedIndex;  //makes sure the selection is saves while the rest of the selections are being cleared.
            SortBy.SelectedIndex = -1;  //clears the rest of the filter options.
            statusSelector.SelectedIndex = -1;
            prioritySelector.SelectedIndex = -1;
            pickDate.Text = null;
            dateRange.SelectedIndex = saveIndex;
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            if (dateRange.SelectedIndex == 0)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-1));
            if (dateRange.SelectedIndex == 1)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-7));
            if (dateRange.SelectedIndex == 2)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddMonths(-1));
            if(dateRange.SelectedIndex==3)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddYears(-1));
            ParcelsListView.ItemsSource = p;
        }
        /// <summary>
        /// closes the window
        /// </summary>
        private void closeButton_Click(object sender, RoutedEventArgs e)  
        {
            Close();
        }
        /// <summary>
        /// opens a window for a new parcel
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)  
        {
            new ParcelWindow(Bl, cName).ShowDialog();
            ShowInfo();
        }
        /// <summary>
        /// opens a window for an existing parcel
        /// </summary>
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)  
        {
            ParcelToList updateParcel = new ParcelToList();
            updateParcel = (ParcelToList)ParcelsListView.SelectedItem;
            try
            {
                if (updateParcel == null)
                    throw new Exception("clicked wrong area");
                Parcel realParcel = new Parcel();
                realParcel = Bl.getParcel(updateParcel.parcelId);
                new ParcelWindow(Bl, realParcel, cName).ShowDialog();
                ShowInfo();
            }
            catch (Exception exc)
            {
              
            }
        }
        /// <summary>
        /// sorts list by priority
        /// </summary>
        private void typeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {
            int saveIndex = prioritySelector.SelectedIndex;  //makes sure the selection is saves while the rest of the selections are being cleared.
            statusSelector.SelectedIndex = -1;  //clears the rest of the filter options.
            SortBy.SelectedIndex = -1;
            dateRange.SelectedIndex = -1;
            pickDate.Text = null;
            prioritySelector.SelectedIndex = saveIndex;
            if (prioritySelector.SelectedIndex != -1)
            {
                prioritiesFilter = (Priorities)prioritySelector.SelectedItem;
                if ((int)prioritiesFilter != 4)
                    ParcelsListView.ItemsSource = Bl.allParcels(x => x.priority == prioritiesFilter);
                else
                    ParcelsListView.ItemsSource = Bl.allParcels();
            }
        }
        /// <summary>
        /// sorts list by status
        /// </summary>
        private void statusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)   
        {
            try
            {
                int saveIndex = statusSelector.SelectedIndex; //makes sure the selection is saves while the rest of the selections are being cleared.
                if (statusSelector.SelectedIndex != -1)
                {
                    SortBy.SelectedIndex = -1; //clears the rest of the filter options.
                    dateRange.SelectedIndex = -1;
                    prioritySelector.SelectedIndex = -1;
                    pickDate.Text = null;
                    statusSelector.SelectedIndex = saveIndex;
                    statusFilter = (ParcelStatus)statusSelector.SelectedItem;
                    if ((int)statusFilter != 5)
                        ParcelsListView.ItemsSource = Bl.allParcels(x => x.parcelStatus == statusFilter);
                    else
                        ParcelsListView.ItemsSource = Bl.allParcels();
                }
            }
            catch
            { }
        }
        /// <summary>
        /// sorts list by calender
        /// </summary>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)  
        {
            DateTime? saveDate = pickDate.SelectedDate; //makes sure the selection is saves while the rest of the selections are being cleared.
            if (pickDate.Text != null)
            {
                SortBy.SelectedIndex = -1; //clears the rest of the filter options.
                statusSelector.SelectedIndex = -1;
                dateRange.SelectedIndex = -1;
                prioritySelector.SelectedIndex = -1;
                pickDate.SelectedDate = saveDate;
                ParcelsListView.ItemsSource = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested >= pickDate.SelectedDate && Bl.getParcel(x.parcelId).requested < pickDate.SelectedDate.Value.AddDays(1));
            }
        }
        /// <summary>
        /// refreshes the list
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)  
        {
            SortBy.SelectedIndex = -1;  //clears the rest of the filter options.
            statusSelector.SelectedIndex = -1;
            pickDate.Text = null;
            dateRange.SelectedIndex = -1;
            prioritySelector.SelectedIndex = -1;
            ParcelsListView.ItemsSource = Bl.allParcels();

        }
        /// <summary>
        /// sortd by sender/reciever
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)  
        {
            int saveIndex = SortBy.SelectedIndex; //makes sure the selection is saves while the rest of the selections are being cleared.
            statusSelector.SelectedIndex = -1; //clears the rest of the filter options.
            dateRange.SelectedIndex = -1;
            prioritySelector.SelectedIndex = -1;
            pickDate.Text = null;
            SortBy.SelectedIndex = saveIndex;
            if (SortBy.SelectedIndex == 0)
            {
                ParcelsListView.ItemsSource = Bl.allParcels();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("sendername");
                view.GroupDescriptions.Add(groupDescription);
            }
            else
            {
                ParcelsListView.ItemsSource = Bl.allParcels();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("recivername");
                view.GroupDescriptions.Add(groupDescription);
            }
        }
        /// <summary>
        /// logs out of the account
        /// </summary>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)  
        {
            new UserWindow().Show();
            Bl.releaseAllFromCharge();
            Close();
        }
    }
}
