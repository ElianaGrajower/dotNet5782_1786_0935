using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


//dont think i have any of the delelyes
//basically just recheck my crud

namespace DAL
{
    namespace DalObject     
    {
        public class DalObject 
        {
            public static Random r = new Random();
            public DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func
           
            
           
            #region  GetStation
            public Station GetStation(int stationId)
            {
                try
                {
                    return findStation(stationId);
                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }
            }
            #endregion
            #region GetDrone
            public Drone GetDrone(int droneId)
            {
                try
                {
                    return findDrone(droneId);
                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }
            }
            #endregion
            #region GetCustomer
            public Customer GetCustomer(int customerId)
            {
                try
                {
                    return findCustomer(customerId);
                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }
            }
            #endregion
            #region GetParcel
            public Parcel GetParcel(int parcelId)
            {
                try
                {
                    return findParcel(parcelId);
                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }
            }
            #endregion
            public void UpdateDrone(Drone droneToUpdate)
            {

                DataSource.DroneList.RemoveAll(x => x.DroneId == droneToUpdate.DroneId);
                DataSource.DroneList.Add(droneToUpdate);


            }
            public void UpdateStation(Station stationToUpdate)
            {

                DataSource.StationList.RemoveAll(x => x.StationId == stationToUpdate.StationId);
                DataSource.StationList.Add(stationToUpdate);


            }
            //still need to finsh customer func and find out id=f staton can be aded afr=ter he fact
            public void UpdateCustomer(Customer customerToUpdate)
            {

                DataSource.CustomerList.RemoveAll(x => x.CustomerId == customerToUpdate.CustomerId);
                DataSource.CustomerList.Add(customerToUpdate);


            }
            public void UpdateParcel(Parcel parcelToUpdate)
            {

                DataSource.ParcelList.RemoveAll(x => x.ParcelId == parcelToUpdate.ParcelId);
                DataSource.ParcelList.Add(parcelToUpdate);


            }
            #region AddStation
            public void AddStation(Station stationToAdd) //adds station to list
            {
                if (DataSource.StationList.Count(x => x.StationId == stationToAdd.StationId) != 0)
                    throw new AlreadyExistException("The station already exist in the system");
                DataSource.StationList.Add(stationToAdd);
            }
            #endregion
            #region  AddDrone
            public void AddDrone(Drone droneToAdd) //adds drone to list
            {
                if (DataSource.DroneList.Count(x => x.DroneId == droneToAdd.DroneId) != 0)
                    throw new AlreadyExistException("The drone already exist in the system");
                DataSource.DroneList.Add(droneToAdd);


            }
            public void AddDroneCharge(DroneCharge droneChargeToAdd) //adds drone to list
            {
                if (DataSource.DroneChargeList.Count(x => x.DroneId == droneChargeToAdd.DroneId) != 0)
                    throw new AlreadyExistException("The drone is already being charged at a station");
                DataSource.DroneChargeList.Add(droneChargeToAdd);


            }

            #endregion
            #region AddCustomer
            public void AddCustomer(Customer customerToAdd) //adds customer to list
            {

                if (DataSource.CustomerList.Count(x => x.CustomerId == customerToAdd.CustomerId) != 0)
                    throw new AlreadyExistException("The customer already exist in the system");
                DataSource.CustomerList.Add(customerToAdd);
            }
            #endregion
            #region AddParcel
            public void AddParcel(Parcel parcelToAdd) //adds parcel to list
            {
                if (DataSource.ParcelList.Count(x => x.ParcelId == parcelToAdd.ParcelId) != 0)
                    throw new AlreadyExistException("The parcel already exist in the system");
                DataSource.ParcelList.Add(parcelToAdd);
            }
            #endregion
            public void DeleteDrone(int id)
            {
                try
                {
                    DataSource.DroneList.Remove(findDrone(id));

                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }

            }
            public void DeleteCustomer(int id)
            {

                try
                {
                    DataSource.CustomerList.Remove(findCustomer(id));

                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }

            }
            public void DeleteParcel(int id)
            {

                try
                {
                    DataSource.ParcelList.Remove(findParcel(id));

                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }

            }
            public void DeleteStation(int id)
            {
                try
                {
                    DataSource.StationList.Remove(findStation(id));
                    
                }
                catch(DoesntExistException exc)
                {
                    throw exc;
                }
               

            }
            public void DeleteDroneCharge(int droneId,int stationId)
            {

                try
                {
                    DataSource.DroneChargeList.Remove(findDroneCharge(droneId,stationId));

                }
                catch (DoesntExistException exc)
                {
                    throw exc;
                }

            }

            #region matchUpParcel
            public string matchUpParcel(Parcel parcelToUpdate) //matches up package with drone
            {
                Parcel myParcel = DataSource.ParcelList.Find(x => x.ParcelId == parcelToUpdate.ParcelId);
                if (myParcel.ParcelId == 0)
                    throw new DoesntExistException("This parcel doesn't exist in the system");
                string complete = "Your request was completed successfully";
                Drone drone = new Drone();
                drone = (DataSource.DroneList.Find(temp => /*temp.Status == DroneStatuses.available &&*/ temp.MaxWeight >= parcelToUpdate.Weight)); //finds avail drone that can contain weight of pckg
                DataSource.DroneList.RemoveAll(temp => temp.DroneId == drone.DroneId); //removes the availabe drone
                if (drone.DroneId != 0 && parcelToUpdate.ParcelId != 0) //if found drone updates info to match pckg
                {
                    parcelToUpdate.DroneId = drone.DroneId;
                    parcelToUpdate.Scheduled = DateTime.Now;
                    // drone.Status = DroneStatuses.delivery;
                    DataSource.DroneList.Add(drone);
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcelToUpdate.ParcelId);
                    DataSource.ParcelList.Add(parcelToUpdate); //adds updates parcel back into list of parcel
                    
                }
                return complete;


            }
            #endregion
            #region pickUpParcel
            public string pickUpParcel(Customer customerToUpdate, Parcel parcelToUpdate) //matches up packg with sender of pckg
            {
                Parcel myParcel = DataSource.ParcelList.Find(x => x.ParcelId == parcelToUpdate.ParcelId);
                Customer myCustomer = DataSource.CustomerList.Find(x => x.CustomerId == customerToUpdate.CustomerId);
                if (myParcel.ParcelId == 0)
                    throw new DoesntExistException("This parcel doesn't exist in the system");
                if (myCustomer.CustomerId == 0)
                    throw new DoesntExistException("This customer doesn't exist in the system");
                string complete = "Your request was completed successfully";
                parcelToUpdate.SenderId = customerToUpdate.CustomerId;
                parcelToUpdate.PickedUp = DateTime.Now;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId == parcelToUpdate.DroneId)); //make new droe equal the one matched up with parcel
                if (drone.DroneId != 0 && parcelToUpdate.ParcelId != 0) //if such a drone exists updaed drone
                {
                    //  drone.Status = DroneStatuses.delivery;
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == drone.DroneId);
                    DataSource.DroneList.Add(drone);
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcelToUpdate.ParcelId);
                    DataSource.ParcelList.Add(parcelToUpdate);

                }
                return complete;

            }
            #endregion
            #region deliverParcel

            public string deliverParcel(Customer customerToUpdate, Parcel parcelToUpdate, int priorityLevel) //matches up parcel with buyer
            {

                Parcel myParcel = DataSource.ParcelList.Find(x => x.ParcelId == parcelToUpdate.ParcelId);
                Customer myCustomer = DataSource.CustomerList.Find(x => x.CustomerId == customerToUpdate.CustomerId);
                if (myParcel.ParcelId == 0 || myCustomer.CustomerId == 0)
                    throw new DoesntExistException("This parcel doesn't exist in the system");

                string complete = "Your request was completed successfully";
                parcelToUpdate.TargetId = customerToUpdate.CustomerId;
                parcelToUpdate.Delivered = DateTime.Now;
                parcelToUpdate.Priority = (Priorities)priorityLevel;
                Drone drone = new Drone(); //builds new drone
                drone = (DataSource.DroneList.Find(temp => temp.DroneId == parcelToUpdate.DroneId));
                if (drone.DroneId != 0) //ensures drone exists and updates its status
                {
                    //   drone.Status = DroneStatuses.available;
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == parcelToUpdate.DroneId);
                    DataSource.DroneList.Add(drone);
                }

                if (parcelToUpdate.ParcelId != 0) //ensures parcel exists and updates its status
                {
                    DataSource.ParcelList.RemoveAll(temp => temp.ParcelId == parcelToUpdate.ParcelId);
                    DataSource.ParcelList.Add(parcelToUpdate);

                }
                return complete;

            }
            #endregion
            #region chargeDrone
            public string chargeDrone(Drone droneToUpdate, string stationNum) //charges drone
            {

                Drone myDrone = DataSource.DroneList.Find(x => x.DroneId == droneToUpdate.DroneId);
                if (myDrone.DroneId == 0)
                    throw new DoesntExistException("This drone doesn't exist in the system");
                string complete = "Your request was completed successfully";
                //  drone.Status = DroneStatuses.maintenance;
                //  drone.Battery = 0;
                Station station = DataSource.StationList.Find(temp => (temp.Name == stationNum)); //builds station
                if (station.StationId != 0 && droneToUpdate.DroneId != 0) //if station exists updates it
                {
                    DataSource.StationList.Remove(station); //removes station
                    DataSource.DroneList.RemoveAll(temp => temp.DroneId == droneToUpdate.DroneId); //removes station
                    station.ChargeSlots--;
                    DroneCharge charge = new DroneCharge();
                    charge.DroneId = droneToUpdate.DroneId;
                    charge.StationId = station.StationId;
                    DataSource.DroneChargeList.Add(charge); //adds new charge to chargedroen list
                    DataSource.StationList.Add(station); //adds updated station
                    DataSource.DroneList.Add(droneToUpdate); //adds updated drone

                }
                return complete;

            }
            #endregion
            #region releaseDrone
            public string releaseDrone(DroneCharge charge) //releases drone from charge
            {
                Drone myDrone = DataSource.DroneList.Find(temp => (temp.DroneId == charge.DroneId)); //pulls correct drone
                if (myDrone.DroneId == 0)
                    throw new DoesntExistException("This drone doesn't exist in the system");
                string complete = "Your request was completed successfully";

                if (myDrone.DroneId != 0) //if droen exists updates  
                {
                    // drone.Status = DroneStatuses.available;
                    //drone.Battery = 100;
                    DataSource.DroneList.RemoveAll(m => (m.DroneId == charge.DroneId));
                    DataSource.DroneList.Add(myDrone);
                }

                Station myStation = DataSource.StationList.Find(s => (s.StationId == charge.StationId)); //pulls correct station
                if (myStation.StationId == 0)
                    throw new DoesntExistException("This drone doesn't exist in the system");
                if (myStation.StationId != 0) //if station exists updates
                {
                    myStation.ChargeSlots++;
                    DataSource.StationList.RemoveAll(temp => (temp.StationId == charge.StationId));
                    DataSource.StationList.Add(myStation);
                }

                DataSource.DroneChargeList.RemoveAll(temp => temp.DroneId == charge.DroneId);
                return complete;
            }
            #endregion
            #region PrintStation
            public string PrintStation(int stationId) //prints a station
            {
                if (findStation(stationId).StationId != 0)
                    return findStation(stationId).ToString();
                throw new DoesntExistException("The station doesn't exist in system");
            }
            #endregion
            #region PrintDrone
            public string PrintDrone(int droneId) //prints a drone
            {
                if (findDrone(droneId).DroneId != 0)
                    return findDrone(droneId).ToString();
                throw new DoesntExistException("The drone doesn't exist in system");
            }
            #endregion
            #region PrintCustomer
            public string PrintCustomer(int customerId) //prints a customer
            {
                if (findCustomer(customerId).CustomerId != 0)
                    return findCustomer(customerId).ToString();
                throw new DoesntExistException("The customer doesn't exist in system");
            }
            #endregion
            #region PrintParcel
            public string PrintParcel(int parcelId) //prints a parcel
            {
                if (findParcel(parcelId).ParcelId != 0)
                    return findParcel(parcelId).ToString();
                throw new DoesntExistException("The parcel doesn't exist in system");
            }
            #endregion
            #region findParcel
            public Parcel findParcel(int parcelId) //finds a parcel using its id
            {

                for (int i = 0; i < DataSource.ParcelList.Count(); i++) //goes over parcel list
                {
                    if (DataSource.ParcelList[i].ParcelId == parcelId) //if id matches
                    {
                        return (DataSource.ParcelList[i]);
                    }
                }
                throw new DoesntExistException("The parcel doesn't exist in system");
            }
            #endregion
            #region findCustomer
            public Customer findCustomer(int customerId) //finds a customer using its id
            {

                for (int i = 0; i < DataSource.CustomerList.Count(); i++) //goes over customer list
                {
                    if (DataSource.CustomerList[i].CustomerId == customerId) //if id matches
                    {
                        return (DataSource.CustomerList[i]);
                    }
                }
                throw new DoesntExistException("The customer doesn't exist in system");
            }
            #endregion
            public DroneCharge findDroneCharge(int droneId,int stationId) 
            {

                for (int i = 0; i < DataSource.DroneChargeList.Count(); i++) 
                {
                    if (DataSource.DroneChargeList[i].DroneId == droneId && DataSource.DroneChargeList[i].StationId == stationId) //if id matches
                    {
                        return (DataSource.DroneChargeList[i]);
                    }
                }
                throw new DoesntExistException("The drone or station doesn't exist in system");
            }
            #region findDrone
            public Drone findDrone(int droneId) //finds a drone using its id
            {

                for (int i = 0; i < DataSource.DroneList.Count(); i++) //goes over drone list
                {
                    if (DataSource.DroneList[i].DroneId == droneId) //if id matches
                    {
                        return (DataSource.DroneList[i]);
                    }
                }
                throw new DoesntExistException("The drone doesn't exist in system");
            }
            #endregion
            #region findStation
            public Station findStation(int stationId) //finds a station using its id
            {

                for (int i = 0; i < DataSource.StationList.Count(); i++) //goes over station list
                {
                    if (DataSource.StationList[i].StationId == stationId) //if id matches
                    {
                        return (DataSource.StationList[i]);
                    }
                }
                throw new DoesntExistException("The station doesn't exist in system");

            }
            #endregion
            #region findDroneCharge
            public DroneCharge findDroneCharge(int droneChargeId) //finds a drone charge using its id
            {

                for (int i = 0; i < DataSource.DroneChargeList.Count(); i++) //goes over dronecharge list
                {
                    if (DataSource.DroneChargeList[i].DroneId == droneChargeId) //if ifd match
                    {
                        return (DataSource.DroneChargeList[i]);
                    }
                }
                throw new DoesntExistException("The dronecharge doesn't exist in system");
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
            #region ChargeCapacity
            public double[] ChargeCapacity()
            {

                double[] arr = new double[] { DataSource.config.available, DataSource.config.lightLoad, DataSource.config.mediumLoad, DataSource.config.heavyLoad, DataSource.config.chargeSpeed };
                return arr;

            }
            #endregion
            #region printStationsList
            public IEnumerable<Station> printStationsList() //prints list of stations 
            {

                foreach (Station item in DataSource.StationList)
                {
                    yield return item;
                }


            }
            #endregion
            #region printDronesList
            public IEnumerable<Drone> printDronesList() //prints list of drone
            {
                foreach (Drone item in DataSource.DroneList)
                {
                    yield return item;
                }
            }
            #endregion
            #region printCustomersList
            public IEnumerable<Customer> printCustomersList() //prints customer list
            {
                foreach (Customer item in DataSource.CustomerList)
                {
                    yield return item;
                }

            }
            #endregion
            #region printParcelsList
            public IEnumerable<Parcel> printParcelsList() //prints parcel list
            {
                foreach (Parcel item in DataSource.ParcelList)
                {
                    yield return item;
                }
            }
            #endregion
            //#region GetPartialParcelsList

            //public IEnumerable<Parcel> GetPartialParcelsList(Predicate<> predicate) //prints parcel list
            //{
            //    foreach (Parcel item in DataSource.ParcelList)
            //    {
            //        yield return item;
            //    }
            //}
            //#endregion
            #region printDroneChargeList
            public IEnumerable<DroneCharge> printDroneChargeList() //prints DroneCharge list
            {
                foreach (DroneCharge item in DataSource.DroneChargeList)
                {
                    yield return item;
                }
            }
            #endregion
            public void attribute(int dID, int pID)//the function attribute parcel to drone
            {
                Drone tmpD = GetDrone(dID);
                Parcel tmpP = GetParcel(pID);
                DataSource.ParcelList.RemoveAll(m => m.ParcelId == tmpP.ParcelId);   //removing all the data from the place in the list the equal to tmpP id
                tmpP.DroneId = tmpD.DroneId;        //attribute drones id to parcel 
                tmpP.Scheduled = DateTime.Now; //changing the time to be right now
                DataSource.ParcelList.Add(tmpP); //adding to the parcel list tmpP
            }



        }
    }

}

