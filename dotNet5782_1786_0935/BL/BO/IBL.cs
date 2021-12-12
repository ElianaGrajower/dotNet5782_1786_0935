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
        public List<IBL.BO.Station> GetStationsList();
        public int FindStation(Location location);
        public void AddDrone(IBL.BO.Drone DronetoAdd, int stationId);
        public bool OnlyDigits(char x);
        public IBL.BO.Station GetStation(int stationId);
        public List<IBL.BO.Station> GetStations();
        public List<IBL.BO.Station> GetAvailableStationsList();
        public int AvailableChargingSlots();
        public void AddCustomer(IBL.BO.Customer CustomertoAdd);
        public void AddStation(IBL.BO.Station StationtoAdd);
        public void AddParcel(IBL.BO.Parcel ParceltoAdd);
        public void DeleteStation(int StationId);
        public void DeleteParcel(int ParcelId);
        public void DeleteCustomer(int CustomerId);
        public void DeleteDrone(int DroneId);
        public IBL.BO.Customer GetCustomer(int customerId);
        public IBL.BO.Parcel GetParcel(int parcelId);
        public void UpdateDroneName(int droneID, string dModel);
        public void UpdateCustomerName(int CustomerId, string name, string number);
        public void ReleaseDroneFromCharge(int droneId, int chargeTime);
        public void updateStation(int stationID, int AvlblDCharges, string Name = " ");
        public void MatchDroneWithPacrel(int droneId);
        public void PickUpParcel(int droneId);
        public void DeliveredParcel(int droneId);
        public List<IBL.BO.Drone> GetDronesList();
        public List<IBL.BO.Customer> GetCustomersList();
        public List<IBL.BO.Parcel> GetParcelsList();
        public void SendDroneToCharge(int droneID);
        public List<IBL.BO.Parcel> GetUnmatchedParcelsList();
        public IBL.BO.Drone GetDrone(int id);
        public IBL.BO.DroneToList returnsDrone(int id);
    }     
}
