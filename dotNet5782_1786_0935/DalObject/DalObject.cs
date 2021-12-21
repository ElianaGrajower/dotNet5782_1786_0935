//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using System.Threading.Tasks;
//using DalApi;
//using DAL;


//namespace Dal
//{

//    sealed class DalObject : DalApi.IDal
//    {

//        static readonly IDal instance = new DalObject();
//        public static IDal Instance { get => instance; }
//        DalObject() { }
//        public static Random r = new Random();
//         DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func



//        #region  getStation
//        public Station getStation(int stationId)
//        {
//            try
//            {
//                return findStation(stationId);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getDrone
//        public Drone getDrone(int droneId)
//        {
//            try
//            {
//                return findDrone(droneId);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getCustomer
//        public Customer getCustomer(int customerId)
//        {
//            try
//            {
//                return findCustomer(customerId);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getParcel
//        public Parcel getParcel(int parcelId)
//        {
//            try
//            {
//                return findParcel(parcelId);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }
//        }
//        #endregion
//        public void UpdateDrone(Drone droneToUpdate)
//        {

//            DataSource.DroneList.RemoveAll(x => x.droneId == droneToUpdate.droneId);
//            DataSource.DroneList.Add(droneToUpdate);


//        }
//        public void UpdateStation(Station stationToUpdate)
//        {

//            DataSource.StationList.RemoveAll(x => x.stationId == stationToUpdate.stationId);
//            DataSource.StationList.Add(stationToUpdate);


//        }
//        //still need to finsh customer func and find out id=f staton can be aded afr=ter he fact
//        public void UpdateCustomer(Customer customerToUpdate)
//        {

//            DataSource.CustomerList.RemoveAll(x => x.customerId == customerToUpdate.customerId);
//            DataSource.CustomerList.Add(customerToUpdate);


//        }
//        public void UpdateParcel(Parcel parcelToUpdate)
//        {

//            DataSource.ParcelList.RemoveAll(x => x.parcelId == parcelToUpdate.parcelId);
//            DataSource.ParcelList.Add(parcelToUpdate);


//        }
//        #region AddStation
//        public void AddStation(Station stationToAdd) //adds station to list
//        {
//            if (DataSource.StationList.Count(x => x.stationId == stationToAdd.stationId) != 0)
//                throw new AlreadyExistException("The station already exist in the system");
//            DataSource.StationList.Add(stationToAdd);
//        }
//        #endregion
//        #region  AddDrone
//        public void AddDrone(Drone droneToAdd) //adds drone to list
//        {
//            if (DataSource.DroneList.Count(x => x.droneId == droneToAdd.droneId) != 0)
//                throw new AlreadyExistException("The drone already exist in the system");
//            DataSource.DroneList.Add(droneToAdd);


//        }
//        public void AddDroneCharge(DroneCharge droneChargeToAdd) //adds drone to list
//        {
//            if (DataSource.DroneChargeList.Count(x => x.droneId == droneChargeToAdd.droneId) != 0)
//                throw new AlreadyExistException("The drone is already being charged at a station");
//            DataSource.DroneChargeList.Add(droneChargeToAdd);


//        }

//        #endregion
//        #region AddCustomer
//        public void AddCustomer(Customer customerToAdd) //adds customer to list
//        {

//            if (DataSource.CustomerList.Count(x => x.customerId == customerToAdd.customerId) != 0)
//                throw new AlreadyExistException("The customer already exist in the system");
//            DataSource.CustomerList.Add(customerToAdd);
//        }
//        #endregion
//        #region AddParcel
//        public void AddParcel(Parcel parcelToAdd) //adds parcel to list
//        {
//            if (DataSource.ParcelList.Count(x => x.parcelId == parcelToAdd.parcelId) != 0)
//                throw new AlreadyExistException("The parcel already exist in the system");
//            DataSource.ParcelList.Add(parcelToAdd);
//        }
//        #endregion
//        public void deleteDrone(int id)
//        {
//            try
//            {
//                DataSource.DroneList.Remove(findDrone(id));

//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }

//        }
//        public void deleteCustomer(int id)
//        {

//            try
//            {
//                DataSource.CustomerList.Remove(findCustomer(id));

//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }

//        }
//        public void deleteParcel(int id)
//        {

//            try
//            {
//                DataSource.ParcelList.Remove(findParcel(id));

//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }

//        }
//        public void deleteStation(int id)
//        {
//            try
//            {
//                DataSource.StationList.Remove(findStation(id));

//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }


//        }
//        public void deleteDroneCharge(int droneId, int stationId)
//        {

//            try
//            {
//                DataSource.DroneChargeList.Remove(findDroneCharge(droneId, stationId));

//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }

//        }

//        #region matchUpParcel
//        public string matchUpParcel(Parcel parcelToUpdate) //matches up package with drone
//        {
//            Parcel myParcel = DataSource.ParcelList.Find(x => x.parcelId == parcelToUpdate.parcelId);
//            if (myParcel.parcelId == 0)
//                throw new DoesntExistException("This parcel doesn't exist in the system");
//            string complete = "Your request was completed successfully";
//            Drone drone = new Drone();
//            drone = (DataSource.DroneList.Find(temp => /*temp.Status == DroneStatuses.available &&*/ temp.maxWeight >= parcelToUpdate.weight)); //finds avail drone that can contain weight of pckg
//            DataSource.DroneList.RemoveAll(temp => temp.droneId == drone.droneId); //removes the availabe drone
//            if (drone.droneId != 0 && parcelToUpdate.parcelId != 0) //if found drone updates info to match pckg
//            {
//                parcelToUpdate.droneId = drone.droneId;
//                parcelToUpdate.scheduled = DateTime.Now;
//                // drone.Status = DroneStatuses.delivery;
//                DataSource.DroneList.Add(drone);
//                DataSource.ParcelList.RemoveAll(temp => temp.parcelId == parcelToUpdate.parcelId);
//                DataSource.ParcelList.Add(parcelToUpdate); //adds updates parcel back into list of parcel

//            }
//            return complete;


//        }
//        #endregion
//        #region pickUpParcel
//        public string pickUpParcel(Customer customerToUpdate, Parcel parcelToUpdate) //matches up packg with sender of pckg
//        {
//            Parcel myParcel = DataSource.ParcelList.Find(x => x.parcelId == parcelToUpdate.parcelId);
//            Customer myCustomer = DataSource.CustomerList.Find(x => x.customerId == customerToUpdate.customerId);
//            if (myParcel.parcelId == 0)
//                throw new DoesntExistException("This parcel doesn't exist in the system");
//            if (myCustomer.customerId == 0)
//                throw new DoesntExistException("This customer doesn't exist in the system");
//            string complete = "Your request was completed successfully";
//            parcelToUpdate.senderId = customerToUpdate.customerId;
//            parcelToUpdate.pickedUp = DateTime.Now;
//            Drone drone = new Drone(); //builds new drone
//            drone = (DataSource.DroneList.Find(temp => temp.droneId == parcelToUpdate.droneId)); //make new droe equal the one matched up with parcel
//            if (drone.droneId != 0 && parcelToUpdate.parcelId != 0) //if such a drone exists updaed drone
//            {
//                //  drone.Status = DroneStatuses.delivery;
//                DataSource.DroneList.RemoveAll(temp => temp.droneId == drone.droneId);
//                DataSource.DroneList.Add(drone);
//                DataSource.ParcelList.RemoveAll(temp => temp.parcelId == parcelToUpdate.parcelId);
//                DataSource.ParcelList.Add(parcelToUpdate);

//            }
//            return complete;

//        }
//        #endregion
//        #region deliverParcel

//        public string deliverParcel(Customer customerToUpdate, Parcel parcelToUpdate, int priorityLevel) //matches up parcel with buyer
//        {

//            Parcel myParcel = DataSource.ParcelList.Find(x => x.parcelId == parcelToUpdate.parcelId);
//            Customer myCustomer = DataSource.CustomerList.Find(x => x.customerId == customerToUpdate.customerId);
//            if (myParcel.parcelId == 0 || myCustomer.customerId == 0)
//                throw new DoesntExistException("This parcel doesn't exist in the system");

//            string complete = "Your request was completed successfully";
//            parcelToUpdate.targetId = customerToUpdate.customerId;
//            parcelToUpdate.delivered = DateTime.Now;
//            parcelToUpdate.priority = (Priorities)priorityLevel;
//            Drone drone = new Drone(); //builds new drone
//            drone = (DataSource.DroneList.Find(temp => temp.droneId == parcelToUpdate.droneId));
//            if (drone.droneId != 0) //ensures drone exists and updates its status
//            {
//                //   drone.Status = DroneStatuses.available;
//                DataSource.DroneList.RemoveAll(temp => temp.droneId == parcelToUpdate.droneId);
//                DataSource.DroneList.Add(drone);
//            }

//            if (parcelToUpdate.parcelId != 0) //ensures parcel exists and updates its status
//            {
//                DataSource.ParcelList.RemoveAll(temp => temp.parcelId == parcelToUpdate.parcelId);
//                DataSource.ParcelList.Add(parcelToUpdate);

//            }
//            return complete;

//        }
//        #endregion
//        #region chargeDrone
//        public string chargeDrone(Drone droneToUpdate, string stationNum) //charges drone
//        {

//            Drone myDrone = DataSource.DroneList.Find(x => x.droneId == droneToUpdate.droneId);
//            if (myDrone.droneId == 0)
//                throw new DoesntExistException("This drone doesn't exist in the system");
//            string complete = "Your request was completed successfully";
//            //  drone.Status = DroneStatuses.maintenance;
//            //  drone.Battery = 0;
//            Station station = DataSource.StationList.Find(temp => (temp.name == stationNum)); //builds station
//            if (station.stationId != 0 && droneToUpdate.droneId != 0) //if station exists updates it
//            {
//                DataSource.StationList.Remove(station); //removes station
//                DataSource.DroneList.RemoveAll(temp => temp.droneId == droneToUpdate.droneId); //removes station
//                station.chargeSlots--;
//                DroneCharge charge = new DroneCharge();
//                charge.droneId = droneToUpdate.droneId;
//                charge.stationId = station.stationId;
//                DataSource.DroneChargeList.Add(charge); //adds new charge to chargedroen list
//                DataSource.StationList.Add(station); //adds updated station
//                DataSource.DroneList.Add(droneToUpdate); //adds updated drone

//            }
//            return complete;

//        }
//        #endregion
//        #region releaseDrone
//        public string releaseDrone(DroneCharge charge) //releases drone from charge
//        {
//            Drone myDrone = DataSource.DroneList.Find(temp => (temp.droneId == charge.droneId)); //pulls correct drone
//            if (myDrone.droneId == 0)
//                throw new DoesntExistException("This drone doesn't exist in the system");
//            string complete = "Your request was completed successfully";

//            if (myDrone.droneId != 0) //if droen exists updates  
//            {
//                // drone.Status = DroneStatuses.available;
//                //drone.Battery = 100;
//                DataSource.DroneList.RemoveAll(m => (m.droneId == charge.droneId));
//                DataSource.DroneList.Add(myDrone);
//            }

//            Station myStation = DataSource.StationList.Find(s => (s.stationId == charge.stationId)); //pulls correct station
//            if (myStation.stationId == 0)
//                throw new DoesntExistException("This drone doesn't exist in the system");
//            if (myStation.stationId != 0) //if station exists updates
//            {
//                myStation.chargeSlots++;
//                DataSource.StationList.RemoveAll(temp => (temp.stationId == charge.stationId));
//                DataSource.StationList.Add(myStation);
//            }

//            DataSource.DroneChargeList.RemoveAll(temp => temp.droneId == charge.droneId);
//            return complete;
//        }
//        #endregion
//        #region PrintStation
//        public string PrintStation(int stationId) //prints a station
//        {
//            if (findStation(stationId).stationId != 0)
//                return findStation(stationId).ToString();
//            throw new DoesntExistException("The station doesn't exist in system");
//        }
//        #endregion
//        #region PrintDrone
//        public string PrintDrone(int droneId) //prints a drone
//        {
//            if (findDrone(droneId).droneId != 0)
//                return findDrone(droneId).ToString();
//            throw new DoesntExistException("The drone doesn't exist in system");
//        }
//        #endregion
//        #region PrintCustomer
//        public string PrintCustomer(int customerId) //prints a customer
//        {
//            if (findCustomer(customerId).customerId != 0)
//                return findCustomer(customerId).ToString();
//            throw new DoesntExistException("The customer doesn't exist in system");
//        }
//        #endregion
//        #region PrintParcel
//        public string PrintParcel(int parcelId) //prints a parcel
//        {
//            if (findParcel(parcelId).parcelId != 0)
//                return findParcel(parcelId).ToString();
//            throw new DoesntExistException("The parcel doesn't exist in system");
//        }
//        #endregion
//        #region findParcel
//        public Parcel findParcel(int parcelId) //finds a parcel using its id
//        {

//            for (int i = 0; i < DataSource.ParcelList.Count(); i++) //goes over parcel list
//            {
//                if (DataSource.ParcelList[i].parcelId == parcelId) //if id matches
//                {
//                    return (DataSource.ParcelList[i]);
//                }
//            }
//            throw new DoesntExistException("The parcel doesn't exist in system");
//        }
//        #endregion
//        #region findCustomer
//        public Customer findCustomer(int customerId) //finds a customer using its id
//        {

//            for (int i = 0; i < DataSource.CustomerList.Count(); i++) //goes over customer list
//            {
//                if (DataSource.CustomerList[i].customerId == customerId) //if id matches
//                {
//                    return (DataSource.CustomerList[i]);
//                }
//            }
//            throw new DoesntExistException("The customer doesn't exist in system");
//        }
//        #endregion
//        public DroneCharge findDroneCharge(int droneId, int stationId)
//        {

//            for (int i = 0; i < DataSource.DroneChargeList.Count(); i++)
//            {
//                if (DataSource.DroneChargeList[i].droneId == droneId && DataSource.DroneChargeList[i].stationId == stationId) //if id matches
//                {
//                    return (DataSource.DroneChargeList[i]);
//                }
//            }
//            throw new DoesntExistException("The drone or station doesn't exist in system");
//        }
//        #region findDrone
//        public Drone findDrone(int droneId) //finds a drone using its id
//        {

//            for (int i = 0; i < DataSource.DroneList.Count(); i++) //goes over drone list
//            {
//                if (DataSource.DroneList[i].droneId == droneId) //if id matches
//                {
//                    return (DataSource.DroneList[i]);
//                }
//            }
//            throw new DoesntExistException("The drone doesn't exist in system");
//        }
//        #endregion
//        #region findStation
//        public Station findStation(int stationId) //finds a station using its id
//        {

//            for (int i = 0; i < DataSource.StationList.Count(); i++) //goes over station list
//            {
//                if (DataSource.StationList[i].stationId == stationId) //if id matches
//                {
//                    return (DataSource.StationList[i]);
//                }
//            }
//            throw new DoesntExistException("The station doesn't exist in system");

//        }
//        #endregion
//        #region findDroneCharge
//        public DroneCharge findDroneCharge(int droneChargeId) //finds a drone charge using its id
//        {

//            for (int i = 0; i < DataSource.DroneChargeList.Count(); i++) //goes over dronecharge list
//            {
//                if (DataSource.DroneChargeList[i].droneId == droneChargeId) //if ifd match
//                {
//                    return (DataSource.DroneChargeList[i]);
//                }
//            }
//            throw new DoesntExistException("The dronecharge doesn't exist in system");
//        }
//        #endregion
//        #region getParcelId
//        public int getParcelId() //returns parcel id
//        {
//            return DataSource.config.assignparcelId++; //genrates parcel id
//        }
//        #endregion
//        #region distance
//        public double distance(double lattitude1, double longitute1, double lattitude2, double longitute2) //calculates distance between coordinates for bonus
//        {
//            //ditance between 2 points is: sqrt of- pow2(x1-x2) + pow2(y1-y2)
//            lattitude1 = lattitude1 - lattitude2; //(x1-x2)
//            longitute1 = longitute1 - longitute2; //(y1 - y2)
//            lattitude1 = Math.Pow((lattitude1), 2); //pow2(update)
//            longitute1 = Math.Pow(longitute1, 2); //pow2(update)
//            lattitude1 = lattitude1 + longitute1; //update + update
//            return Math.Sqrt(lattitude1); //sqrt of update
//        }
//        #endregion
//        #region ChargeCapacity
//        public double[] ChargeCapacity()
//        {

//            double[] arr = new double[] { DataSource.config.available, DataSource.config.lightLoad, DataSource.config.mediumLoad, DataSource.config.heavyLoad, DataSource.config.chargeSpeed };
//            return arr;

//        }
//        #endregion
//        #region printStationsList
//        public IEnumerable<Station> printStationsList() //prints list of stations 
//        {

//            foreach (Station item in DataSource.StationList)
//            {
//                yield return item;
//            }


//        }
//        #endregion
//        #region printDronesList
//        public IEnumerable<Drone> printDronesList() //prints list of drone
//        {
//            foreach (Drone item in DataSource.DroneList)
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region printCustomersList
//        public IEnumerable<Customer> printCustomersList() //prints customer list
//        {
//            foreach (Customer item in DataSource.CustomerList)
//            {
//                yield return item;
//            }

//        }
//        #endregion
//        #region printParcelsList
//        public IEnumerable<Parcel> printParcelsList() //prints parcel list
//        {
//            foreach (Parcel item in DataSource.ParcelList)
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region printDroneChargeList
//        public IEnumerable<DroneCharge> printDroneChargeList() //prints DroneCharge list
//        {
//            foreach (DroneCharge item in DataSource.DroneChargeList)
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region attribute
//        public void attribute(int dID, int pID)//the function attribute parcel to drone
//        {
//            Drone tmpD = getDrone(dID);
//            Parcel tmpP = getParcel(pID);
//            DataSource.ParcelList.RemoveAll(m => m.parcelId == tmpP.parcelId);   //removing all the data from the place in the list the equal to tmpP id
//            tmpP.droneId = tmpD.droneId;        //attribute drones id to parcel 
//            tmpP.scheduled = DateTime.Now; //changing the time to be right now
//            DataSource.ParcelList.Add(tmpP); //adding to the parcel list tmpP
//        }
//        #endregion



//    }


//}

