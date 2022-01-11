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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        internal readonly IBL Bl = BlFactory.GetBl();
        //  ObservableCollection<ParcelToList> myObservableCollection;
        static ParcelStatus? statusFilter;
        static Priorities? prioritiesFilter;
        string cName;
        bool isUser = false;
        Customer c = new Customer();
        


        public ParcelListWindow(IBL parcel, string customerName) //for an employee
        {
            InitializeComponent();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            ParcelsListView.ItemsSource = Bl.getParcelsList();
            statusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            prioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            expanderHeader.Text = " " + customerName;
            cName = customerName;
          
        }
        public ParcelListWindow(IBL parcel, Customer customer, string customerName) //for a customer
        {
            InitializeComponent();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            ParcelsListView.ItemsSource = Bl.allParcels().Where(x => x.sendername == customer.name || x.recivername == customer.name);  ///have to change the showinfo accordingly
            statusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            prioritySelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            isUser = true;
            c = customer;
            expanderHeader.Text = " " + customerName;
            cName = customerName;
        
        }
        public void ShowInfo()  /////majorly work on this!!!
        {
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            //   statusFilter = (ParcelStatus)statusSelector.SelectedItem;
            if (isUser)
            {
                ParcelsListView.ItemsSource = Bl.allParcels().Where(x => x.sendername == c.name || x.recivername == c.name);  ///have to change the showinfo accordingly
            }
            else
            {
                if (statusSelector.SelectedIndex != 4)
                {
                    if (statusSelector.Text != "")
                        p = Bl.allParcels(x => x.parcelStatus == statusFilter);
                    else
                        p = Bl.allParcels();
                }
                if (dateRange.SelectedIndex != -1)
                {
                    // IEnumerable<ParcelToList> p = new List<ParcelToList>();
                    p = Bl.getParcelsList();
                    if (dateRange.SelectedIndex == 0)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-1));// && Bl.getParcel(x.parcelId).requested < DateTime.Now.AddDays(1));
                    if (dateRange.SelectedIndex == 1)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-7));
                    if (dateRange.SelectedIndex == 2)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddMonths(-1));
                    if (dateRange.SelectedIndex == 3)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddYears(-1));
                    //  ParcelsListView.ItemsSource = p;
                }
                if (pickDate.ToString() != "Select a date")
                {
                    if (dateRange.SelectedIndex == 0)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-1));// && Bl.getParcel(x.parcelId).requested < DateTime.Now.AddDays(1));
                    if (dateRange.SelectedIndex == 1)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-7));
                    if (dateRange.SelectedIndex == 2)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddMonths(-1));
                    if (dateRange.SelectedIndex == 3)
                        p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddYears(-1));
                }
                ParcelsListView.ItemsSource = p;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)  //date range
        {
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            if (dateRange.SelectedIndex == 0)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-1));// && Bl.getParcel(x.parcelId).requested < DateTime.Now.AddDays(1));
            if (dateRange.SelectedIndex == 1)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddDays(-7));
            if (dateRange.SelectedIndex == 2)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddMonths(-1));
            if(dateRange.SelectedIndex==3)
                p = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested > DateTime.Now.AddYears(-1));
            ParcelsListView.ItemsSource = p;
            //pickDate.DataContext.Equals("select");
        }

        private void dateButton_Click(object sender, RoutedEventArgs e)
        {
            calender.Visibility = Visibility.Visible;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(Bl, cName).ShowDialog();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            //  ParcelsListView.ItemsSource = Bl.getParcelsList();
            ShowInfo();

        }
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
            
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
        //    ParcelsListView.ItemsSource = Bl.getParcelsList();
          

        }

        private void typeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)  //priority
        {
            prioritiesFilter = (Priorities)prioritySelector.SelectedItem;
            if ((int)prioritiesFilter != 4)
                ParcelsListView.ItemsSource = Bl.allParcels(x => x.priority == prioritiesFilter);
            else
                ParcelsListView.ItemsSource = Bl.allParcels();
        }

        private void statusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)  //status 
        {

            if (prioritySelector.SelectedIndex == -1 && dateRange.SelectedIndex == -1 && pickDate.SelectedDate != DateTime.MinValue)
            {
                statusFilter = (ParcelStatus)statusSelector.SelectedItem;
                if ((int)statusFilter != 5)
                    ParcelsListView.ItemsSource = Bl.allParcels(x => x.parcelStatus == statusFilter);
                else
                    ParcelsListView.ItemsSource = Bl.allParcels();
            }
            else if (prioritySelector.SelectedIndex != -1 && dateRange.SelectedIndex == -1 && pickDate.SelectedDate != DateTime.MinValue)
            {
                statusFilter = (ParcelStatus)statusSelector.SelectedItem;
                prioritiesFilter = (Priorities)prioritySelector.SelectedItem;
                if ((int)prioritiesFilter == 4)
                    ParcelsListView.ItemsSource = Bl.allParcels(x => x.parcelStatus == statusFilter);
                else
                {
                    if ((int)statusFilter != 5)
                        ParcelsListView.ItemsSource = Bl.allParcels(x => x.priority == prioritiesFilter && x.parcelStatus == statusFilter);
                    else
                        ParcelsListView.ItemsSource = Bl.allParcels(x => x.priority == prioritiesFilter);
                }
            }
            else
            {
                statusFilter = (ParcelStatus)statusSelector.SelectedItem;
                if ((int)statusFilter != 5)
                    ParcelsListView.ItemsSource = Bl.allParcels(x => x.parcelStatus == statusFilter);
                else
                    ParcelsListView.ItemsSource = Bl.allParcels();
            }
            //ParcelsListView.ItemsSource = Bl.allParcels();   //erase the else and continue this




            //if (prioritySelector.SelectedIndex == -1 && dateRange.SelectedIndex != -1 && pickDate.SelectedDate != DateTime.MinValue)








            //if (weightSelector.SelectedIndex != -1)
            //{
            //    weightFilter = (weightCategories)weightSelector.SelectedItem;
            //    if ((int)weightFilter != 4)
            //        DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
            //    else
            //        DronesListView.ItemsSource = bl.allDrones();
            //}
            //if (StatusSelector.SelectedIndex != -1 && weightSelector.SelectedIndex != -1)
            //{
            //    weightFilter = (weightCategories)weightSelector.SelectedItem;
            //    statusFilter = (DroneStatus)StatusSelector.SelectedItem;
            //    if ((int)weightFilter != 4 && (int)statusFilter != 4)
            //        DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter && x.weight == weightFilter);
            //    else
            //      if ((int)statusFilter != 4)
            //        DronesListView.ItemsSource = bl.allDrones(x => x.droneStatus == statusFilter);
            //    else
            //    {
            //        if ((int)weightFilter != 4)
            //            DronesListView.ItemsSource = bl.allDrones(x => x.weight == weightFilter);
            //        else
            //            DronesListView.ItemsSource = bl.allDrones();
            //    }
            //}
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)  //calender
        {
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            ParcelsListView.ItemsSource = Bl.allParcels().Where(x => Bl.getParcel(x.parcelId).requested >= pickDate.SelectedDate && Bl.getParcel(x.parcelId).requested < pickDate.SelectedDate.Value.AddDays(1));
        }

        private void DatePicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ParcelsListView.ItemsSource = Bl.allParcels();
            //dateRange.SelectedIndex = -1;
            //statusSelector.SelectedIndex = -1;
            //prioritySelector.SelectedIndex = -1;
            SortBy.SelectedIndex = -1;
            //the calander
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new UserWindow().Show();
            Bl.releaseAllFromCharge();
            Close();
        }
    }
}
