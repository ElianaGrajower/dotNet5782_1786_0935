using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BL;


namespace BlApi
{
    public interface IBL
    {
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
        public void releaseDroneFromCharge(int droneId); //releases the drone from its charge
        public void updateStation(int stationId, int AvlblDCharges, string name = " ");
        public void matchDroneWithPacrel(int droneId);
        public void pickUpParcel(int droneId);
        public void deliveredParcel(int droneId);
        public List<BO.DroneToList> getDronesList();
        public List<BO.CustomerToList> getCustomersList();
        public List<BO.ParcelToList> getParcelsList();
        public void SendDroneToCharge(int droneId);
        public BO.Drone getDrone(int id);
        public BO.DroneToList returnsDrone(int id);
        public IEnumerable<BO.StationToList> allStations(Func<BO.StationToList, bool> predicate = null);
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null);
        public IEnumerable<BO.ParcelToList> allParcels(Func<BO.ParcelToList, bool> predicate = null);
        public IEnumerable<BO.CustomerToList> allCustomers(Func<BO.CustomerToList, bool> predicate = null);


    }     
}
