using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using BL;


namespace IBL
{
    public interface IBI
    {
        public chargeCapacity GetChargeCapacity();
        public List<IBL.BO.Station> getStationsList();
        public int FindStation(Location location);
        public void AddDrone(IBL.BO.Drone DronetoAdd, int stationId);
        public bool OnlyDigits(char x);
        public IBL.BO.Station getStation(int stationId);
        public List<IBL.BO.Station> getStations();
        public List<IBL.BO.Station> GetAvailableStationsList();
        public int AvailableChargingSlots();
        public void AddCustomer(IBL.BO.Customer CustomertoAdd);
        public void AddStation(IBL.BO.Station StationtoAdd);
        public void AddParcel(IBL.BO.Parcel ParceltoAdd);
        public void DeleteStation(int stationId);
        public void DeleteParcel(int parcelId);
        public void DeleteCustomer(int CustomerId);
        public void DeleteDrone(int droneId);
        public IBL.BO.Customer getCustomer(int customerId);
        public IBL.BO.Parcel getParcel(int parcelId);
        public void UpdateDronename(int droneId, string dModel);
        public void UpdateCustomername(int CustomerId, string name, string number); // 
        public void ReleaseDroneFromCharge(int droneId, int chargeTime); //releases the drone from its charge
        public void updateStation(int stationId, int AvlblDCharges, string name = " ");
        public void MatchDroneWithPacrel(int droneId);
        public void PickUpParcel(int droneId);
        public void deliveredParcel(int droneId);
        public List<IBL.BO.Drone> getDronesList();
        public List<IBL.BO.Customer> getCustomersList();
        public List<IBL.BO.Parcel> getParcelsList();
        public void SendDroneToCharge(int droneId);
        public List<IBL.BO.Parcel> GetUnmatchedParcelsList();
        public IBL.BO.Drone getDrone(int id);
        public IBL.BO.DroneToList returnsDrone(int id);
        public IEnumerable<IBL.BO.Station> allStations(Func<IBL.BO.Station, bool> predicate = null);
    }     
}
