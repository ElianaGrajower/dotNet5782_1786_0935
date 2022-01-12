using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BL;
using System.Net.Mail;

namespace BlApi
{
    public interface IBL
    {
        public int searchCustomer(string userName);
        public bool isEmployee(string userName, string password);
        public chargeCapacity getChargeCapacity();
        public List<BO.StationToList> getStationsList();
        public void addDrone(BO.Drone DronetoAdd, int stationId);
        public BO.Station getStation(int stationId);
        public List<BO.Station> getStations();
        public void addCustomer(BO.Customer CustomertoAdd);
        public void addStation(BO.Station StationtoAdd);
        public int addParcel(BO.Parcel ParceltoAdd);
        public void deleteStation(int stationId);
        public void deleteParcel(int parcelId);
        public void deleteCustomer(int CustomerId);
        public void deleteDrone(int droneId);
        public BO.Customer getCustomer(int customerId);
        public BO.Parcel getParcel(int parcelId);
        public void UpdateDronename(int droneId, string dmodel);
        public void UpdateCustomer(int customerId, string name, string number);
        public void releaseDroneFromCharge(int droneId); //releases the drone from its charge  ///THIS
        public void updateStation(int stationId, int AvlblDCharges, string name = " ");
        public void matchDroneWithPacrel(int droneId); //THIS
        public void pickUpParcel(int droneId);  //THIS
        public void deliveredParcel(int droneId);  //THIS
        public List<BO.DroneToList> getDronesList();
        public List<BO.CustomerToList> getCustomersList();
        public List<BO.CustomerToList> getUsersList();
        public List<BO.CustomerToList> getEmployeesList();
        public List<BO.ParcelToList> getParcelsList();
        public void SendDroneToCharge(int droneId);  //THIS
        public BO.Drone getDrone(int id);
        public BO.DroneToList returnsDrone(int id);
        public IEnumerable<BO.StationToList> allStations(Func<BO.StationToList, bool> predicate = null);
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null);
        public IEnumerable<BO.ParcelToList> allParcels(Func<BO.ParcelToList, bool> predicate = null);
        public IEnumerable<BO.CustomerToList> allCustomers(Func<BO.CustomerToList, bool> predicate = null);
        public void releaseAllFromCharge();
        public double distance(BO.Location l1, BO.Location l2);
        public void openSimulator(int droneId, Action updateView, Func<bool> isRun);

    }
}
