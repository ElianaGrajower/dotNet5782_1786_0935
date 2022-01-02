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



        public ParcelListWindow(IBL parcel)
        {
            InitializeComponent();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            ParcelsListView.ItemsSource = Bl.getParcelsList();
            statusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));

        }
        public void ShowInfo()  /////majorly work on this!!!
        {
            IEnumerable<ParcelToList> p = new List<ParcelToList>();
            p = Bl.getParcelsList();
            //   statusFilter = (ParcelStatus)statusSelector.SelectedItem;
            if (statusSelector.SelectedIndex != 4)
            {
                if (statusSelector.Text != "")
                    p = Bl.allParcels(x => x.parcelStatus == statusFilter);
                else
                    p = Bl.allParcels();
            }
            if(dateRange.SelectedIndex!=-1)
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
            ParcelsListView.ItemsSource = p;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            new ParcelWindow(Bl).ShowDialog();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            //  ParcelsListView.ItemsSource = Bl.getParcelsList();
            ShowInfo();

        }
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList updateParcel = new ParcelToList();
            updateParcel = (ParcelToList)ParcelsListView.SelectedItem;
            Parcel realParcel = new Parcel();
            realParcel = Bl.getParcel(updateParcel.parcelId);
            new ParcelWindow(Bl, realParcel).ShowDialog();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
        //    ParcelsListView.ItemsSource = Bl.getParcelsList();
            ShowInfo();

        }

        private void typeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////first filter
        }

        private void statusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statusFilter = (ParcelStatus)statusSelector.SelectedItem;
            if ((int)statusFilter != 5)
                ParcelsListView.ItemsSource = Bl.allParcels(x => x.parcelStatus == statusFilter);
            else
                ParcelsListView.ItemsSource = Bl.allParcels();








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
    }
}
