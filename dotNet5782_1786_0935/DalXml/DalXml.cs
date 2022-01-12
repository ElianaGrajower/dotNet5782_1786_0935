using System;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace Dal
{
    sealed class DalXml : IDal
    {

        internal  string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "1111aaa" ,"@gmail.com"};
        internal  string[] stationName = { "Raanana Central Station", "Tel Aviv Central Station" };
        internal  string[] droneName = { "Reaper", "Shadow", "Grey Eagle", "Global Hawk", "Pioneer", "Fire Scout", "Snowgoose", "Hunter", "Stalker", "GNAT", "Wing Loong II", "AVENGER", "Apollo Earthly", "AirHaven", "indRazer", "Godspeed", "Phantom", "Novotek", "Tri-Propeller", "WikiDrone" };
        internal  string[] customerName = { "Michael", "Hannah", "Fred", "Sam", "Tom", "Jessie", "George", "Tiffany", "Elizabeth", "Rachel" };


        public static Random r = new Random();
        #region singelton
        static DalXml() { }
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        #endregion
        //private DalXml() { Initialize(); } // default constructer calls on initialize func
                                           // #region Initialize
        public void Initialize()
        {
            createStation(); //creats a station with random information
            createDrone(); //creats a drone with random information
            createCustomer(); //creats a customer with random information
            createParcel(); //creats a parcel with random information

        }

        #region createStation
        public void createStation() //creats a station with random information
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            for (int i = 0; i < 2; i++) //creates 2 stations with information
                listStations.Add(new Station()
                {
                    stationId = r.Next(100000000, 999999999),
                    name = stationName[i],
                    longitude = (r.NextDouble() + r.Next(34, 35)) + 0.57, //gets coordinates for (-90 - 90) 
                    latitude = (r.NextDouble() + r.Next(29, 33)) + 0.207, //gets coordinates for (-180 - 180)
                    chargeSlots = r.Next(1, 100)
                    //active = true
                });

            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
        }
        #endregion
        #region createDrone
        public void createDrone() //creats a drone with random information
        {
            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            for (int i = 0; i < 10; i++) //creates 5 drones with information
                listDrones.Add(new Drone()
                {
                    droneId = r.Next(100000000, 999999999),
                    model = droneName[i],
                    maxWeight = (DO.weightCategories)r.Next(1, 3)
                    // active = true
                });
            XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);
        }
        #endregion
        #region createCustomer
        public void createCustomer() //creats a customer with random information
        {
            XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath); 
            
            string p = letters[10];
            for (int i = 0; i < 8; i++) //creates 8 customers with information
                listCustomers.Add(new XElement("Customer",

                   new XElement("customerId", r.Next(100000000, 999999999)),
                    new XElement("name", customerName[i]),
                    new XElement("Phone", "05" + r.Next(00000000, 99999999)),
                    new XElement("longitude", (r.NextDouble() + r.Next(34, 35)) + 0.57), 
                    new XElement("latitude", (r.NextDouble() + r.Next(29, 33)) + 0.207), 
                     new XElement("password", letters[i] + p),
                     new XElement("isCustomer", true),
                     new XElement("email", customerName[i]+ letters[11])))
                    ;
                
                
            for (int i = 8; i < 10; i++) //creates 2 workers
                listCustomers.Add(new XElement("Customer",

                     new XElement("customerId", r.Next(100000000, 999999999)),
                      new XElement("name", customerName[i]),
                      new XElement("Phone", "05" + r.Next(00000000, 99999999)),
                      new XElement("longitude", (r.NextDouble() + r.Next(34, 35)) + 0.57), //gets coordinates for (-90 - 90) 
                      new XElement("latitude", (r.NextDouble() + r.Next(29, 33)) + 0.207), //gets coordinates for (-180 - 180)
                       new XElement("password", letters[i] + p),
                       new XElement("isCustomer", false),
                        new XElement("email", customerName[i] + letters[11])))
                      ;
            XMLTools.SaveListToXMLElement(listCustomers, CustomersPath);
        }
        #endregion
        #region createParcel
        public void createParcel() //creats a parcel with random information
        {
            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            List<double> list = XMLTools.LoadListFromXMLSerializer<double>(configPath);
            list.Add(0.0009); list.Add(0.001); list.Add(0.002); list.Add(0.003); list.Add(10); list.Add(1001);
            var target = (from x in listCustomers.Elements()
                        where Convert.ToInt32(x.Element("customerId").Value) != 0
                        select x).FirstOrDefault();

            for (int i = 0; i < 10; i++) //creates 10 parcels with information
            {
                var sender = (from x in listCustomers.Elements().Skip(i)
                            where Convert.ToInt32(x.Element("customerId").Value) != 0
                            select x).FirstOrDefault();
                listParcels.Add(new Parcel()
                {

                    parcelId = (int)list[5] + i,
                    weight = (DO.weightCategories)r.Next(1, 3), //chooses a weight from light, average, heavy
                    priority = (DO.Priorities)r.Next(1, 3),
                    requested = DateTime.Now.AddDays(-4 * i),
                    scheduled = DateTime.Now,
                    pickedUp = null,
                    delivered = null,
                    senderId = Convert.ToInt32(sender.Element("customerId").Value),
                    targetId = Convert.ToInt32(target.Element("customerId").Value),
                    droneId = listDrones[i].droneId
                    


                });
            }
            list[5] = list[5] + 10;
            XMLTools.SaveListToXMLSerializer<double>(list, configPath);
            XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
        }
        #endregion



        string configPath = @"staticConfigXml.xml"; //XElement
        string DroneChargesPath = @"DroneChargesXml.xml"; //XElement-switched
        string StationsPath = @"StationsXml.xml"; //XMLSerializer
        string DronesPath = @"DronesXml.xml"; //XMLSerializer
        string CustomersPath = @"CustomersXml.xml"; //XMLSerializer
        string ParcelsPath = @"ParcelsXml.xml"; //xmlserializer

        #region get
        #region  getStation /doesnt need did active
        public Station getStation(int stationId)
        {

            try
            {
                var station = findStation(stationId);
                
                    return station;
            
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region getDrone /doesnt need did active
        public Drone getDrone(int droneId)
        {
            try
            {
                var drone = findDrone(droneId);
                // if (drone.active)
                return drone;
                //  else
                //  throw new DoesntExistException("This drone doesnt exist\n");
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region getCustomer /doesnt need did active
        public Customer getCustomer(int customerId)
        {
            try
            {
                var customer = findCustomer(customerId);
                // if (customer.active)
                return customer;
                // else
                //  throw new DoesntExistException("This customer doesn't exist in the system\n");
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);//switched for exc
            }
        }
        #endregion
        #region getParcel /doesnt need did active
        public Parcel getParcel(int parcelId)
        {
            try
            {
                var parcel = findParcel(parcelId);
                //  if (parcel.active)
                return parcel;
                //  else
                //   throw new DoesntExistException("This parcel doesnt exist int he system\n");
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }
        }
        #endregion
        #region getParcelId//did xmlserializer
        public int getParcelId() //returns parcel id
        {
            List<double> list = XMLTools.LoadListFromXMLSerializer<double>(configPath);
            list[5]++;
            XMLTools.SaveListToXMLSerializer<double>(list, configPath);
            return (int)list[5]--;
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
        #region returnCustomer /* doesnt need/
        public Customer returnCustomer(string name, string password)
        {
            try
            {
                var customer = checkCustomer(name, password);
                return customer;


            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #endregion

        #region update
        #region UpdateDrone /did xmlserializer and active
        public void UpdateDrone(Drone droneToUpdate)
        {
            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            listDrones.RemoveAll(x => x.droneId == droneToUpdate.droneId);
            //  droneToUpdate.active = true;
            listDrones.Add(droneToUpdate);
            XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);


        }
        #endregion
        #region UpdateStation/did the xmlserializer/ and active
        public void UpdateStation(Station stationToUpdate)
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            listStations.RemoveAll(x => x.stationId == stationToUpdate.stationId);
            //stationToUpdate.active = true;
            listStations.Add(stationToUpdate);
            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);


        }
        #endregion
        #region UpdateCustomer/did the xmlserializer/ and active
        public void UpdateCustomer(Customer customerToUpdate)
        {

            
            deleteCustomer(customerToUpdate.customerId);
            AddCustomer(customerToUpdate);


        }
        #endregion
        #region UpdateParcel /*did the xmlserializer and active
        public void UpdateParcel(Parcel parcelToUpdate)
        {

            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            listParcels.RemoveAll(x => x.parcelId == parcelToUpdate.parcelId);
            // parcelToUpdate.active = true;
            listParcels.Add(parcelToUpdate);
            XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);


        }
        #endregion
        #endregion

        #region add
        #region AddStation /did the xmlserializer/ and active
        public void AddStation(Station stationToAdd) //adds station to list
        {


            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (listStations.Count(x => x.stationId == stationToAdd.stationId) != 0)
                throw new AlreadyExistException("The station already exist in the system");
            //  stationToAdd.active = true;
            listStations.Add(stationToAdd);
            XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
        }
        #endregion
        #region  AddDrone /*did the xmlserializer and active
        public void AddDrone(Drone droneToAdd) //adds drone to list
        {
            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            if (listDrones.Count(x => x.droneId == droneToAdd.droneId) != 0)
                throw new AlreadyExistException("The drone already exist in the system");
            //  droneToAdd.active = true;
            listDrones.Add(droneToAdd);
            XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);

        }
        #endregion
        #region AddDroneCharge
        public void AddDroneCharge(DroneCharge droneChargeToAdd) //adds drone to list
        {

            List<DroneCharge> listDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargesPath);
            if (listDroneCharges.Count(x => x.droneId == droneChargeToAdd.droneId) != 0)
                throw new AlreadyExistException("The drone is already being charged at a station");
            listDroneCharges.Add(droneChargeToAdd);
            XMLTools.SaveListToXMLSerializer(listDroneCharges, DroneChargesPath);


            //    XElement droneChargeElement = XMLTools.LoadListFromXMLElement(DroneChargesPath);
            //    IEnumerable<XElement> list = droneChargeElement.Elements();
            //    DroneCharge droneCharge = (from item in droneChargeElement.Elements()
            //                               where int.Parse(droneChargeElement.Element("Drone Id").Value) == droneChargeToAdd.droneId

            //                               select new DroneCharge()
            //                               {
            //                                   droneId = int.Parse(droneChargeElement.Element("Drone id").Value),
            //                                   stationId = int.Parse(droneChargeElement.Element("Station id").Value),
            //                                   //chargeTime = DateTime.Parse(droneChargeElement.Element("Charge Time").Value),
            //                                 //  active = bool.Parse(droneChargeElement.Element("Active").Value),


            //                               }).FirstOrDefault();
            //    if (droneCharge.droneId != null)
            //        throw new AlreadyExistException("The drone is already being charged at a station");
            //  //  droneChargeToAdd.active = true;
            //    droneChargeToAdd.chargeTime = DateTime.Now;
            //    droneChargeElement.Add(droneChargeToAdd);
            //    XMLTools.SaveListToXMLElement(droneChargeElement, DroneChargesPath);


        }

        #endregion
        #region AddCustomer /*did the xmlseriler and active
        public void AddCustomer(Customer customerToAdd) //adds customer to list
        {
            XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
            var ifExists = (from c in listCustomers.Elements()
                            where Convert.ToInt32(c.Element("customerId").Value) == customerToAdd.customerId
                            select c).FirstOrDefault();
            var checkName = (from c in listCustomers.Elements()
                            where (c.Element("name").Value) == customerToAdd.name
                            select c).FirstOrDefault();


            if ( ifExists!=null)
                throw new AlreadyExistException("A customer with this id already exist in the system");
            if (checkName!=null)
                throw new AlreadyExistException("A customer with this user name already exist in the system");

            XElement newAdd = new XElement("Customer",

                   new XElement("customerId", customerToAdd.customerId),
                    new XElement("name", customerToAdd.name),
                    new XElement("Phone", customerToAdd.Phone),
                    new XElement("longitude", customerToAdd.longitude), //gets coordinates for (-90 - 90) 
                    new XElement("latitude", customerToAdd.latitude), //gets coordinates for (-180 - 180)
                     new XElement("password", customerToAdd.password),
                     new XElement("isCustomer", customerToAdd.isCustomer),
                     new XElement("email", customerToAdd.email))
                    ;

            listCustomers.Add(newAdd);



            XMLTools.SaveListToXMLElement(listCustomers, CustomersPath);
        }
        #endregion
        #region AddParcel /did the xmlSerializer*/ and active
        public void AddParcel(Parcel parcelToAdd) //adds parcel to list
        {
            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            if (listParcels.Count(x => x.parcelId == parcelToAdd.parcelId) != 0)
                throw new AlreadyExistException("The parcel already exist in the system");
            // parcelToAdd.active = true;
            listParcels.Add(parcelToAdd);
            XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
        }
        #endregion
        #endregion

        #region delete
        #region deleteDrone /*did the xmlserializer and active
        public void deleteDrone(int id)
        {
            try
            {
                List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
                var drone = findDrone(id);
                //   if (drone.active == true)
                // {
                //  var temp = listDrones.Find(d => d.droneId == id);
                //   temp.active = false;
                listDrones.Remove(drone);
                //   listDrones.Add(temp);
                // }
                // else
                // throw new DoesntExistException("This drone doesnt exist in the system\n");

                XMLTools.SaveListToXMLSerializer(listDrones, DronesPath);

            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region deleteCustomer did the serialzier and active
        public void deleteCustomer(int id)
        {
            try
            {
                XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
                var customer = findCustomer(id);
                (from c in listCustomers.Elements()
                 where Convert.ToInt32(c.Element("customerId").Value) == id
                 select c).Remove();




                XMLTools.SaveListToXMLElement(listCustomers, CustomersPath);

            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region deleteParcel/* did xmlserializer and active
        public void deleteParcel(int id)
        {

            try
            {
                List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
                var parcel = findParcel(id);
                //if (parcel.active == true && parcel.scheduled == null)
                //{
                //    var temp = listParcels.Find(d => d.parcelId == id);
                //    temp.active = false;
                listParcels.Remove(parcel);
                // listParcels.Add(temp);
                //   }
                //  else
                //  throw new DoesntExistException("This parcel doesnt exist in the system\n");


                XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
            }
            catch (DoesntExistException exc)
            {
                throw new DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region deleteStation /*did xmlseriallizer and active
        public void deleteStation(int id)
        {
            try
            {
                List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
                var station = findStation(id);
                //if (station.active == true)
                //{
                //    var temp = listStations.Find(d => d.stationId == id);
                //    temp.active = false;
                listStations.Remove(station);
                //    listStations.Add(temp);
                //}
                //else
                //    throw new DoesntExistException("station doesnt exist\n");

                XMLTools.SaveListToXMLSerializer(listStations, StationsPath);
            }
            catch (DoesntExistException exc)
            {
                throw exc;
            }


        }
        #endregion
        #region deleteDroneCharge //i think i did xekemnt
        public void deleteDroneCharge(int droneId, int stationId)
        {
            List<DroneCharge> listDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargesPath);
            if (listDroneCharges.Count(x => x.droneId == droneId) == 0)
                throw new DoesntExistException("THis drone is not currently being charged\n");
            else
                listDroneCharges.RemoveAll(x => x.droneId == droneId);


            listDroneCharges.RemoveAll(dc => dc.droneId == droneId);
            XMLTools.SaveListToXMLSerializer(listDroneCharges, DroneChargesPath);

        }
        #endregion
        #endregion

        #region find
        #region findParcel /*did xmlserializer
        public Parcel findParcel(int parcelId) //finds a parcel using its id
        {
            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);



            for (int i = 0; i < listParcels.Count(); i++) //goes over parcel list
            {
                if (listParcels[i].parcelId == parcelId) //if id matches
                {
                    return (listParcels[i]);
                }

            }
            throw new DoesntExistException("The parcel doesn't exist in system");

        }
        #endregion
        #region findCustomer/did xelement
        public Customer findCustomer(int customerId) //finds a customer using its id
        {

            XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
            Customer c = new Customer();
            foreach(var customer in listCustomers.Elements())//goes over customer list
            {
                if (Convert.ToInt32(customer.Element("customerId").Value )== customerId) //if id matches
                {
                    c.customerId = Convert.ToInt32(customer.Element("customerId").Value);
                    c.name = (customer.Element("name").Value);
                    c.Phone= (customer.Element("Phone").Value);
                    c.latitude = Convert.ToDouble(customer.Element("latitude").Value);
                    c.longitude = Convert.ToDouble(customer.Element("longitude").Value);
                    c.password = (customer.Element("password").Value);
                    c.isCustomer = Convert.ToBoolean(customer.Element("isCustomer").Value);
                    c.email= (customer.Element("email").Value);
                    return c;


                }
            }

            throw new DoesntExistException("The customer doesn't exist in system");


        }
        #endregion
        #region findDroneCharge /i think i did xelelemt
        public DroneCharge findDroneCharge(int droneId, int stationId)
        {
            List<DroneCharge> listDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargesPath);



            for (int i = 0; i < listDroneCharges.Count(); i++) //goes over parcel list
            {
                if (listDroneCharges[i].droneId == droneId) //if id matches
                {
                    return (listDroneCharges[i]);
                }

            }
            throw new DoesntExistException("The drone isnt't charging");
            //XElement droneChargeElement = XMLTools.LoadListFromXMLElement(DroneChargesPath);
            //IEnumerable<XElement> list = droneChargeElement.Elements();
            //DroneCharge droneCharge = (from item in droneChargeElement.Elements()
            //                           where int.Parse(droneChargeElement.Element("Drone Id").Value) == droneId && int.Parse(droneChargeElement.Element("Station Id").Value) == stationId

            //                           select new DroneCharge()
            //                           {
            //                               droneId = int.Parse(droneChargeElement.Element("Drone id").Value),
            //                               stationId = int.Parse(droneChargeElement.Element("Station id").Value),
            //                               chargeTime = DateTime.Parse(droneChargeElement.Element("Charge Time").Value)
            //                              // active = bool.Parse(droneChargeElement.Element("Active").Value)


            //                           }).FirstOrDefault();
            //if (droneCharge.droneId != null)
            //    return droneCharge;
            //for (int i = 0; i < DataSource.DroneChargeList.Count(); i++)
            //{
            //    if (DataSource.DroneChargeList[i].droneId == droneId && DataSource.DroneChargeList[i].stationId == stationId) //if id matches
            //    {
            //        return (DataSource.DroneChargeList[i]);
            //    }
            //}

            //  throw new DoesntExistException("The drone or station doesn't exist in system");
        }
        #endregion
        #region findDrone /didxmlserializer
        public Drone findDrone(int droneId) //finds a drone using its id
        {

            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            for (int i = 0; i < listDrones.Count(); i++) //goes over drone list
            {
                if (listDrones[i].droneId == droneId) //if id matches
                {
                    return (listDrones[i]);
                }
            }
            throw new DoesntExistException("The drone doesn't exist in system");
        }
        #endregion
        #region findStation /did the xmlserilizer
        public Station findStation(int stationId) //finds a station using its id
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            for (int i = 0; i < listStations.Count(); i++) //goes over station list
            {
                if (listStations[i].stationId == stationId) //if id matches
                {
                    return (listStations[i]);
                }
            }
            throw new DoesntExistException("The station doesn't exist in system");

        }
        #endregion
        #region findDroneCharge /i think i did xelemnt
        public DroneCharge findDroneCharge(int droneChargeId) //finds a drone charge using its id
        {
            List<DroneCharge> listDroneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargesPath);



            for (int i = 0; i < listDroneCharges.Count(); i++) //goes over parcel list
            {
                if (listDroneCharges[i].droneId == droneChargeId) //if id matches
                {
                    return (listDroneCharges[i]);
                }

            }
            throw new DoesntExistException("The drone isnt't charging");
          
               
        }
        #endregion
        #endregion

        #region getLists
        #region printStationsList /*did xml serializer
        public IEnumerable<Station> printStationsList() //prints list of stations 
        {
            List<Station> listStations = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            foreach (Station item in listStations)
            {
                yield return item;
            }


        }
        #endregion
        #region printDronesList /*did xmlserializer
        public IEnumerable<Drone> printDronesList() //prints list of drone
        {

            List<Drone> listDrones = XMLTools.LoadListFromXMLSerializer<Drone>(DronesPath);
            foreach (Drone item in listDrones)
            {
                yield return item;
            }
        }
        #endregion
        #region printCustomersList /*did xmlserializer
        public IEnumerable<Customer> printCustomersList() //prints customer list
        {
           XElement listCcustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
            Customer c = new Customer();
            return from customer in listCcustomers.Elements()
                   select new Customer()
                   {
                       customerId = Convert.ToInt32(customer.Element("customerId").Value),
                       name = (customer.Element("name").Value),
                       Phone = (customer.Element("Phone").Value),
                       latitude = Convert.ToDouble(customer.Element("latitude").Value),
                       longitude = Convert.ToDouble(customer.Element("longitude").Value),
                       password = (customer.Element("password").Value),
                       isCustomer = Convert.ToBoolean(customer.Element("isCustomer").Value)

                   };

        }
        #endregion
        #region printParcelsList did xmlserializer
        public IEnumerable<Parcel> printParcelsList() //prints parcel list
        {
            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);

            foreach (Parcel item in listParcels)
            {
                yield return item;
            }
        }
        #endregion
        #region printDroneChargeList /i think i did xelement
        public IEnumerable<DroneCharge> printDroneChargeList() //prints DroneCharge list
        {
           
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargesPath);

            foreach (DroneCharge item in droneChargeList/*.Where(s => s.active == true)*/)
            {
                yield return item;
            }

        }
        #endregion
        #endregion

        #region random help
        #region ChargeCapacity //did xmlserializer
        public double[] ChargeCapacity()
        {
            List<double> list = XMLTools.LoadListFromXMLSerializer<double>(configPath);

            double[] arr = new double[7];
            for (int i = 0; i < list.Count(); i++)
            { arr[i] = list[i]; }


            return arr;


        }
        #endregion
        #region attribute xmlserializer did active
        public void attribute(int dID, int pID)//the function attribute parcel to drone
        {
            List<Parcel> listParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelsPath);
            try
            {
                Drone tmpD = getDrone(dID);
                Parcel tmpP = getParcel(pID);
                listParcels.RemoveAll(m => m.parcelId == tmpP.parcelId);   //removing all the data from the place in the list the equal to tmpP id
                tmpP.droneId = tmpD.droneId;        //attribute drones id to parcel 
                tmpP.scheduled = DateTime.Now; //changing the time to be right now
                listParcels.Add(tmpP); //adding to the parcel list tmpP
                XMLTools.SaveListToXMLSerializer(listParcels, ParcelsPath);
            }
            catch (Exception exc)
            {
                throw new DoesntExistException(exc.Message);
            }
        }
        #endregion
        #region checkCustomer /did xelement
        public Customer checkCustomer(string name, string password) //finds a customer using its id
        {
            bool flagExist = false, coorectPassword = false;
            int i = 0, j = 0;
            Customer c = new Customer();
            XElement listCustomers = XMLTools.LoadListFromXMLElement(CustomersPath);
            foreach (var customer in listCustomers.Elements()) //goes over customer list
            {
                i++;
                if ((customer.Element("name").Value) == name)//if id matches
                {
                    j = i;
                    flagExist = true;
                    if ((customer.Element("password").Value) == password)
                    {
                        coorectPassword = true;
                        c.customerId = Convert.ToInt32(customer.Element("customerId").Value);
                        c.name = (customer.Element("name").Value);
                        c.Phone = (customer.Element("Phone").Value);
                        c.latitude = Convert.ToDouble(customer.Element("latitude").Value);
                        c.longitude = Convert.ToDouble(customer.Element("longitude").Value);
                        c.password = (customer.Element("password").Value);
                        c.isCustomer = Convert.ToBoolean(customer.Element("isCustomer").Value);
                        return c;

                    }


                }
            }
            if (!flagExist)
                throw new DoesntExistException("The customer doesn't exist in system");
            if (!coorectPassword)
                throw new DoesntExistException("Incorrect Paswword\n");
            return c;

        }
        #endregion
        #endregion

    }

}