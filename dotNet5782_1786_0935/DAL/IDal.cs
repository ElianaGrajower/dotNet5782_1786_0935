using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IDAL  
{
    namespace DO
    {
        public interface IDal
        {
            // public DalObject();// { DataSource.Initialize(); } // default constructer calls on initialize func
            public void AddStation(Station station); //adds station to list
            public void AddDrone(Drone drone); //adds drone to list
            public void AddCustomer(Customer customer); //adds customer to list
            public void AddParcel(Parcel parcel); //adds parcel to list
            public List<Station> printStationsList(); //prints list of stations 
            public List<Drone> printDronesList(); //prints list of drone
            public List<Customer> printCustomersList(); //prints customer list
            public List<Parcel> printParcelsList(); //prints parcel list
            public string matchUpParcel(Parcel parcel); //matches up package with drone
            public string pickUpParcel(Customer customer, Parcel parcel); //matches up packg with sender of pckg
            public string deliverParcel(Customer customer, Parcel parcel, int priorityLevel); //matches up parcel with buyer
            public string chargeDrone(Drone drone, int stationNum); //charges drone
            public string releaseDrone(DroneCharge charge); //releases drone from charge
            public string PrintStation(int stationId); //prints a station
            public string PrintDrone(int droneId); //prints a drone
            public string PrintCustomer(int customerId); //prints a customer
            public string PrintParcel(int parcelId); //prints a parcel
            public Parcel findParcel(int parcelId); //finds a parcel using its id
            public Customer findCustomer(int customerId); //finds a customer using its id
            public Drone findDrone(int droneId); //finds a drone using its id
            public Station findStation(int stationId); //finds a station using its id
            public DroneCharge findDroneCharge(int droneChargeId); //finds a drone charge using its id
            public int getParcelId(); //returns parcel id
            public double distance(double lattitude1, double longitute1, double lattitude2, double longitute2); //calculates distance between coordinates for bonus
            public double[] ChargeCopacity();


        }
    }
}
