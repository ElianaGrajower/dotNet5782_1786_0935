//using System;
//using DalApi;
//using DO;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Linq;


//namespace DalXml
//{
//    sealed class DlXml : IDal
//    {



//        public static Random r = new Random();
//        #region singelton
//        static DlXml() { }
//        static readonly IDal instance = new DlXml();
//        public static IDal Instance { get => instance; }
//        #endregion
//        // private DlXml() { DlXml.Initialize(); } // default constructer calls on initialize func


//        string configPath = @"staticConfigXml.xml"; //XElement
//        string DroneChargesPath = @"DroneChargesXml.xml"; //XElement-switched
//        string StationsPath = @"StationsXml.xml"; //XMLSerializer
//        string DronesPath = @"DronesXml.xml"; //XMLSerializer
//        string CustomersPath = @"CustomersXml.xml"; //XMLSerializer
//        string ParcelsPath = @"ParcelsXml.xml"; //xmlserializer


//        #region  getStation /doesnt need did active
//        public Station getStation(int stationId)
//        {

//            try
//            {
//               var station= findStation(stationId);
//                if (station.active)
//                    return station;
//                else
//                    throw new DoesntExistException("This station doesnt exist in the system\n");
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getDrone /doesnt need did active
//        public Drone getDrone(int droneId)
//        {
//            try
//            {
//                var drone= findDrone(droneId);
//                if (drone.active)
//                    return drone;
//                else
//                    throw new DoesntExistException("This drone doesnt exist\n");
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getCustomer /doesnt need did active
//        public Customer getCustomer(int customerId)
//        {
//            try
//            {
//                var customer= findCustomer(customerId);
//                if (customer.active)
//                    return customer;
//                else
//                    throw new DoesntExistException("This customer doesnt in the system\n");
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getParcel /doesnt need did active
//        public Parcel getParcel(int parcelId)
//        {
//            try
//            {
//                var parcel= findParcel(parcelId);
//                if (parcel.active)
//                    return parcel;
//                else
//                    throw new DoesntExistException("This parcel doesnt exist int he system\n");
//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }
//        }
//        #endregion
//        #region UpdateDrone /did xmlserializer and active
//        public void UpdateDrone(Drone droneToUpdate)
//        {
//            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
//            listDrones.RemoveAll(x => x.droneId == droneToUpdate.droneId);
//            droneToUpdate.active = true;
//            listDrones.Add(droneToUpdate);
//            XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);


//        }
//        #endregion
//        #region UpdateStation/*did the xmlserializer*/ and active
//        public void UpdateStation(Station stationToUpdate)
//        {
//            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
//            listStations.RemoveAll(x => x.stationId == stationToUpdate.stationId);
//            stationToUpdate.active = true;
//            listStations.Add(stationToUpdate);
//            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);


//        }
//        #endregion
//        #region UpdateCustomer/*did the xmlserializer*/ and active
//        public void UpdateCustomer(Customer customerToUpdate)
//        {

//            List<Customer> listcustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
//            listcustomers.RemoveAll(x => x.customerId == customerToUpdate.customerId);
//            customerToUpdate.active = true;
//            listcustomers.Add(customerToUpdate);
//            XMLTools.SaveListToXMLSerializer(listcustomers, CustomersPath);


//        }
//        #endregion
//        #region UpdateParcel /*did the xmlserializer and active
//        public void UpdateParcel(Parcel parcelToUpdate)
//        {

//            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
//            listParcels.RemoveAll(x => x.parcelId == parcelToUpdate.parcelId);
//            parcelToUpdate.active = true;
//            listParcels.Add(parcelToUpdate);
//            XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);


//        }
//        #endregion
//        #region AddStation /*did the xmlserializer*/ and active
//        public void AddStation(Station stationToAdd) //adds station to list
//        {

//            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
//            if (listStations.Count(x => x.stationId == stationToAdd.stationId) != 0 && listStations.Count(x => getStation(x.stationId).active == true) != 0)
//                throw new AlreadyExistException("The station already exist in the system");
//            stationToAdd.active = true;
//            listStations.Add(stationToAdd);
//            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
//        }
//        #endregion
//        #region  AddDrone /*did the xmlserializer*/ and active
//        public void AddDrone(Drone droneToAdd) //adds drone to list
//        {
//            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
//            if (listDrones.Count(x => x.droneId == droneToAdd.droneId) != 0 && listDrones.Count(x => getDrone(x.droneId).active == true) != 0)
//                throw new AlreadyExistException("The drone already exist in the system");
//            droneToAdd.active = true;
//            listDrones.Add(droneToAdd);
//            XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);

//        }
//        public void AddDroneCharge(DroneCharge droneChargeToAdd) //adds drone to list
//        {
//            if (DataSource.DroneChargeList.Count(x => x.droneId == droneChargeToAdd.droneId) != 0 && DataSource.DroneChargeList.Count(x => getDroneCharge(x.droneId).active == true) != 0)
//                throw new AlreadyExistException("The drone is already being charged at a station");
//            droneChargeToAdd.active = true;
//            DataSource.DroneChargeList.Add(droneChargeToAdd);


//        }

//        #endregion
//        #region AddCustomer /*did the xmlseriler and active
//        public void AddCustomer(Customer customerToAdd) //adds customer to list
//        {
//            List<Customer> listCustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
//            if (listCustomers.Count(x => x.customerId == customerToAdd.customerId) != 0 && listCustomers.Count(x => getCustomer(x.customerId).active == true) != 0)
//                throw new AlreadyExistException("A customer with this id already exist in the system");
//            if (listCustomers.Count(x => x.name == customerToAdd.name) != 0 && listCustomers.Count(x => getCustomer(x.customerId).active == true) != 0)
//                throw new AlreadyExistException("A customer with this user name already exist in the system");
//            customerToAdd.active = true;
//            listCustomers.Add(customerToAdd);
//            XMLTools.SaveListToXMLSerializer(listCustomers, CustomersPath);
//        }
//        #endregion
//        #region AddParcel /did the xmlSerializer*/ and active
//        public void AddParcel(Parcel parcelToAdd) //adds parcel to list
//        {
//            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
//            if (listParcels.Count(x => x.parcelId == parcelToAdd.parcelId) != 0 && listParcels.Count(x => getParcel(x.parcelId).active == true) != 0)
//                throw new AlreadyExistException("The parcel already exist in the system");
//            parcelToAdd.active = true;
//            listParcels.Add(parcelToAdd);
//            XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
//        }
//        #endregion
//        #region deleteDrone /*did the xmlserializer and active
//        public void deleteDrone(int id)
//        {
//            try
//            {
//                List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
//                var drone = findDrone(id);
//                if (drone.active == true)
//                {
//                    var temp = listDrones.Find(d => d.droneId == id);
//                    temp.active = false;
//                    listDrones.Remove(drone);
//                    listDrones.Add(temp);
//                }
//                else
//                    throw new DoesntExistException("This drone doesnt exist in the system\n");
    
//                XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);

//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }

//        }
//        #endregion
//        #region deleteCustomer did the serialzier and active
//        public void deleteCustomer(int id)
//        {
//            try
//            {
//                List<Customer> listCustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
//                var customer = findCustomer(id);
//                if (customer.active==true)
//                {
//                    var temp = listCustomers.Find(d => d.customerId == id);
//                    temp.active = false;
//                    listCustomers.Remove(customer);
//                    listCustomers.Add(temp);
//                }
//                else
//                    throw new DoesntExistException("This customer doesnt exist in the system");
//                XMLTools.SaveListToXMLSerializer(listCustomers, CustomersPath);

//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }

//        }
//        #endregion
//        #region deleteParcel/* did xmlserializer and active
//        public void deleteParcel(int id)
//        {

//            try
//            {
//                List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
//                var parcel = findParcel(id);
//                if (parcel.active == true && parcel.scheduled == DateTime.MinValue)
//                {
//                    var temp = listParcels.Find(d => d.parcelId == id);
//                    temp.active = false;
//                    listParcels.Remove(parcel);
//                    listParcels.Add(temp);
//                }
//                else
//                    throw new DoesntExistException("This parcel doesnt exist in the system\n");


//                XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }

//        }
//        #endregion
//        #region deleteStation /*did xmlseriallizer and active
//        public void deleteStation(int id)
//        {
//            try
//            {
//                List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
//                var station = findStation(id);
//                if (station.active == true)
//                {
//                    var temp = listStations.Find(d => d.stationId == id);
//                    temp.active = false;
//                    listStations.Remove(station);
//                    listStations.Add(temp);
//                }
//                else
//                    throw new DoesntExistException("station doesnt exist\n");

//                XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }


//        }
//        #endregion
//        #region deleteDroneCharge
//        public void deleteDroneCharge(int droneId, int stationId)
//        {

//            try
//            {
//                //if (findDroneCharge(droneId).active)
//                //{
//                //    var temp = DataSource.DroneChargeList.Find(d => d.droneId == droneId);
//                //    var rid = DataSource.DroneChargeList.Find(d => d.droneId == droneId);
//                //    temp.active = false;
//                //    DataSource.DroneChargeList.Remove(rid);
//                //    DataSource.DroneChargeList.Add(temp);
//                //}
//                DataSource.DroneChargeList.Remove(findDroneCharge(droneId));
//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }

//        }
//        #endregion
//        #region PrintStation /doesnt need
//        public string PrintStation(int stationId) //prints a station
//        {
//            if (findStation(stationId).stationId != 0)
//                return findStation(stationId).ToString();
//            throw new DoesntExistException("The station doesn't exist in system");
//        }
//        #endregion
//        #region PrintDrone /doesnt need
//        public string PrintDrone(int droneId) //prints a drone
//        {
//            if (findDrone(droneId).droneId != 0)
//                return findDrone(droneId).ToString();
//            throw new DoesntExistException("The drone doesn't exist in system");
//        }
//        #endregion
//        #region PrintCustomer /doesnt need
//        public string PrintCustomer(int customerId) //prints a customer
//        {
//            if (findCustomer(customerId).customerId != 0)
//                return findCustomer(customerId).ToString();
//            throw new DoesntExistException("The customer doesn't exist in system");
//        }
//        #endregion
//        #region PrintParcel /doesnt need
//        public string PrintParcel(int parcelId) //prints a parcel
//        {
//            if (findParcel(parcelId).parcelId != 0)
//                return findParcel(parcelId).ToString();
//            throw new DoesntExistException("The parcel doesn't exist in system");
//        }
//        #endregion
//        #region findParcel /*did xmlserializer
//        public Parcel findParcel(int parcelId) //finds a parcel using its id
//        {
//            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);



//            for (int i = 0; i < listParcels.Count(); i++) //goes over parcel list
//            {
//                if (listParcels[i].parcelId == parcelId) //if id matches
//                {
//                    return (listParcels[i]);
//                }

//            }
//            throw new DoesntExistException("The parcel doesn't exist in system");

//        }
//        #endregion
//        #region findCustomer/did xmlserialier
//        public Customer findCustomer(int customerId) //finds a customer using its id
//        {

//            List<Customer> listCustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);

//            for (int i = 0; i < listCustomers.Count(); i++) //goes over customer list
//            {
//                if (listCustomers[i].customerId == customerId) //if id matches
//                {
//                    return (listCustomers[i]);

//                }
//            }

//            throw new DoesntExistException("The customer doesn't exist in system");


//        }
//        #endregion
//        #region checkCustomer /did xml serialier
//        public Customer checkCustomer(string name, string password) //finds a customer using its id
//        {
//            bool flagExist = false, coorectPassword = false;
//            int j = 0;
//            List<Customer> listCustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
//            for (int i = 0; i < listCustomers.Count(); i++) //goes over customer list
//            {
//                if (listCustomers[i].name == name) //if id matches
//                {
//                    j = i;
//                    flagExist = true;
//                    if (listCustomers[i].password == password)
//                        coorectPassword = true;


//                }
//            }
//            if (!flagExist)
//                throw new DoesntExistException("The customer doesn't exist in system");
//            if (!coorectPassword)
//                throw new DoesntExistException("Incorrect Paswword\n");
//            return (listCustomers[j]);
//        }
//        #endregion
//        #region findDroneCharge
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
//        #endregion
//        #region findDrone /didxmlserializer
//        public Drone findDrone(int droneId) //finds a drone using its id
//        {

//            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
//            for (int i = 0; i < listDrones.Count(); i++) //goes over drone list
//            {
//                if (listDrones[i].droneId == droneId) //if id matches
//                {
//                    return (listDrones[i]);
//                }
//            }
//            throw new DoesntExistException("The drone doesn't exist in system");
//        }
//        #endregion
//        #region findStation /did the xmlserilizer
//        public Station findStation(int stationId) //finds a station using its id
//        {
//            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
//            for (int i = 0; i < listStations.Count(); i++) //goes over station list
//            {
//                if (listStations[i].stationId == stationId) //if id matches
//                {
//                    return (listStations[i]);
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
//        #region ChargeCapacity
//        public double[] ChargeCapacity()
//        {

//            double[] arr = new double[] { DataSource.config.available, DataSource.config.lightLoad, DataSource.config.mediumLoad, DataSource.config.heavyLoad, DataSource.config.chargeSpeed };
//            return arr;

//        }
//        #endregion
//        #region printStationsList /*did xml serializer
//        public IEnumerable<Station> printStationsList() //prints list of stations 
//        {
//            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
//            foreach (Station item in listStations.Where(s => s.active == true))
//            {
//                yield return item;
//            }


//        }
//        #endregion
//        #region printDronesList /*did xmlserializer
//        public IEnumerable<Drone> printDronesList() //prints list of drone
//        {

//            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
//            foreach (Drone item in listDrones.Where(s => s.active == true))
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region printCustomersList /*did xmlserializer
//        public IEnumerable<Customer> printCustomersList() //prints customer list
//        {
//            List<Customer> listCcustomers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomersPath);
//            foreach (Customer item in listCcustomers.Where(s => s.active == true))
//            {
//                yield return item;
//            }

//        }
//        #endregion
//        #region printParcelsList
//        public IEnumerable<Parcel> printParcelsList() //prints parcel list
//        {
//            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

//            foreach (Parcel item in listParcels.Where(s => s.active == true))
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region printDroneChargeList
//        public IEnumerable<DroneCharge> printDroneChargeList() //prints DroneCharge list
//        {
//            foreach (DroneCharge item in DataSource.DroneChargeList.Where(s => s.active == true))
//            {
//                yield return item;
//            }
//        }
//        #endregion
//        #region attribute did active
//        public void attribute(int dID, int pID)//the function attribute parcel to drone
//        {
//            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
//            try
//            {
//                Drone tmpD = getDrone(dID);
//                Parcel tmpP = getParcel(pID);
//                listParcels.RemoveAll(m => m.parcelId == tmpP.parcelId);   //removing all the data from the place in the list the equal to tmpP id
//                tmpP.droneId = tmpD.droneId;        //attribute drones id to parcel 
//                tmpP.scheduled = DateTime.Now; //changing the time to be right now
//                listParcels.Add(tmpP); //adding to the parcel list tmpP
//                XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
//            }
//            catch(Exception exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }
//        }
//        #endregion
//        #region returnCustomer /* doesnt need/
//        public Customer returnCustomer(string name, string password)
//        {
//            try
//            {
//                var customer=checkCustomer(name, password);
//                if (customer.active)
//                    return customer;
//                else
//                    throw new DoesntExistException("This customer doesnt exist in the system\n");

//            }
//            catch (DoesntExistException exc)
//            {
//                throw exc;
//            }
//        }
//        #endregion
//        #region getDroneCharge
//        public DroneCharge getDroneCharge(int droneId)
//        {
//            try
//            {
//                return findDroneCharge(droneId);
//            }
//            catch (DoesntExistException exc)
//            {
//                throw new DoesntExistException(exc.Message);
//            }
//        }
//        #endregion

//    }


//}

