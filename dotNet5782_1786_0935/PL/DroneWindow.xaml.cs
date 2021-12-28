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


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        public DroneWindow(IBL b,BO.DroneToList drone)//update drone
        {
            InitializeComponent();
            //ShowInfo();
            this.bl = b;

            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s=>s.numberOfAvailableSlots>0).Select(s=>s.stationId);
            update.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Hidden;
            if (drone.droneStatus == BO.DroneStatus.available)
            {
                chargeDrone.Visibility = Visibility.Visible;
                releaseDrone.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Visible;
                pickupParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Hidden;
            }
            if (drone.droneStatus == BO.DroneStatus.maintenance)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Hidden;
            }
            if (drone.droneStatus == BO.DroneStatus.delivery)
            {
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Hidden;
            }
            longitudeText.Visibility = Visibility.Visible;
            lattitudeText.Visibility = Visibility.Visible;
            stationIdCombo.Visibility = Visibility.Hidden;
            longitudeRead.Visibility = Visibility.Visible;
            lattitudeRead.Visibility = Visibility.Visible;
            stationRead.Visibility = Visibility.Hidden;
            weightText.Visibility = Visibility.Visible;
            weight.Visibility = Visibility.Hidden;
            idText.Text = drone.droneId.ToString();
            modelText.Text = drone.model.ToString();
            lattitudeText.Text = drone.location.lattitude.ToString();
            longitudeText.Text = drone.location.longitude.ToString();
            weightText.Text = drone.weight.ToString();
            weightText.IsReadOnly = true;
            idText.IsReadOnly = true;
        }
        public DroneWindow(IBL drone)//new drone
        {
            InitializeComponent();
            this.bl = drone;
            weight.ItemsSource = Enum.GetValues(typeof(weightCategories));
            stationIdCombo.ItemsSource = bl.allStations(s => s.numberOfAvailableSlots > 0).Select(s=>s.stationId);
            //  stationIdCombo.Items.Add(Bl.getStationsList());
            update.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Visible;
            chargeDrone.Visibility = Visibility.Hidden;
            releaseDrone.Visibility = Visibility.Hidden;
            matchUpParcel.Visibility = Visibility.Hidden;
            pickupParcel.Visibility = Visibility.Hidden;
            deliverParcel.Visibility = Visibility.Hidden;
            longitudeText.Visibility = Visibility.Hidden;
            lattitudeText.Visibility = Visibility.Hidden;
            stationIdCombo.Visibility = Visibility.Visible;
            longitudeRead.Visibility = Visibility.Hidden;
            lattitudeRead.Visibility = Visibility.Hidden;
            stationRead.Visibility = Visibility.Visible;
            weightText.Visibility = Visibility.Hidden;
            weight.Visibility = Visibility.Visible;
            weightText.IsReadOnly = false;
            idText.IsReadOnly = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            string name = modelText.Text.ToString();
            try
            {
                int id = Convert.ToInt32(idText.Text);
                bl.UpdateDronename(id, name);
                MessageBox.Show("drone model updated succesfuly");
            }
            catch(DoesntExistException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Drone newDrone = new Drone();
            //try
            //{
            //    newDrone.droneId = Convert.ToInt32(idText.Text);
            //    newDrone.Maxweight = (weightCategories)weight.SelectedItem;
            //    newDrone.Model = modelText.Text;
            //    int stationId = Convert.ToInt32(stationIdText.Text);
            //}
            //catch
            //{ MessageBox.Show("input incurect"); }
            try
            {
                newDrone.droneId = Convert.ToInt32(idText.Text);
                newDrone.maxWeight = (weightCategories)weight.SelectedItem;
                newDrone.model = modelText.Text;
                int stationId = Convert.ToInt32(stationIdCombo.SelectedItem);
                bl.addDrone(newDrone, stationId);
                MessageBox.Show("added drone succesfully");
                addAnotherDrone.Visibility = Visibility.Visible;
            }
            catch(BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch(Exception exc)
            {
                MessageBox.Show("invalid input");
            }
        }
        private void releaseDrone_Click(object sender, RoutedEventArgs e)
        {
            releaseCanvas.Visibility = Visibility.Visible;
        }
        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weight.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            int time;
            try
            {
                if (Convert.ToInt32(releaseTime.Text) == null)
                    throw new BO.InvalidInputException("Invalid input!\n");
            }

            catch(InvalidInputException exc)
            {
                MessageBox.Show(exc.Message);
            }
           
            time = Convert.ToInt32(releaseTime.Text);
                try
            {
                bl.releaseDroneFromCharge(Convert.ToInt32(idText.Text), time);
                MessageBox.Show("drone released succesfully");
                releaseDrone.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Visible;
                
            }
            catch (BO.DoesntExistException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            releaseCanvas.Visibility = Visibility.Collapsed;
            

        }

        private void deliverParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.deliveredParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone delivered succesfully");
                deliverParcel.Visibility = Visibility.Hidden;
                matchUpParcel.Visibility = Visibility.Visible;
                pickupParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Visible;
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void matchUpParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.matchDroneWithPacrel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone matched up succesfully");
                matchUpParcel.Visibility = Visibility.Hidden;
                pickupParcel.Visibility = Visibility.Visible;
                deliverParcel.Visibility = Visibility.Hidden;
                chargeDrone.Visibility = Visibility.Hidden;
            }
             catch(BO.DoesntExistException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void pickupParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.pickUpParcel(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone pickedUp succesfully");
                pickupParcel.Visibility = Visibility.Hidden;
                deliverParcel.Visibility = Visibility.Visible;
                matchUpParcel.Visibility = Visibility.Hidden;
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void chargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToCharge(Convert.ToInt32(idText.Text));
                MessageBox.Show("drone charging succesfully");
                chargeDrone.Visibility = Visibility.Hidden;
                releaseDrone.Visibility = Visibility.Visible;
            }
            catch (BO.AlreadyExistsException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.InvalidInputException exc)
            { MessageBox.Show(exc.Message); }
            catch (BO.UnableToCompleteRequest exc)
            { MessageBox.Show(exc.Message); }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
            addAnotherDrone.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
     

    }
}
