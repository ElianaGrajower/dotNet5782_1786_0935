using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DAL;
using DO;

namespace Dal
{

    sealed class DalObject : DalApi.IDal
    {
        public static Random r = new Random();
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
       
        private DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func



        #region  getStation
        public Station getStation(int stationId)
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
        #region getDrone
        public Drone getDrone(int droneId)
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
        #region getCustomer
        public Customer getCustomer(int customerId)
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
        #region getParcel
        public Parcel getParcel(int parcelId)
        {
            try
            {
                return findParcel(parcelId);
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }
        }
        #endregion
        #region UpdateDrone
        public void UpdateDrone(Drone droneToUpdate)
        {

            DataSource.DroneList.RemoveAll(x => x.droneId == droneToUpdate.droneId);
            DataSource.DroneList.Add(droneToUpdate);


        }
        #endregion
        #region UpdateStation
        public void UpdateStation(Station stationToUpdate)
        {

            DataSource.StationList.RemoveAll(x => x.stationId == stationToUpdate.stationId);
            DataSource.StationList.Add(stationToUpdate);


        }
        #endregion
        #region UpdateCustomer
        public void UpdateCustomer(Customer customerToUpdate)
        {

            DataSource.CustomerList.RemoveAll(x => x.customerId == customerToUpdate.customerId);
            DataSource.CustomerList.Add(customerToUpdate);


        }
        #endregion
        #region UpdateParcel
        public void UpdateParcel(Parcel parcelToUpdate)
        {

            DataSource.ParcelList.RemoveAll(x => x.parcelId == parcelToUpdate.parcelId);
            DataSource.ParcelList.Add(parcelToUpdate);


        }
        #endregion
        #region AddStation
        public void AddStation(Station stationToAdd) //adds station to list
        {
            
           
                if (DataSource.StationList.Count(x => x.stationId == stationToAdd.stationId) != 0 && DataSource.StationList.Count(x =>getStation(x.stationId).active==true)!=0)
                throw new AlreadyExistException("The station already exist in the system");
            stationToAdd.active = true;
            DataSource.StationList.Add(stationToAdd);
        }
        #endregion
        #region  AddDrone
        public void AddDrone(Drone droneToAdd) //adds drone to list
        {
            if (DataSource.DroneList.Count(x => x.droneId == droneToAdd.droneId) != 0 &&DataSource.DroneList.Count(x => getDrone(x.droneId).active==true)!=0)
                throw new AlreadyExistException("The drone already exist in the system");
            droneToAdd.active = true;
            DataSource.DroneList.Add(droneToAdd);


        }
        public void AddDroneCharge(DroneCharge droneChargeToAdd) //adds drone to list
        {
            if (DataSource.DroneChargeList.Count(x => x.droneId == droneChargeToAdd.droneId) != 0 && DataSource.DroneChargeList.Count(x => getDroneCharge(x.droneId).active == true) != 0)
              throw new AlreadyExistException("The drone is already being charged at a station");
            droneChargeToAdd.active = true;
            DataSource.DroneChargeList.Add(droneChargeToAdd);


        }

        #endregion
        #region AddCustomer
        public void AddCustomer(Customer customerToAdd) //adds customer to list
        {

            if (DataSource.CustomerList.Count(x => x.customerId == customerToAdd.customerId) !=0 && DataSource.CustomerList.Count(x => getCustomer(x.customerId).active == true) != 0)
                throw new AlreadyExistException("The customer already exist in the system");
           customerToAdd.active = true;
            DataSource.CustomerList.Add(customerToAdd);
        }
        #endregion
        #region AddParcel
        public void AddParcel(Parcel parcelToAdd) //adds parcel to list
        {
            if (DataSource.ParcelList.Count(x => x.parcelId == parcelToAdd.parcelId) != 0 &&DataSource.ParcelList.Count(x => getParcel(x.parcelId).active == true) != 0)
                throw new AlreadyExistException("The parcel already exist in the system");
            parcelToAdd.active = true;
            DataSource.ParcelList.Add(parcelToAdd);
        }
        #endregion
        #region deleteDrone
        public void deleteDrone(int id)
        {
            try
            {
                //if (findDrone(id).active==true)
                //{
                //    var temp = DataSource.DroneList.Find(d => d.droneId == id);
                //    var rid = DataSource.DroneList.Find(d => d.droneId == id);
                //    temp.active = false;
                //    DataSource.DroneList.Remove(rid);
                //    DataSource.DroneList.Add(temp);
                //}
                 DataSource.DroneList.Remove(findDrone(id));

            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region deleteCustomer
        public void deleteCustomer(int id)
        {
            try
            {
                //if (findCustomer(id).active)
                //{
                //    var temp = DataSource.CustomerList.Find(d => d.customerId == id);
                //    var rid = DataSource.CustomerList.Find(d => d.customerId == id);
                //    temp.active = false;
                //    DataSource.CustomerList.Remove(rid);
                //    DataSource.CustomerList.Add(temp);
                //}
                DataSource.CustomerList.Remove(findCustomer(id));

            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region deleteParcel
        public void deleteParcel(int id)
        {

            try
            {
                //if (findParcel(id).active==true && findParcel(id).scheduled==DateTime.MinValue)
                //{
                //    var temp = DataSource.ParcelList.Find(d => d.parcelId == id);
                //    var rid = DataSource.ParcelList.Find(d => d.parcelId == id);
                //    temp.active = false;
                //    DataSource.ParcelList.Remove(rid);
                //    DataSource.ParcelList.Add(temp);
                //}
                 DataSource.ParcelList.Remove(findParcel(id));
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region deleteStation
        public void deleteStation(int id)
        {
            try
            {
                // findStation(id);
                // DataSource.StationList.ForEach(s => { if (s.stationId == id) s.active = false; });

                //if (findStation(id).active == true)
                //{
                //    var temp = DataSource.StationList.Find(d => d.stationId == id);
                //    // var rid = DataSource.StationList.Find(d => d.stationId == id);
                //    temp.active = false;
                //    DataSource.StationList.Remove(findStation(id));
                //    DataSource.StationList.Add(temp);
                //}
                //else
                //   throw new DoesntExistException("station doesnt exist\n");
                 DataSource.StationList.Remove(findStation(id));
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }


        }
        #endregion
        #region deleteDroneCharge
        public void deleteDroneCharge(int droneId, int stationId)
        {

            try
            {
                //if (findDroneCharge(droneId).active)
                //{
                //    var temp = DataSource.DroneChargeList.Find(d => d.droneId == droneId);
                //    var rid = DataSource.DroneChargeList.Find(d => d.droneId == droneId);
                //    temp.active = false;
                //    DataSource.DroneChargeList.Remove(rid);
                //    DataSource.DroneChargeList.Add(temp);
                //}
                  DataSource.DroneChargeList.Remove(findDroneCharge(droneId));
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region PrintStation
        public string PrintStation(int stationId) //prints a station
        {
            if (findStation(stationId).stationId != 0)
                return findStation(stationId).ToString();
            throw new DoesntExistException("The station doesn't exist in system");
        }
        #endregion
        #region PrintDrone
        public string PrintDrone(int droneId) //prints a drone
        {
            if (findDrone(droneId).droneId != 0)
                return findDrone(droneId).ToString();
            throw new DoesntExistException("The drone doesn't exist in system");
        }
        #endregion
        #region PrintCustomer
        public string PrintCustomer(int customerId) //prints a customer
        {
            if (findCustomer(customerId).customerId != 0)
                return findCustomer(customerId).ToString();
            throw new DoesntExistException("The customer doesn't exist in system");
        }
        #endregion
        #region PrintParcel
        public string PrintParcel(int parcelId) //prints a parcel
        {
            if (findParcel(parcelId).parcelId != 0)
                return findParcel(parcelId).ToString();
            throw new DoesntExistException("The parcel doesn't exist in system");
        }
        #endregion
        #region findParcel
        public Parcel findParcel(int parcelId) //finds a parcel using its id
        {

            for (int i = 0; i < DataSource.ParcelList.Count(); i++) //goes over parcel list
            {
                if (DataSource.ParcelList[i].parcelId == parcelId) //if id matches
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
                if (DataSource.CustomerList[i].customerId == customerId) //if id matches
                {
                    return (DataSource.CustomerList[i]);

                }
            }
         
                throw new DoesntExistException("The customer doesn't exist in system");
           
           
        }
        #endregion
        #region checkCustomer
        public Customer checkCustomer(string name, string password) //finds a customer using its id
        {
            bool flagExist = false, coorectPassword = false;
            int j = 0;
            for (int i = 0; i < DataSource.CustomerList.Count(); i++) //goes over customer list
            {
                if (DataSource.CustomerList[i].name == name) //if id matches
                {
                    j = i;
                    flagExist = true;
                    if (DataSource.CustomerList[i].password == password)
                        coorectPassword = true;


                }
            }
            if (!flagExist)
                throw new DoesntExistException("The customer doesn't exist in system");
            if (!coorectPassword)
                throw new DoesntExistException("Incorrect Paswword\n");
            return (DataSource.CustomerList[j]);
        }
        #endregion
        #region findDroneCharge
        public DroneCharge findDroneCharge(int droneId, int stationId)
        {

            for (int i = 0; i < DataSource.DroneChargeList.Count(); i++)
            {
                if (DataSource.DroneChargeList[i].droneId == droneId && DataSource.DroneChargeList[i].stationId == stationId) //if id matches
                {
                    return (DataSource.DroneChargeList[i]);
                }
            }
            throw new DoesntExistException("The drone or station doesn't exist in system");
        }
        #endregion
        #region findDrone
        public Drone findDrone(int droneId) //finds a drone using its id
        {

            for (int i = 0; i < DataSource.DroneList.Count(); i++) //goes over drone list
            {
                if (DataSource.DroneList[i].droneId == droneId) //if id matches
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
                if (DataSource.StationList[i].stationId == stationId) //if id matches
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
                if (DataSource.DroneChargeList[i].droneId == droneChargeId) //if ifd match
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
            return DataSource.config.assignparcelId++; //genrates parcel id
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

            foreach (Station item in DataSource.StationList.Where(s => s.active == true))
            {
                yield return item;
            }


        }
        #endregion
        #region printDronesList
        public IEnumerable<Drone> printDronesList() //prints list of drone
        {
            foreach (Drone item in DataSource.DroneList.Where(s => s.active == true))
            {
                yield return item;
            }
        }
        #endregion
        #region printCustomersList
        public IEnumerable<Customer> printCustomersList() //prints customer list
        {
            foreach (Customer item in DataSource.CustomerList.Where(s => s.active == true))
            {
                yield return item;
            }

        }
        #endregion
        #region printParcelsList
        public IEnumerable<Parcel> printParcelsList() //prints parcel list
        {
            foreach (Parcel item in DataSource.ParcelList.Where(s => s.active == true))
            {
                yield return item;
            }
        }
        #endregion
        #region printDroneChargeList
        public IEnumerable<DroneCharge> printDroneChargeList() //prints DroneCharge list
        {
            foreach (DroneCharge item in DataSource.DroneChargeList.Where(s => s.active == true))
            {
                yield return item;
            }
        }
        #endregion
        #region attribute
        public void attribute(int dID, int pID)//the function attribute parcel to drone
        {
            Drone tmpD = getDrone(dID);
            Parcel tmpP = getParcel(pID);
            DataSource.ParcelList.RemoveAll(m => m.parcelId == tmpP.parcelId);   //removing all the data from the place in the list the equal to tmpP id
            tmpP.droneId = tmpD.droneId;        //attribute drones id to parcel 
            tmpP.scheduled = DateTime.Now; //changing the time to be right now
            DataSource.ParcelList.Add(tmpP); //adding to the parcel list tmpP
        }
        #endregion
        #region returnCustomer
        public Customer returnCustomer(string name,string password)
        {
            try
            {
                return checkCustomer(name,password);
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region getDroneCharge
        public DroneCharge getDroneCharge(int droneId)
        {
            try
            {
                return findDroneCharge(droneId);
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }
        }
        #endregion

    }


}