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


        public ParcelListWindow(IBL parcel)
        {
            InitializeComponent();
            //myObservableCollection = new ObservableCollection<ParcelToList>(Bl.getParcelsList());
            //DataContext = myObservableCollection;
            ParcelsListView.ItemsSource = Bl.getParcelsList();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            ParcelsListView.ItemsSource = Bl.getParcelsList();

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
            ParcelsListView.ItemsSource = Bl.getParcelsList();

        }

        private void typeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ////first filter
        }
    }
}
