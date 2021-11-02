using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            public static Random r = new Random();
            public DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func
            public void AddStation(Station station) //adds station to list
            {
                DataSource.StationList.Add(station);
            }
            public void AddDrone(Drone drone) //adds drone to list
            {
                DataSource.DroneList.Add(drone);
            }
            public void AddCustomer(Customer customer) //adds customer to list
            {
                DataSource.CustomerList.Add(customer);
            }
            public void AddParcel(Parcel parcel) //adds parcel to list
            {
                DataSource.ParcelList.Add(parcel);
            }
            public List<Station> printStationsList() //prints list of stations 
            {
                return DataSource.StationList; //returns station list
            }
            public List<Drone> printDronesList() //prints list of drone
            {
                return DataSource.DroneList; //returns drone list
            }
            public List<Customer> printCustomersList() //prints customer list
            {
                return DataSource.CustomerList; //returns customer list

            }
            public List<Parcel> printParcelsList() //prints parcel list
            {
                return DataSource.ParcelList;  //returns parcel list
            }
            
            
            public string matchUpParcel(Parcel parcel) //matches up package with drone
            {
                string complete = "Your request was completed successfully";
                Drone drone = new Drone();
                drone = (DataSource.DroneList.Find(temp => temp.Status == DroneStatuses.available && temp.MaxWeight >= parcel.Weight)); //finds avail drone that can contain weight of pckg
                DataSource.DroneList.RemoveAll(temp => temp.DroneId == drone.DroneId); //removes the availabe drone
                if (drone.DroneId != 0 && parcel.ParcelId != 0) //if found drone updates info to match pckg
                {
                    parcel.DroneId = drone.DroneId;
                    parcel.Scheduled = DateTime.Now;
                    drone.Status = DroneStatuses.delivery;
                    DataSource.DroneList.Add(drone);
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcel.ParcelId);
                    DataSource.ParcelList.Add(parcel); //adds updates parcel back into list of parcel
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            public string pickUpParcel(Customer customer,Parcel parcel) //matches up packg with sender of pckg
            {
                string complete = "Your request was completed successfully";
                parcel.SenderId = customer.CustomerId;
                parcel.PickedUp = DateTime.Now;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId == parcel.DroneId)); //make new droen equal the one matched up with parcel
                if (drone.DroneId != 0 && parcel.ParcelId != 0) //if such a drone exists updaed drone
                {
                    drone.Status = DroneStatuses.delivery;
                    DataSource.DroneList.RemoveAll(temp=> temp.DroneId == drone.DroneId);
                    DataSource.DroneList.Add(drone);
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcel.ParcelId);
                    DataSource.ParcelList.Add(parcel);
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            public string deliverParcel(Customer customer, Parcel parcel, int priorityLevel) //matches up parcel with buyer
            {
                string complete = "Your request was completed successfully";
                parcel.TargetId = customer.CustomerId;
                parcel.Delivered = DateTime.Now;
                parcel.Priority = (Priorities)priorityLevel;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId== parcel.DroneId));
                if(drone.DroneId!=0) //ensures drone exists and updates its status
                { 
                    drone.Status = DroneStatuses.available;
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == parcel.DroneId);
                    DataSource.DroneList.Add(drone);
                }
                else
                    return "Your request could not be completed";
                if (parcel.ParcelId!= 0) //ensures parcel exists and updates its status
                {
                    DataSource.ParcelList.RemoveAll(temp=> temp.ParcelId == parcel.ParcelId);
                    DataSource.ParcelList.Add(parcel);
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            public string chargeDrone(Drone drone,int stationNum) //charges drone
            {
                string complete = "Your request was completed successfully";
                drone.Status = DroneStatuses.maintenance;
                Station station = DataSource.StationList.Find(temp => (temp.Name == stationNum)); //builds station
                if (station.StationId != 0 && drone.DroneId != 0) //if station exists updates it
                {
                    DataSource.StationList.Remove(station); //removes station
                    DataSource.DroneList.Remove(drone); //removes station
                    station.ChargeSlots--;
                    DroneCharge charge = new DroneCharge();
                    charge.DroneId = drone.DroneId;
                    charge.StationId = station.StationId;
                    DataSource.DroneChargeList.Add(charge); //adds new charge to chargedroen list
                    DataSource.StationList.Add(station); //adds updated station
                    DataSource.DroneList.Add(drone); //adds updated drone
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            public string releaseDrone(DroneCharge charge) //releases drone from charge
            {
                string complete = "Your request was completed successfully";
                Drone drone = DataSource.DroneList.Find(temp => (temp.DroneId == charge.DroneId)); //pulls correct drone
                if (drone.DroneId != 0) //if droen exists updates  
                {
                    drone.Status = DroneStatuses.available;
                    drone.Battery = 100;
                    DataSource.DroneList.RemoveAll(m => (m.DroneId == charge.DroneId));
                    DataSource.DroneList.Add(drone);
                }
                else
                    return "Your request could not be completed";
                Station station = DataSource.StationList.Find(s => (s.StationId == charge.StationId)); //pulls correct station
                if (station.StationId != 0) //if station exists updates
                {
                    station.ChargeSlots++;
                    DataSource.StationList.RemoveAll(temp => (temp.StationId == charge.StationId));
                    DataSource.StationList.Add(station);
                }
                else
                    return "Your request could not be completed";
                DataSource.DroneChargeList.Remove(charge);
                return complete;
            }
            public string PrintStation(int stationId) //prints a station
            {
                if (findStation(stationId).StationId != 0)
                    return findStation(stationId).ToString();
                return "Your request could not be completed";
                return "Your request could not be completed"; 
            }
            public string PrintDrone(int droneId) //prints a drone
            {
                if (findDrone(droneId).DroneId != 0)
                    return findDrone(droneId).ToString();
                return "Your request could not be completed";
            }
            public string PrintCustomer(int customerId) //prints a customer
            {
                if (findCustomer(customerId).CustomerId != 0)
                    return findCustomer(customerId).ToString();
                return "Your request could not be completed";
            }
            public string PrintParcel(int parcelId) //prints a parcel
            {
                if (findParcel(parcelId).ParcelId != 0)
                    return findParcel(parcelId).ToString();
                return "Your request could not be completed";
            }
            public Parcel findParcel(int parcelId) //finds a parcel using its id
            {
                Parcel notFound = new Parcel();
                for (int i = 0; i < DataSource.ParcelList.Count(); i++) //goes over parcel list
                {
                    if (DataSource.ParcelList[i].ParcelId == parcelId) //if id matches
                    {
                        return (DataSource.ParcelList[i]);
                    }
                }
                return notFound;
            }
            public Customer findCustomer(int customerId) //finds a customer using its id
            {
                Customer notFound = new Customer();
                for (int i = 0; i < DataSource.CustomerList.Count(); i++) //goes over customer list
                {
                    if (DataSource.CustomerList[i].CustomerId == customerId) //if id matches
                    {
                        return (DataSource.CustomerList[i]); 
                    }
                }
                return notFound;
            }
            public Drone findDrone(int droneId) //finds a drone using its id
            {
                Drone notFound = new Drone();
                for (int i = 0; i < DataSource.DroneList.Count(); i++) //goes over drone list
                {
                    if (DataSource.DroneList[i].DroneId == droneId) //if id matches
                    {
                        return (DataSource.DroneList[i]);
                    }
                }
                return notFound;
            }
            public Station findStation(int stationId) //finds a station using its id
            {
                Station notFound = new Station();
                for (int i = 0; i < DataSource.StationList.Count(); i++) //goes over station list
                {
                    if (DataSource.StationList[i].StationId == stationId) //if id matches
                    {
                        return (DataSource.StationList[i]);
                    }
                }
                return notFound;
            }
            public DroneCharge findDroneCharge(int droneChargeId) //finds a drone charge using its id
            {
                DroneCharge notFound = new DroneCharge();
                for (int i = 0; i < DataSource.DroneChargeList.Count(); i++) //goes over dronecharge list
                {
                    if (DataSource.DroneChargeList[i].DroneId == droneChargeId) //if ifd match
                    {
                        return (DataSource.DroneChargeList[i]);
                    }
                }
                return notFound;
            }
            public int getParcelId() //returns parcel id
            {
                return DataSource.config.assignParcelId.Next(100000000+DataSource.ParcelList.Count(), 999999999); //genrates parcel id
            }
            public double distance(double lattitude1,double longitute1, double lattitude2, double longitute2) //calculates distance between coordinates for bonus
            {
                //ditance between 2 points is: sqrt of- pow2(x1-x2) + pow2(y1-y2)
                lattitude1 = lattitude1 - lattitude2; //(x1-x2)
                longitute1 = longitute1 - longitute2; //(y1 - y2)
                lattitude1 = Math.Pow(lattitude1, 2); //pow2(update)
                longitute1 = Math.Pow(longitute1, 2); //pow2(update)
                lattitude1 = lattitude1 + longitute1; //update + update
                return Math.Sqrt(lattitude1); //sqrt of update
            }
        }
    }
}
