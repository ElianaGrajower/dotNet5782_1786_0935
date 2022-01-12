﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DAL;
using BO;
using DalApi;
using BlApi;




namespace BL
{

    sealed internal class BL : IBL
    {
        #region singelton
        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }
        internal IDal dal = DalFactory.GetDal();
        #endregion

        #region help
        private List<BO.DroneToList> drones;
        public static Random rand = new Random();
        #endregion

        #region helpFunctions
        #region emailValidity
        private bool emailValidity(string email)
        {
            for (int i = 0; i < email.Length; i++)
            {
                if (email[i] == '@')
                    return true;
            }
            return false;
        }
        #endregion
        #region findTheParcel
        //this function finds a parcel based on many different orders of priorities
        private DO.Parcel findTheParcel(BO.weightCategories we, BO.Location a, double battery, DO.Priorities pri)
        {
            double d, x;
            DO.Parcel theParcel = new DO.Parcel();

            BO.Location loc = new BO.Location(30, 35);
            DO.Customer customer = new DO.Customer();
            double far = 1000000;

            var parcels = dal.printParcelsList().Where(p => p.targetId != 0);
            var tempParcel = from item in parcels
                             where item.priority == pri
                             select item;

            foreach (var item in tempParcel)
            {
                //checks if foudn parcel
                customer = dal.getCustomer(item.senderId);
                loc.latitude = customer.latitude;
                loc.longitude = customer.longitude;
                chargeCapacity chargeCapacity = getChargeCapacity();
                var target = dal.getCustomer(item.targetId);
                d = distance(a, loc);
                x = distance(loc, new BO.Location(dal.getCustomer(item.targetId).latitude,
                                                   dal.getCustomer(item.targetId).longitude));
                double fromCusToSta = distance(new BO.Location(target.latitude,
                                                               target.longitude),
                                                                closestStation(new BO.Location(target.latitude,
                                                                target.longitude), false, stationLocationslist()));

                double batteryUse = x * chargeCapacity.chargeCapacityArr[(int)item.weight] +
                                   fromCusToSta * chargeCapacity.chargeCapacityArr[0] + d * chargeCapacity.chargeCapacityArr[0];
                if (d < far && (battery - batteryUse) > 0 && item.scheduled == null
                                                            && weight(we, (BO.weightCategories)item.weight) == true)
                {
                    far = d;
                    theParcel = item;
                    return theParcel;
                }



            }

            //recursion
            if (pri == DO.Priorities.emergency)
                theParcel = findTheParcel(we, a, battery, DO.Priorities.fast);
            //recursion
            if (pri == DO.Priorities.fast)
                theParcel = findTheParcel(we, a, battery, DO.Priorities.regular);
            if (theParcel.parcelId == 0)
                throw new BO.DoesntExistException("ERROR! there is not a parcel that match to the drone ");
            return theParcel;
        }
        #endregion
        #region searchCostumer
        public int searchCustomer(string userName) //recieves the name of a customer and returns its id
        {
            int id = getCustomersList().Where(c => c.customerName == userName).Select(s => s.customerId).FirstOrDefault();
            return id;
        }
        #endregion
        #region isEmployee
        public bool isEmployee(string userName, string password)
        {
            int id = getCustomersList().Where(c => c.customerName == userName).Select(s => s.customerId).FirstOrDefault();
            if (id == null)
                throw new BO.DoesntExistException("this userName doest exist\n");
            if (getCustomer(id).password != password)
                throw new BO.InvalidInputException("the password is incorrect\n");
            if (getCustomer(id).isCustomer == true)
                return false;
            return true;
        }
        #endregion
        #region passwordProtection
        private bool passwordProtection(string password)
        {
            bool flag = false;
            if (password.Length < 8)
                return false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= 65 && password[i] <= 90)
                {
                    flag = true;
                    break;
                }

            }
            if (!flag)
                return false;
            flag = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= 97 && password[i] <= 122)
                {
                    flag = true;
                    break;
                }

            }
            if (!flag)
                return false;
            flag = false;
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] >= 48 && password[i] <= 57)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
                return false;
            return true;
        }
        #endregion
        #region getChargeCapacity
        //this function returns an arr of chargecapacity
        public chargeCapacity getChargeCapacity()
        {

            double[] arr = dal.ChargeCapacity();
            var chargeCapacity = new chargeCapacity
            {
                pwrAvailable = arr[0],
                pwrLight = arr[1],
                pwrAverge = arr[2],
                pwrHeavy = arr[3],
                pwrRateLoadingDrone = arr[4],
                chargeCapacityArr = arr
            };
            return chargeCapacity;
        }
        #endregion
        #region getUnvailablechargeSlots
        private int getUnvailablechargeSlots(int stationId)
        {
            int count = dal.printDroneChargeList().Where(c => c.stationId == stationId).Count();
            return count;
        }
        #endregion
        #region minBatteryRequired
        //this function checks how much battery is needed for the drone to get from point a to point b
        private int minBatteryRequired(int droneId)
        { //gets drone 
            var drone = getDrone(droneId);
            //if drone available
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = closestStation(drone.location, false, stationLocationslist());
                return (int)(getChargeCapacity().chargeCapacityArr[(int)getChargeCapacity().pwrAvailable]
                                                * distance(drone.location, location));
            }
            //if drone out for delivery
            if (drone.droneStatus == DroneStatus.delivery)
            {
                DO.Parcel parcel = dal.getParcel(drone.parcel.parcelId);
                //if wasnt picked up and updates all the info
                if (parcel.pickedUp == null)
                {
                    int minValue;
                    BO.Customer sender = getCustomer(parcel.senderId);
                    var target = getCustomer(parcel.targetId);
                    double droneToSender = distance(drone.location, sender.location);
                    minValue = (int)(getChargeCapacity().chargeCapacityArr[(int)getChargeCapacity().pwrAvailable] * droneToSender);
                    double senderToTarget = distance(sender.location, target.location);
                    minValue += (int)(getChargeCapacity().chargeCapacityArr[(int)parcel.weight] * senderToTarget);
                    Location baseStationlocation = closestStation(target.location, false, stationLocationslist());
                    double targetToCharge = distance(target.location, baseStationlocation);
                    minValue += (int)(getChargeCapacity().chargeCapacityArr[(int)getChargeCapacity().pwrAvailable] * targetToCharge);
                    if (minValue >= 0)
                        return minValue;
                }
                //if wasnt delievred updates all the info
                if (parcel.delivered == null)
                {
                    int minValue;
                    var sender = getCustomer(parcel.senderId);
                    var target = getCustomer(parcel.targetId);
                    double senderToTarget = distance(sender.location, target.location);
                    int batteryUsage = (int)parcel.weight;
                    minValue = (int)(getChargeCapacity().chargeCapacityArr[batteryUsage] * senderToTarget);
                    Location baseStationlocation = closestStation(target.location, false, stationLocationslist());
                    double targetToCharge = distance(target.location, baseStationlocation);
                    minValue += (int)(getChargeCapacity().chargeCapacityArr[(int)getChargeCapacity().pwrAvailable] * targetToCharge);
                    if (minValue >= 0)
                        return minValue;
                }
            }
            return 90;
        }
        #endregion
        #region stationLocationslist
        //returns a list of all the the station locations
        private List<Location> stationLocationslist()
        {
            List<Location> locations = new List<Location>();
            foreach (var station in getStations())
            {
                //adds location of current station
                locations.Add(new Location(station.location.latitude, station.location.longitude));

            }
            return locations;
        }
        #endregion
        #region onlyDigits
        //this function ensures that  enteres is only numbers
        private bool onlyDigits(char x)
        {
            //ensures that in range of ascii value of numbers
            if (48 <= x && x <= 57)
                return true;
            return false;

        }
        #endregion
        #region deg2rad
        //this function converts degree to radianim
        private static double deg2rad(double val)
        {
            return (Math.PI / 180) * val;
        }
        #endregion
        #region distance
        //calculates distance between two location based on a mathematical calculation for coordinates
        public double distance(BO.Location l1, BO.Location l2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(l2.latitude - l1.latitude);  // deg2rad below
            var dLon = deg2rad(l2.longitude - l1.longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(l1.latitude)) * Math.Cos(deg2rad(l2.latitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // distance in km
            return d;
        }
        #endregion
        #region closestStation
        //finds closestStation with chargeSlots
        private Location closestStation(Location currentLocation, bool withChargeSlots, List<Location> l)//the function could also be used to check in addtion if the charge slots are more then 0
        {

            var locations = l;
            Location location = locations[0];
            //calculates distance
            double d = distance(locations[0], currentLocation);
            for (int i = 1; i < locations.Count; i++)
            {
                //if has chatgeslots
                if (withChargeSlots)
                {
                    var station = getStations().ToList().Find(x => x.location.longitude == locations[i].longitude
                                                               && x.location.latitude == locations[i].latitude);

                    //checks distance
                    if (distance(locations[i], currentLocation) < d && station.chargeSlots > 0)
                    {
                        d = distance(locations[i], currentLocation);
                    }
                    else
                    {
                        if (locations.Count() == 0)
                            throw new Exception("there are no stations with available charge slots");
                        locations.RemoveAt(i);
                        closestStation(currentLocation, withChargeSlots, locations);
                    }

                }
                else
                {
                    if (distance(locations[i], currentLocation) < d)
                    {
                        location = locations[i];
                        d = distance(locations[i], currentLocation);
                    }

                }

            }
            return location;
        }
        #endregion
        #region getDroneBattery
        //RETURNS BATTERY OF DRONE
        private double getDroneBattery(int droneId)
        {

            var drone = drones.ToList().Where(drone => drone.droneId == droneId).FirstOrDefault();
            if (drone == null)
                throw new BO.DoesntExistException("Doesnt exist");

            return drone.battery;


        }
        #endregion
        #region findStation
        //this function finds a station based on coordinates that it recives
        private int findStation(Location location)
        {
            var station = dal.printStationsList().Where(s => s.longitude == location.longitude && s.latitude == location.latitude);
            if (station.Count() == 0)
                throw new BO.DoesntExistException("station with these coordinates doesnt exist\n");
            var stationId = station.First().stationId;
            return stationId;
        }
        #endregion
        #region weight
        //returns the weight cattegory
        private bool weight(BO.weightCategories dr, BO.weightCategories pa)
        {
            if (dr == BO.weightCategories.heavy)
                return true;
            if (dr == BO.weightCategories.average && (pa == BO.weightCategories.average || pa == BO.weightCategories.light))
                return true;
            if (dr == BO.weightCategories.light && pa == BO.weightCategories.light)
                return true;
            return false;
        }
        #endregion
        #region indexOfChargeCapacity
        //returns what kinds of parcel it can hold
        private int indexOfChargeCapacity(DO.weightCategories w)
        {
            if (w == DO.weightCategories.light)
                return 1;
            if (w == DO.weightCategories.heavy)
                return 3;
            if (w == DO.weightCategories.average)
                return 2;

            return 0;

        }
        #endregion

        #endregion

        #region lists
        #region getDronesList
        //returns dronelist
        public List<BO.DroneToList> getDronesList()
        {
            List<BO.DroneToList> drone = new List<BO.DroneToList>();
            try
            {
                //calls get drone to convert to ibl

                foreach (var d in drones)
                {
                    var temp = dal.getDrone(d.droneId);
                    // if (temp.active)
                    drone.Add(returnsDrone(d.droneId));
                }
            }
            catch (ArgumentException) { throw new BO.DoesntExistException(); }
            return drone;
        }
        #endregion  
        #region getCustomersList
        //returns customer list
        public List<BO.CustomerToList> getCustomersList()
        {
            List<BO.CustomerToList> customer = new List<BO.CustomerToList>();
            try
            {
                var customerDal = dal.printCustomersList().ToList();
                foreach (var c in customerDal)
                {
                    var newCustomer = getCustomer(c.customerId);
                    var temp = new BO.CustomerToList()
                    {
                        customerId = c.customerId,
                        customerName = newCustomer.name,
                        phone = newCustomer.phone,
                        parcelsdelivered = newCustomer.parcelsdelivered
                                           .Where(s => s.parcelStatus == ParcelStatus.delivered).Count(),
                        undeliveredParcels = newCustomer.parcelsdelivered
                                             .Where(s => s.parcelStatus != ParcelStatus.delivered).Count(),
                        recievedParcel = newCustomer.parcelsOrdered
                                         .Where(s => s.parcelStatus == ParcelStatus.delivered).Count(),
                        transitParcel = newCustomer.parcelsOrdered
                                       .Where(s => s.parcelStatus != ParcelStatus.delivered).Count(),
                        isCustomer = newCustomer.isCustomer,


                    };
                    customer.Add(temp);
                }
            }
            catch (ArgumentException) { throw new BO.DoesntExistException(); }
            return customer;


        }
        #endregion
        #region getUsersList
        //returns customer list
        public List<BO.CustomerToList> getUsersList()
        {

            try
            {
                var list = getCustomersList().Where(c => c.isCustomer == true);
                if (list.FirstOrDefault() != null)
                    return list.ToList();

            }
            catch (ArgumentException) { throw new BO.DoesntExistException("No customers exist"); }
            catch (Exception exc) { throw new BO.DoesntExistException("No customers exist"); }
            return null;
        }
        #endregion
        #region getEmployeesList
        //returns employee list
        public List<BO.CustomerToList> getEmployeesList()
        {
            try
            {
                var list = getCustomersList().Where(c => c.isCustomer == false);
                if (list.FirstOrDefault() != null)
                    return list.ToList();

            }
            catch (ArgumentException) { throw new BO.DoesntExistException("No customers exist"); }
            catch (Exception exc) { throw new BO.DoesntExistException("No customers exist"); }
            return null;
        }
        #endregion
        #region getParcelsList
        //returns parcel list
        public List<BO.ParcelToList> getParcelsList()
        {
            List<BO.ParcelToList> parcel = new List<BO.ParcelToList>();
            try
            {
                //calls getParcel to convert
                var parcelDal = dal.printParcelsList().ToList();
                foreach (var p in parcelDal)
                {
                    var newParcel = getParcel(p.parcelId);
                    var temp = new BO.ParcelToList()
                    {
                        parcelId = p.parcelId,
                        sendername = newParcel.sender.customerName,
                        recivername = newParcel.target.customerName,
                        weight = newParcel.weight,
                        priority = newParcel.priority

                    };
                    if (newParcel.delivered != null)
                        temp.parcelStatus = ParcelStatus.delivered;
                    else
                    {
                        if (newParcel.pickedUp != null)
                            temp.parcelStatus = ParcelStatus.pickedUp;
                        else
                        {
                            if (newParcel.scheduled != null)
                                temp.parcelStatus = ParcelStatus.matched;
                            else
                                temp.parcelStatus = ParcelStatus.created;
                        }
                    }
                    parcel.Add(temp);
                }
            }
            catch (ArgumentException) { throw new BO.DoesntExistException(); }
            return parcel;


        }
        #endregion
        #region getStationsList
        //this function returns a list of all the stations
        public List<BO.StationToList> getStationsList()
        {
            List<BO.StationToList> stations = new List<BO.StationToList>();
            try
            {//makes temp list
                //calls on getlist of stations function from dal
                var stationsDal = dal.printStationsList().ToList();
                //in each link calls on get station which convers it to ibl station and adds it to temp list
                foreach (var s in stationsDal)
                {
                    var temp = new BO.StationToList()
                    {
                        stationId = s.stationId,
                        name = getStation(s.stationId).name,
                        numberOfSlotsInUse = getStation(s.stationId).numberOfSlotsInUse,
                        numberOfAvailableSlots = getStation(s.stationId).chargeSlots - getUnvailablechargeSlots(s.stationId)


                    };
                    stations.Add(temp);
                }
            }
            catch (ArgumentException) { throw new BO.DoesntExistException(); }
            //returns temp list
            return stations;
        }
        #endregion getStationsList
        #region allDrones
        public IEnumerable<DroneToList> allDrones(Func<DroneToList, bool> predicate = null)
        {
            if (predicate == null)
            {
                return drones.Take(drones.Count).ToList();
            }
            return drones.Where(predicate).ToList();
        }
        #endregion
        #region allStations

        public IEnumerable<BO.StationToList> allStations(Func<BO.StationToList, bool> predicate = null)
        {
            var station = getStationsList();
            if (predicate == null)
            {


                return station.Take(station.Count).ToList();
            }
            return station.Where(predicate).ToList();
        }
        #endregion
        #region allParcels
        public IEnumerable<BO.ParcelToList> allParcels(Func<BO.ParcelToList, bool> predicate = null)
        {
            var parcel = getParcelsList();
            if (predicate == null)
            {
                return parcel.Take(parcel.Count).ToList();
            }
            return parcel.Where(predicate).ToList();
        }
        #endregion
        #region allCustomers
        public IEnumerable<BO.CustomerToList> allCustomers(Func<BO.CustomerToList, bool> predicate = null)
        {
            var customer = getCustomersList();
            if (predicate == null)
            {
                return customer.Take(customer.Count).ToList();
            }
            return customer.Where(predicate).ToList();
        }
        #endregion
        #region getStations
        //this function returns a list of stations
        public List<BO.Station> getStations()
        {
            List<BO.Station> stations = new List<BO.Station>();
            try
            {//gets list of dal stations
                var stationsDal = dal.printStationsList().ToList();
                //in each link calls on get statio to conver to ibl station and adds to temp list
                foreach (var s in stationsDal)
                { stations.Add(getStation(s.stationId)); }
            }
            catch (ArgumentException) { throw new BO.DoesntExistException(); }
            return stations;
        }
        #endregion
        #endregion

        public IEnumerable<int> confirmDelivery(int customerId)
        {
            var customer = getCustomer(customerId);
            var parcelsList = getParcelsList().Where(p=>p.recivername==customer.name)
                                     .Where(p=>p.parcelStatus==ParcelStatus.delivered)
                                                                .Select(p=>p.parcelId);
            return parcelsList;
            
        }
        public IEnumerable<int> confirmPickUp(int customerId)
        {
            var customer = getCustomer(customerId);
            var parcelsList = getParcelsList().Where(p => p.sendername == customer.name)
                                   .Where(p => p.parcelStatus == ParcelStatus.delivered)
                                                                .Select(p => p.parcelId);
            return parcelsList;

        }

        #region add
        #region addDrone
        //this function adds a drone
        public void addDrone(BO.Drone droneToAdd, int stationId)
        {
            //checks validity of input
            if (droneToAdd.droneId <= 0)
                throw new BO.InvalidInputException("drone id not valid- must be a posittive\n");
            if (droneToAdd.maxWeight != BO.weightCategories.light && droneToAdd.maxWeight != BO.weightCategories.average
                                                                    && droneToAdd.maxWeight != BO.weightCategories.heavy)
                throw new BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");
            if (droneToAdd.battery == 0)
                droneToAdd.battery = rand.Next(20, 40);
            if (droneToAdd.droneStatus == 0)
            { droneToAdd.droneStatus = DroneStatus.maintenance; }
            try
            {
                //checks if station exists
                double Stationlatitude = dal.findStation(stationId).latitude;
                double Stationlongitude = dal.findStation(stationId).longitude;
                droneToAdd.location = new Location(Stationlatitude, Stationlongitude);
            }
            catch (DO.DoesntExistException exc)
            {
                throw new BO.DoesntExistException(exc.Message);
            }
            // builds new dronetolist
            DroneToList dtl = new DroneToList();
            dtl.droneId = droneToAdd.droneId;
            dtl.model = droneToAdd.model;
            dtl.weight = droneToAdd.maxWeight;
            dtl.battery = droneToAdd.battery;
            dtl.droneStatus = droneToAdd.droneStatus;
            dtl.location = new Location(30, 35);
            dtl.location.latitude = dal.findStation(stationId).latitude;
            dtl.location.longitude = dal.findStation(stationId).longitude;
            //build DO.drone
            DO.Drone newDrone = new DO.Drone()
            {
                droneId = droneToAdd.droneId,
                model = droneToAdd.model,
                maxWeight = (DO.weightCategories)((int)droneToAdd.maxWeight)
            };
            try
            {
                //updates the list that contain info
                dal.AddDrone(newDrone);
                if (drones.Count(x => x.droneId == droneToAdd.droneId) == 0)//need to somehow use active here
                    drones.Add(dtl);
                DO.DroneCharge dc = new DO.DroneCharge { droneId = droneToAdd.droneId, stationId = stationId, };
                var tempstation = getStation(stationId);
                if (drones.Where(d => dtl.droneId == d.droneId).Count() == 0)
                    tempstation.decreaseChargeSlots();
                updateStation(tempstation.stationId, tempstation.chargeSlots, "");

                dal.AddDroneCharge(dc);
            }
            catch (BO.AlreadyExistsException exc)
            {
                throw exc;
            }
            catch (DO.AlreadyExistException exc)
            {
                throw new BO.AlreadyExistsException(exc.Message);
            }
        }
        #endregion
        #region addCustomer
        //adds customer
        public void addCustomer(BO.Customer customertoAdd)
        {

            customertoAdd.parcelsOrdered = new List<ParcelinCustomer>();
            customertoAdd.parcelsdelivered = new List<ParcelinCustomer>();
            //chelcs validity of input
            if (customertoAdd.location.latitude < 29.207 || customertoAdd.location.latitude > 33.207)
                throw new BO.InvalidInputException("latitude is out of range\n");
            if (customertoAdd.location.longitude < 34.572 || customertoAdd.location.longitude > 35.572)
                throw new BO.InvalidInputException("longitude is out of range\n");
            if (customertoAdd.customerId > 999999999 || customertoAdd.customerId < 100000000)
                throw new BO.InvalidInputException("customer id not valid\n");
            if (!customertoAdd.phone.All(onlyDigits))
                throw new BO.InvalidInputException("customer phone not valid- must contain only numbers\n");
            if (!passwordProtection(customertoAdd.password))
                throw new BO.InvalidInputException
                        ("Password must be at least eight digits and contain at least one uppercase letter and one digit\n");
            if (!emailValidity(customertoAdd.email))
                throw new BO.InvalidInputException("Invalid email address\n");

            //builds idal customer
            DO.Customer newCustomer = new DO.Customer()
            {
                customerId = customertoAdd.customerId,
                name = customertoAdd.name,
                Phone = customertoAdd.phone,
                email = customertoAdd.email,
                latitude = customertoAdd.location.latitude,
                longitude = customertoAdd.location.longitude,
                password = customertoAdd.password,
                isCustomer = customertoAdd.isCustomer
            };
            try
            {
                //adss it to dal customerlist

                dal.AddCustomer(newCustomer);
            }
            catch (AlreadyExistException exc)
            {
                throw new AlreadyExistsException(exc.Message);
            }
        }
        #endregion
        #region addStation
        //adds station
        public void addStation(BO.Station StationtoAdd)
        {

            StationtoAdd.dronesAtStation = new List<DroneInCharging>();
            //checks validity of input
            if (StationtoAdd.stationId <= 0)
                throw new BO.InvalidInputException("station id not valid- must be a posittive\n");//check error
            if (StationtoAdd.location.latitude < 29.207 || StationtoAdd.location.latitude > 33.207)
                throw new BO.InvalidInputException("latitude is out of range\n");
            if (StationtoAdd.location.longitude < 34.572 || StationtoAdd.location.longitude > 35.572)
                throw new BO.InvalidInputException("longitude is out of range\n");
            if (StationtoAdd.chargeSlots <= 0)
                throw new BO.InvalidInputException("invalid amount of chargeSlots- must be a positive number");


            //builds dal station
            DO.Station newStation = new DO.Station()
            {
                stationId = StationtoAdd.stationId,
                name = StationtoAdd.name,
                latitude = StationtoAdd.location.latitude,
                longitude = StationtoAdd.location.longitude,
                chargeSlots = StationtoAdd.chargeSlots,
                // active=true
            };
            try
            {
                //adds it do dal station list
                dal.AddStation(newStation);
            }
            catch (DO.AlreadyExistException exc)
            {
                throw new BO.AlreadyExistsException(exc.Message);
            }
        }
        #endregion
        #region addParcel
        //adds parcel
        public int addParcel(BO.Parcel parcelToAdd)
        {

            //checks validity of input
            if (!(parcelToAdd.sender.customerId >= 10000000 && parcelToAdd.sender.customerId <= 1000000000))
                throw new BO.InvalidInputException("the id number of sender of the the parcel is invalid\n");
            if (!(parcelToAdd.target.customerId >= 10000000 && parcelToAdd.target.customerId <= 1000000000))
                throw new BO.InvalidInputException("the id   number of receiver of the parcel is invalid\n");
            if (!(parcelToAdd.weight >= (BO.weightCategories)1 && parcelToAdd.weight <= (BO.weightCategories)3))
                throw new BO.InvalidInputException("the given weight is not valid\n");
            if (!(parcelToAdd.priority >= (BO.Priorities)0 && parcelToAdd.priority <= (BO.Priorities)3))
                throw new BO.InvalidInputException("the given priority is not valid\n");
            //build idalparcel
            DO.Parcel parcelDo = new DO.Parcel();
            parcelDo.parcelId = dal.getParcelId();
            parcelDo.senderId = parcelToAdd.sender.customerId;
            parcelDo.targetId = parcelToAdd.target.customerId;
            parcelDo.weight = (DO.weightCategories)parcelToAdd.weight;
            parcelDo.priority = (DO.Priorities)parcelToAdd.priority;
            parcelDo.requested = DateTime.Now;
            parcelDo.scheduled = null;
            parcelDo.pickedUp = null;
            parcelDo.delivered = null;
            parcelDo.droneId = 0;

            try
            {
                //adds it to dal list of parcels
                dal.AddParcel(parcelDo);
                return parcelDo.parcelId;
            }
            catch (DO.AlreadyExistException exp)
            {
                throw new AlreadyExistException(exp.Message);
            }
        }
        #endregion
        #endregion

        #region delete
        #region deleteStation
        //delets station
        public void deleteStation(int stationId)
        {
            try
            {
                //Calls on delete fun from dal
                dal.deleteStation(stationId);
            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region deleteParcel
        //deletes parcel
        public void deleteParcel(int parcelId)
        {
            try
            {
                //calls on delete func from dal
                dal.deleteParcel(parcelId);
            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region deleteCustomer
        //delets customer
        public void deleteCustomer(int customerId)
        {
            try
            {
                //calls on delete func from dal
                dal.deleteCustomer(customerId);
            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region deleteDrone
        //delets drone 
        public void deleteDrone(int droneId)
        {
            try
            {
                dal.deleteDrone(droneId);
                drones.RemoveAll(d => d.droneId == droneId);
            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #endregion

        #region get
        #region getCustomer
        //returns customer
        public BO.Customer getCustomer(int customerId)
        {
            try
            {
                //gets customer from dal list
                DO.Customer temp = dal.getCustomer(customerId);
                var parcelList = dal.printParcelsList();
                //  var boParcelList = getParcelsList();
                //starts building bo customer
                BO.Customer customer = new BO.Customer()
                {
                    customerId = temp.customerId,
                    name = temp.name,
                    phone = temp.Phone,
                    password = temp.password,
                    isCustomer = temp.isCustomer,
                    location = new Location(temp.latitude, temp.longitude)
                    {
                        latitude = temp.latitude,
                        longitude = temp.longitude,

                    },
                    parcelsOrdered = parcelList.Where(parcel => parcel.targetId == customerId)
                                                           .Select(Parcel => new ParcelinCustomer()

                                                           {
                                                               parcelId = Parcel.parcelId,
                                                               weight = (BO.weightCategories)((int)Parcel.weight),
                                                               priority = (BO.Priorities)((int)Parcel.priority),
                                                               parcelStatus = getParcelsList().Where(p => p.parcelId == Parcel.parcelId).First().parcelStatus,
                                                               customerInParcel = new CustomerInParcel()
                                                               {
                                                                   customerId = Parcel.senderId,
                                                                   customerName = dal.getCustomer(Parcel.senderId).name
                                                               }
                                                           }),

                    parcelsdelivered = parcelList.Where(parcel => parcel.senderId == customerId)
                                                             .Select(Parcel => new ParcelinCustomer()

                                                             {
                                                                 parcelId = Parcel.parcelId,
                                                                 weight = (BO.weightCategories)((int)Parcel.weight),
                                                                 priority = (BO.Priorities)((int)Parcel.priority),
                                                                 parcelStatus = getParcelsList().Where(p => p.parcelId == Parcel.parcelId).First().parcelStatus,
                                                                 customerInParcel = new CustomerInParcel()
                                                                 {
                                                                     customerId = Parcel.targetId,
                                                                     customerName = dal.getCustomer(Parcel.targetId).name
                                                                 }

                                                             }),
                    email = temp.email

                };

                return customer;

            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }
            catch (DO.DoesntExistException exc)
            {
                throw new BO.DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region getParcel
        //returns parcle
        public BO.Parcel getParcel(int parcelsId)
        {

            try
            {
                //gets parcel
                DO.Parcel temp = dal.getParcel(parcelsId);
                //builds a parcel
                BO.Parcel parcel = new BO.Parcel()
                {
                    parcelId = parcelsId,
                    sender = new CustomerInParcel()
                    {
                        customerId = temp.senderId,
                        customerName = dal.getCustomer(temp.senderId).name
                    },
                    target = new CustomerInParcel()
                    {
                        customerId = temp.targetId,
                        customerName = dal.getCustomer(temp.targetId).name
                    },
                    weight = (BO.weightCategories)((int)temp.weight),
                    priority = (BO.Priorities)((int)temp.priority),
                    requested = temp.requested,
                    scheduled = temp.scheduled,
                    pickedUp = temp.pickedUp,
                    delivered = temp.delivered,
                    //builds a drone in parcel
                    drone = new DroneInParcel()
                };

                //fills the drone in parcel
                parcel.drone.droneId = temp.droneId;
                try
                {
                    if (temp.droneId != 0)
                        parcel.drone.battery = getDroneBattery(temp.droneId);
                    else
                        parcel.drone.battery = 0;
                }
                catch (DO.DoesntExistException exc)
                {
                    throw new BO.DoesntExistException(exc.Message);
                }
                catch (BO.DoesntExistException exc)
                {
                    throw new BO.DoesntExistException(exc.Message);
                }
                if (temp.droneId != 0 && temp.delivered == null)
                {
                    var drone = getDrone(temp.droneId);
                    parcel.drone.location = new Location(drone.location.latitude, drone.location.longitude);
                    parcel.drone.droneId = drone.droneId;
                }
                else
                {
                    parcel.drone.location = new Location(30, 35);
                }

                return parcel;

            }
            catch (BO.DoesntExistException exc)
            {
                throw new BO.DoesntExistException(exc.Message);
            }
        }
        #endregion
        #region getDrone
        //returns drone
        public BO.Drone getDrone(int id)
        {
            //ensures drone exists
            var drn = drones.Find(x => x.droneId == id);
            if (drn == null)
                throw new BO.DoesntExistException("The drone doesn't exist in system");
            //build ibl drone
            BO.Drone d = new BO.Drone();
            d.droneId = drn.droneId;
            d.model = drn.model;
            d.maxWeight = drn.weight;
            d.droneStatus = drn.droneStatus;
            d.battery = drn.battery;
            d.location = new BO.Location(30, 35);
            d.location = drn.location;
            BO.ParcelInTransit pt = new BO.ParcelInTransit();
            if (drn.droneStatus == BO.DroneStatus.delivery && d.droneId != 0)
            {
                pt.parcelId = drn.parcelId;
                DO.Parcel p = new DO.Parcel();
                try
                {
                    p = dal.getParcel(drn.parcelId);
                }
                catch (Exception)
                {
                    throw new BO.DoesntExistException("The parcel doesn't exist in system");
                }
                if (p.requested == null)
                    pt.parcelStatus = false;
                else
                if (p.pickedUp == null)
                    pt.parcelStatus = false;
                else
                    if (p.delivered == null)
                    pt.parcelStatus = true;
                else
                    pt.parcelStatus = true;
                var newSender = getCustomer(p.senderId);
                var newTarget = getCustomer(p.targetId);
                //pt.parcelId = p.parcelId;
                pt.priority = (BO.Priorities)p.priority;
                pt.weight = (BO.weightCategories)p.weight;
                pt.sender = new BO.CustomerInParcel();
                pt.sender.customerId = newSender.customerId;
                pt.sender.customerName = newSender.name;
                pt.target = new BO.CustomerInParcel();
                pt.target.customerId = newTarget.customerId;
                pt.target.customerName = newTarget.name;
                DO.Customer sender = dal.getCustomer(p.senderId);
                DO.Customer target = dal.getCustomer(p.targetId);
                pt.pickupLocation = new BO.Location(sender.latitude, sender.longitude);
                pt.targetLocation = new BO.Location(target.latitude, target.longitude);
                pt.distance = distance(pt.pickupLocation, pt.targetLocation);
                d.parcel = new BO.ParcelInTransit();
                d.parcel = pt;
            }
            return d;
        }
        #endregion
        #region returnsDrone
        //returns dronetolist
        public BO.DroneToList returnsDrone(int id)
        {
            DroneToList droneBo = new DroneToList();
            try
            {
                //gets drone form dal
                DO.Drone droneDo = dal.getDrone(id);
                DroneToList drone = drones.ToList().Find(d => d.droneId == id);
                //gets info
                droneBo.droneId = droneDo.droneId;
                droneBo.model = drone.model;
                droneBo.weight = drone.weight;
                droneBo.location = drone.location;
                droneBo.battery = drone.battery;
                droneBo.droneStatus = drone.droneStatus;
                droneBo.numOfParcelsdelivered = drone.numOfParcelsdelivered;
                droneBo.numOfParcelsdelivered = dal.printParcelsList().Count(x => x.droneId == droneBo.droneId);
                int parcelId = dal.printParcelsList().ToList()
                              .Find(x => x.droneId == droneBo.droneId && x.delivered == null).parcelId;
                droneBo.parcelId = parcelId;

            }
            catch (ArgumentNullException exp)
            {
                throw new BO.DoesntExistException(" \n");
            }
            catch (DO.AlreadyExistException exp)
            {
                throw new BO.DoesntExistException(exp.Message);
            }
            return droneBo;
        }
        #endregion
        #region getStation
        //this function returns a station
        public BO.Station getStation(int stationId)
        {
            try
            {
                BO.Station station = new BO.Station();
                //gets all info it can from dal.stationlist
                DO.Station tempStation = dal.getStation(stationId);
                station.stationId = tempStation.stationId;
                station.name = tempStation.name;
                station.location = new Location(tempStation.latitude, tempStation.longitude);
                station.chargeSlots = tempStation.chargeSlots;
                station.numberOfSlotsInUse = getUnvailablechargeSlots(tempStation.stationId);
                //finds the rest of the info from dronecharging ist
                try
                {
                    station.dronesAtStation = dal.printDroneChargeList().Where(item => item.stationId == stationId)
                    .Select(drone => new DroneInCharging()
                    {
                        chargeTime = drone.chargeTime,
                        chargeTillNow = (DateTime.Now - drone.chargeTime).TotalMinutes,
                        droneId = drone.droneId,
                        battery = getDroneBattery(drone.droneId)
                    }).ToList();
                    return station;
                }
                catch (DO.DoesntExistException exc)
                { throw new BO.DoesntExistException(exc.Message); }
            }
            catch (BO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion

        #endregion

        #region BL
        BL()
        {


            drones = new List<BO.DroneToList>();
            bool flag = false;
            Random rnd = new Random();
            double minBatery = 17;
            //gets lists of drones
            IEnumerable<DO.Drone> d = dal.printDronesList();
            //gets list of parcel
            IEnumerable<DO.Parcel> parcels = dal.printParcelsList();
            chargeCapacity chargeCapacity = getChargeCapacity();
            //goes over every drone in the list
            foreach (var item in d)
            {
                //builds a drone to list
                BO.DroneToList drt = new DroneToList();
                drt.droneId = item.droneId;
                drt.model = item.model;
                drt.weight = (BO.weightCategories)(int)item.maxWeight;
                drt.numOfParcelsdelivered = dal.printParcelsList().Count(x => x.droneId == drt.droneId);
                int parcelId = dal.printParcelsList().ToList().FirstOrDefault(x => x.droneId == drt.droneId
                                                                              && x.delivered == null).parcelId;
                drt.parcelId = parcelId;
                var baseStationLocations = stationLocationslist();
                //goes over every parcel in list
                foreach (var pr in parcels)
                {

                    //if not yet delivered updates info
                    if (pr.droneId == item.droneId)
                    {
                        DO.Customer sender = dal.getCustomer(pr.senderId);
                        DO.Customer target = dal.getCustomer(pr.targetId);
                        BO.Location senderLocation = new Location(sender.latitude, sender.longitude);
                        BO.Location targetLocation = new Location(target.latitude, target.longitude);
                        drt.droneStatus = DroneStatus.delivery;
                        var newSenderLocation = closestStation(senderLocation, false, baseStationLocations);
                        var newTargetLocation = closestStation(targetLocation, false, baseStationLocations);
                        if (pr.pickedUp == null && pr.scheduled != null)
                        {
                            drt.location = new Location(newSenderLocation.latitude, newSenderLocation.longitude);
                            minBatery = distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
                            minBatery += distance(senderLocation, targetLocation) *
                                           chargeCapacity.chargeCapacityArr[(int)pr.weight];
                            minBatery += distance(targetLocation,
                                                 new Location(newTargetLocation.latitude, newTargetLocation.longitude))
                                                 * chargeCapacity.chargeCapacityArr[0];
                        }
                        if (pr.pickedUp != null && pr.delivered == null)
                        {

                            drt.location = senderLocation;
                            minBatery = distance(targetLocation, new Location(newTargetLocation.latitude, newTargetLocation.longitude))
                                                                              * chargeCapacity.chargeCapacityArr[0];
                            minBatery += distance(drt.location, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
                        }
                        if (pr.pickedUp != null && pr.delivered != null)
                        {

                            drt.location = targetLocation;
                            minBatery = distance(targetLocation, new Location(newTargetLocation.latitude, newTargetLocation.longitude))
                                                               * chargeCapacity.chargeCapacityArr[0];
                            minBatery += distance(drt.location, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.weight];
                        }
                        if (minBatery > 100) { minBatery = 100; }
                        if (minBatery < 17) { minBatery = 17; }
                        drt.battery = rnd.Next((int)minBatery, 101);
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    int temp = rnd.Next(1, 3);
                    if (temp == 1)
                        drt.droneStatus = BO.DroneStatus.available;
                    else
                        drt.droneStatus = BO.DroneStatus.maintenance;
                    if (drt.droneStatus == BO.DroneStatus.maintenance)
                    {
                        int r = rnd.Next(0, dal.printStationsList().Count()), i = 0;
                        DO.Station s = new DO.Station();
                        foreach (var ite in dal.printStationsList())
                        {
                            s = ite;
                            if (i == r)
                                break;
                            i++;
                        }
                        DO.DroneCharge DC = new DO.DroneCharge
                        { droneId = drt.droneId, stationId = s.stationId, chargeTime = DateTime.Now };
                        dal.AddDroneCharge(DC);
                        drt.location = new Location(s.latitude, s.longitude);
                        drt.battery = rnd.Next(1, 21); // 100/;
                        if (drt.location == null)
                            drt.location = new Location(29.208, 34.57);
                    }
                    else
                    {
                        List<DO.Customer> lst = new List<DO.Customer>();
                        foreach (var pr in parcels)
                        {
                            if (pr.delivered != null)
                                lst.Add(dal.getCustomer(pr.targetId));
                        }
                        if (lst.Count == 0)
                        {
                            foreach (var pr in dal.printCustomersList())
                            {

                                lst.Add(pr);
                            }
                        }
                        int l = rnd.Next(0, lst.Count());

                        drt.location = new Location(lst[l].latitude, lst[l].longitude);
                        Location Location1 = new Location(lst[l].latitude, lst[l].longitude);
                        var tempLoc = closestStation(Location1, false, baseStationLocations);
                        minBatery += distance(drt.location, new Location
                                                  (tempLoc.longitude, tempLoc.latitude)) * chargeCapacity.chargeCapacityArr[0];
                        if (minBatery > 100) { minBatery = 100; }
                        if (minBatery < 17)
                        { minBatery = 17; }
                        drt.battery = rnd.Next((int)minBatery, 101);

                    }

                }
                if (drt.parcelId == 0)
                    drt.droneStatus = DroneStatus.available;
                drones.Add(drt);






            }


        }
        #endregion

        #region update
        #region UpdateDronename
        //updates drone name
        public void UpdateDronename(int droneId, string dmodel)
        {
            //checks id drone exits
            int dIndex = drones.FindIndex(x => x.droneId == droneId);
            if (dIndex == -1)
            {
                throw new BO.DoesntExistException("drone does not exist");
            }
            try
            {
                //updates info in dal
                var tempDrone = dal.findDrone(droneId);
                tempDrone.model = dmodel;
                dal.UpdateDrone(tempDrone);
                //updats info in drones list
                BO.DroneToList dr = drones.Find(p => p.droneId == droneId);
                drones.Remove(dr);
                dr.model = dmodel;
                drones.Add(dr);
            }
            catch (DO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region UpdateCustomer
        //updates customer
        public void UpdateCustomer(int customerId, string name, string number)
        {
            //updates info
            var tempCustomer = dal.getCustomer(customerId);
            if (name != "")
                tempCustomer.name = name;
            if (number != "")
                tempCustomer.Phone = number;
            dal.UpdateCustomer(tempCustomer);

        }
        #endregion
        #region releaseDroneFromCharge
        //this function releases drone from charge
        public void releaseDroneFromCharge(int droneId)
        {
            var tempDrone = getDrone(droneId);
            var temp = returnsDrone(droneId);
            try
            {
                double chargeTime = DateTime.Now.Subtract(dal.getDroneCharge(droneId).chargeTime).TotalMinutes;
            }
            catch (DO.DoesntExistException exc)
            {
                throw new BO.DoesntExistException(exc.Message);
            }
            //checks if drone is charging
            if (tempDrone.droneStatus == DroneStatus.maintenance)
            {
                double chargeTime = DateTime.Now.Subtract(dal.getDroneCharge(droneId).chargeTime).TotalMinutes;
                //updates info
                var possibleStation = getStation(dal.printStationsList().ToList()
                                        .Find(station => station.latitude == tempDrone.location.latitude
                                        && station.longitude == tempDrone.location.longitude).stationId);
                BatteryUsage usage = new BatteryUsage();
                tempDrone.battery += (chargeTime * usage.chargeSpeed);
                if (tempDrone.battery > 100)
                    tempDrone.battery = 100;
                dal.deleteDroneCharge(tempDrone.droneId, possibleStation.stationId);
                drones.ForEach(d => {
                    if (d.droneId == droneId)
                    { d.droneStatus = DroneStatus.available; d.battery = tempDrone.battery; }
                });
                dal.deleteStation(possibleStation.stationId);
                possibleStation.numberOfSlotsInUse--;
                addStation(possibleStation);

            }
            else
                throw (new UnableToCompleteRequest("Drone was not charging\n"));
        }
        #endregion
        #region updateStation
        //this function updates station
        public void updateStation(int stationId, int AvlblDCharges, string name = "")
        {
            try
            {
                //checks if station exits
                DO.Station stationDl = new DO.Station();
                //updates info
                stationDl = dal.getStation(stationId);
                if (name != "")
                    stationDl.name = name;
                if (AvlblDCharges != 0)
                {
                    if (AvlblDCharges < 0)
                        throw new UnableToCompleteRequest("the amount of drone charging slots is invalid!\n");
                    stationDl.chargeSlots = AvlblDCharges;
                    dal.deleteStation(stationId);
                    dal.AddStation(stationDl);
                }
            }
            catch (BO.DoesntExistException exp)
            {
                throw exp;
            }
            catch (DO.DoesntExistException exp)
            {
                throw new BO.DoesntExistException(exp.Message);
            }
            catch (BO.UnableToCompleteRequest exp)
            {
                throw exp;
            }
        }
        #endregion
        #region MatchDroneWithPacrel
        //matches drone with parcel
        public void matchDroneWithPacrel(int droneId)
        {

            try
            {
                var myDrone = getDrone(droneId);
                var droneLoc = closestStation(myDrone.location, false, stationLocationslist());
                var station = getStations().ToList().Find(x => x.location.longitude == droneLoc.longitude
                                                          && x.location.latitude == droneLoc.latitude);
                if (myDrone.droneStatus != DroneStatus.available)
                    throw new unavailableException("the drone is unavailable\n");
                DO.Parcel myParcel = findTheParcel(myDrone.maxWeight, myDrone.location, myDrone.battery, DO.Priorities.emergency);
                dal.attribute(myDrone.droneId, myParcel.parcelId);
                int index = drones.FindIndex(x => x.droneId == droneId);
                drones.RemoveAt(index);
                myDrone.droneStatus = DroneStatus.delivery;
                myDrone.parcel = new ParcelInTransit();
                myDrone.parcel.parcelId = myParcel.parcelId;
                myDrone.parcel.parcelStatus = false;
                var tempParcel = myParcel;
                tempParcel.droneId = droneId;
                tempParcel.scheduled = DateTime.Now;
                dal.UpdateParcel(tempParcel);
                var tempD = new DroneToList()
                {
                    droneId = myDrone.droneId,
                    model = myDrone.model,
                    battery = myDrone.battery,
                    weight = myDrone.maxWeight,
                    droneStatus = DroneStatus.delivery,
                    location = new Location(myDrone.location.latitude, myDrone.location.longitude),
                    parcelId = myDrone.parcel.parcelId,
                    numOfParcelsdelivered = dal.printParcelsList().Where(p => p.parcelId == myDrone.parcel.parcelId).Count()
                };
                drones.Add(tempD);


            }
            catch (BO.DoesntExistException exp) { throw new BO.DoesntExistException(exp.Message); }

        }
        #endregion
        #region PickUpParcel
        //pick up parcel to deliver(doesnt actually deliver yet!!
        public void pickUpParcel(int droneId)
        {
            var tempDrone = getDrone(droneId);
            var tempParcel = getParcel(tempDrone.parcel.parcelId);
             var customer = getCustomer(tempParcel.sender.customerId);
           
            //ensures was not yet picked up
            if (tempParcel.pickedUp == null)
            {
                //updates info
                //dal.DeleteDrone(tempDrone.droneId);
                int index = drones.FindIndex(d => d.droneId == droneId);
                drones.RemoveAt(index);
                BatteryUsage usage = new BatteryUsage();

                tempDrone.parcel.parcelStatus = true;

                //AddDrone(tempDrone,FindStation(tempDrone.location));
                var tempD = new DroneToList()
                {
                    droneId = tempDrone.droneId,
                    model = tempDrone.model,
                    battery = tempDrone.battery - (distance(tempDrone.location, customer.location) * usage.available),
                    weight = tempDrone.maxWeight,
                    droneStatus = DroneStatus.delivery,
                    location = new Location(tempDrone.location.latitude, tempDrone.location.longitude),
                    parcelId = tempDrone.parcel.parcelId,
                    numOfParcelsdelivered = dal.printParcelsList().Where(p => p.parcelId == tempDrone.parcel.parcelId).Count()
                };
                tempDrone.location.latitude = tempDrone.parcel.pickupLocation.latitude;
                tempDrone.location.longitude = tempDrone.parcel.pickupLocation.longitude;
                drones.Add(tempD);
                // dal.DeleteParcel(tempParcel.parcelId);
                tempParcel.pickedUp = DateTime.Now;
                DO.Parcel parcel = new DO.Parcel()
                {
                    parcelId = tempParcel.parcelId,
                    senderId = tempParcel.sender.customerId,
                    targetId = tempParcel.target.customerId,
                    weight = (DO.weightCategories)tempParcel.weight,
                    priority = (DO.Priorities)tempParcel.priority,
                    droneId = tempParcel.drone.droneId,
                    requested = tempParcel.requested,
                    scheduled = tempParcel.scheduled,
                    pickedUp = tempParcel.pickedUp,
                    delivered = null
                };
                dal.UpdateParcel(parcel);

            }
            else
                throw (new UnableToCompleteRequest());
        }
        #endregion
        #region deliveredParcel
        //delievrs parcel
        public void deliveredParcel(int droneId)
        {
            var tempDrone = getDrone(droneId);
            var tempParcel = new BO.Parcel();
            tempParcel = getParcel(tempDrone.parcel.parcelId);
            //ensures was not yet delivered
            if (tempParcel.delivered == null)
            {
                //updates info
                // dal.DeleteDrone(tempDrone.droneId);
                int index = drones.FindIndex(d => d.droneId == droneId);
                drones.RemoveAt(index);
                BatteryUsage usage = new BatteryUsage();
                var customer = getCustomer(tempDrone.parcel.target.customerId);
                int amount = (int)tempParcel.weight;
                if (amount == 1)
                    tempDrone.battery -= distance(tempDrone.location, customer.location) * usage.light;
                if (amount == 2)
                    tempDrone.battery -= distance(tempDrone.location, customer.location) * usage.medium;
                if (amount == 3)
                    tempDrone.battery -= distance(tempDrone.location, customer.location) * usage.heavy;
                tempDrone.location.latitude = customer.location.latitude;
                tempDrone.location.longitude = customer.location.longitude;
                tempDrone.droneStatus = DroneStatus.available;
                tempDrone.parcel.parcelStatus = true;
                var tempD = new DroneToList()
                {
                    droneId = tempDrone.droneId,
                    model = tempDrone.model,
                    battery = tempDrone.battery,
                    weight = tempDrone.maxWeight,
                    droneStatus = DroneStatus.available,
                    location = new Location(tempDrone.location.latitude, tempDrone.location.longitude),
                    parcelId = 0,
                    numOfParcelsdelivered = dal.printParcelsList().Where(p => p.parcelId == tempDrone.parcel.parcelId).Count()
                };
                drones.Add(tempD);

                DO.Parcel parcel = new DO.Parcel()
                {
                    parcelId = tempParcel.parcelId,
                    senderId = tempParcel.sender.customerId,
                    targetId = tempParcel.target.customerId,
                    weight = (DO.weightCategories)tempParcel.weight,
                    priority = (DO.Priorities)tempParcel.priority,
                    droneId = tempDrone.droneId,
                    requested = tempParcel.requested,
                    scheduled = tempParcel.scheduled,
                    pickedUp = tempParcel.pickedUp,
                    delivered = DateTime.Now
                };
                dal.UpdateParcel(parcel);

            }
            else
                throw (new UnableToCompleteRequest());
        }
        #endregion
        #region SendDroneToCharge
        //sends drone to charge at station
        public void SendDroneToCharge(int droneId)
        {
            BO.Drone drone = new();
            BO.Station station = new();
            try
            {
                //ensures drone exists
                drone = getDrone(droneId);


            }
            catch (DO.DoesntExistException exp)
            {
                throw new BO.DoesntExistException(exp.Message);
            }
            //ensures available
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("not available");
            //find closest sation to charge at
            Location stationLocation = closestStation(drone.location, false, stationLocationslist());

            station = getStations().Find(x => x.location.longitude == stationLocation.longitude
                                          && x.location.latitude == stationLocation.latitude);
            int droneIndex = drones.ToList().FindIndex(x => x.droneId == droneId);
            if ((drone.battery - minBatteryRequired(drones[droneIndex].droneId) < 0))
                throw new UnableToCompleteRequest("the drone doesn't have enough charge");
            //updates info
            if (station.chargeSlots > 0)
            {
                dal.deleteStation(station.stationId);
                station.numberOfSlotsInUse++;
                addStation(station);

                // station.numberOfSlotsInUse++;
            }
            //   if ((drone.battery - minBatteryRequired(drones[droneIndex].droneId) > 0))
            drones[droneIndex].battery -= minBatteryRequired(drones[droneIndex].droneId);
            if (drones[droneIndex].battery < 0)
                drones[droneIndex].battery = 0;
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.maintenance;
            //var temp = getDrone(drones[droneIndex].droneId);
            DO.DroneCharge DC = new DroneCharge { droneId = droneId, stationId = station.stationId, chargeTime = DateTime.Now };
            dal.AddDroneCharge(DC);
        }
        #endregion
        #region releaseAllFromCharge
        public void releaseAllFromCharge()
        {
            var listDrone = allDrones();
            foreach (var drone in listDrone)
            {
                if (drone.droneStatus == DroneStatus.maintenance)
                    releaseDroneFromCharge(drone.droneId);
            }
        }
        #endregion
        #endregion
        public void openSimulator(int droneId, Action updateView, Func<bool> isRun)
        {
            new Simulator(droneId, updateView, isRun, this);
        }

    }






}



















