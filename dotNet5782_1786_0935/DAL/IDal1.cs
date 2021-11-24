using IDAL.DO;
using System.Collections.Generic;

namespace DAL.DalObject
{
    public interface IDal
    {
        void AddCustomer(Customer customer);
        void AddDrone(Drone drone);
        void AddParcel(Parcel parcel);
        void AddStation(Station station);
        string chargeDrone(Drone drone, int stationNum);
        string deliverParcel(Customer customer, Parcel parcel, int priorityLevel);
        double distance(double lattitude1, double longitute1, double lattitude2, double longitute2);
        Customer findCustomer(int customerId);
        Drone findDrone(int droneId);
        DroneCharge findDroneCharge(int droneChargeId);
        Parcel findParcel(int parcelId);
        Station findStation(int stationId);
        int getParcelId();
        string matchUpParcel(Parcel parcel);
        string pickUpParcel(Customer customer, Parcel parcel);
        string PrintCustomer(int customerId);
        List<Customer> printCustomersList();
        string PrintDrone(int droneId);
        List<Drone> printDronesList();
        string PrintParcel(int parcelId);
        List<Parcel> printParcelsList();
        string PrintStation(int stationId);
        List<Station> printStationsList();
        string releaseDrone(DroneCharge charge);
    }
}