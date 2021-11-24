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
        public class DalObject : IDal
        {
            public static Random r = new Random();
            public DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func
            #region AddStation
            public void AddStation(Station station) //adds station to list
            {
                DataSource.StationList.Add(station);
            }
            #endregion
            #region AddDrone
            public void AddDrone(Drone drone) //adds drone to list
            {
                DataSource.DroneList.Add(drone);
            }
            #endregion
            #region AddCustomer
            public void AddCustomer(Customer customer) //adds customer to list
            {
                DataSource.CustomerList.Add(customer);
            }
            #endregion
            #region AddParcel
            public void AddParcel(Parcel parcel) //adds parcel to list
            {
                DataSource.ParcelList.Add(parcel);
            }
            #endregion
            #region printStationsList
            public List<Station> printStationsList() //prints list of stations 
            {
                return DataSource.StationList; //returns station list
            }
            #endregion
            #region printDronesList
            public List<Drone> printDronesList() //prints list of drone
            {
                return DataSource.DroneList; //returns drone list
            }
            #endregion
            #region printCustomersList
            public List<Customer> printCustomersList() //prints customer list
            {
                return DataSource.CustomerList; //returns customer list
            }
            #endregion
            #region printParcelsList
            public List<Parcel> printParcelsList() //prints parcel list
            {
                return DataSource.ParcelList;  //returns parcel list
            }
            #endregion
            #region matchUpParcel
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
            #endregion
            #region pickUpParcel
            public string pickUpParcel(Customer customer, Parcel parcel) //matches up packg with sender of pckg
            {
                string complete = "Your request was completed successfully";
                parcel.SenderId = customer.CustomerId;
                parcel.PickedUp = DateTime.Now;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId == parcel.DroneId)); //make new droen equal the one matched up with parcel
                if (drone.DroneId != 0 && parcel.ParcelId != 0) //if such a drone exists updaed drone
                {
                    drone.Status = DroneStatuses.delivery;
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == drone.DroneId);
                    DataSource.DroneList.Add(drone);
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcel.ParcelId);
                    DataSource.ParcelList.Add(parcel);
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            #endregion
            #region deliverParcel
            public string deliverParcel(Customer customer, Parcel parcel, int priorityLevel) //matches up parcel with buyer
            {
                if (customer.CustomerId == 0)
                    return "Your request could not be completed";
                string complete = "Your request was completed successfully";
                parcel.TargetId = customer.CustomerId;
                parcel.Delivered = DateTime.Now;
                parcel.Priority = (Priorities)priorityLevel;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId == parcel.DroneId));
                if (drone.DroneId != 0) //ensures drone exists and updates its status
                {
                    drone.Status = DroneStatuses.available;
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == parcel.DroneId);
                    DataSource.DroneList.Add(drone);
                }
                else
                    return "Your request could not be completed";
                if (parcel.ParcelId != 0) //ensures parcel exists and updates its status
                {
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcel.ParcelId);
                    DataSource.ParcelList.Add(parcel);
                    return complete;
                }
                else
                    return "Your request could not be completed";
            }
            #endregion
            #region chargeDrone
            public string chargeDrone(Drone drone, int stationNum) //charges drone
            {
                string complete = "Your request was completed successfully";
                drone.Status = DroneStatuses.maintenance;
                drone.Battery = 0;
                Station station = DataSource.StationList.Find(temp => (temp.Name == stationNum)); //builds station
                if (station.StationId != 0 && drone.DroneId != 0) //if station exists updates it
                {
                    DataSource.StationList.Remove(station); //removes station
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == drone.DroneId); //removes station
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
            #endregion
            #region releaseDrone
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
                DataSource.DroneChargeList.RemoveAll(temp => temp.DroneId == charge.DroneId);
                return complete;
            }
            #endregion
            #region PrintStation
            public string PrintStation(int stationId) //prints a station
            {
                if (findStation(stationId).StationId != 0)
                    return findStation(stationId).ToString();
                return "Your request could not be completed";
            }
            #endregion
            #region PrintDrone
            public string PrintDrone(int droneId) //prints a drone
            {
                if (findDrone(droneId).DroneId != 0)
                    return findDrone(droneId).ToString();
                return "Your request could not be completed";
            }
            #endregion
            #region PrintCustomer
            public string PrintCustomer(int customerId) //prints a customer
            {
                if (findCustomer(customerId).CustomerId != 0)
                    return findCustomer(customerId).ToString();
                return "Your request could not be completed";
            }
            #endregion
            #region PrintParcel
            public string PrintParcel(int parcelId) //prints a parcel
            {
                if (findParcel(parcelId).ParcelId != 0)
                    return findParcel(parcelId).ToString();
                return "Your request could not be completed";
            }
            #endregion
            #region findParcel
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
            #endregion
            #region findCustomer
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
            #endregion
            #region findDrone
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
            #endregion
            #region findStation
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
            #endregion
            #region DroneCharge
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
            #endregion
            #region getParcelId
            public int getParcelId() //returns parcel id
            {
                return DataSource.config.assignParcelId++; //genrates parcel id
            }
            #endregion
            #region distance
            public double distance(double lattitude1, double longitute1, double lattitude2, double longitute2) //calculates distance between coordinates for bonus
            {
                //ditance between 2 points is: sqrt of- pow2(x1-x2) + pow2(y1-y2)
                lattitude1 = lattitude1 - lattitude2; //(x1-x2)
                longitute1 = longitute1 - longitute2; //(y1 - y2)
                lattitude1 = Math.Pow((lattitude1), 2); //pow2(update)
                longitute1 = Math.Pow(longitute1, 2); //pow2(update)
                lattitude1 = lattitude1 + longitute1; //update + update
                return Math.Sqrt(lattitude1); //sqrt of update
            }
            #endregion

        }
    }
}
