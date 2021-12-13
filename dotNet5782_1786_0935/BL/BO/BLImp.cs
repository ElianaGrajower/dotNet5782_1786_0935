using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL.DalObject;
using IBL.BO;




namespace BL
{
    
    public class BLImp           
    {
        
        public double[] chargeCapacity;    
        private List<IBL.BO.DroneToList> drones; 
        DAL.DalObject.DalObject dal;
        public static Random rand = new Random(); 

        #region GetChargeCapacity
       //this function returns an arr of chargecapacity
        public chargeCapacity GetChargeCapacity()
        {
        
            double[] arr = dal.ChargeCapacity();
            var chargeCapacity=new chargeCapacity {  pwrLight=arr[0], pwrAverge= arr[1], pwrAvailable= arr[2],pwrHeavy= arr[3],pwrRateLoadingDrone= arr[4], chargeCapacityArr=arr};
            return chargeCapacity;
        }
        #endregion
        #region GetStationsList
        //this function returns a list of all the stations
        public List<IBL.BO.Station> GetStationsList()
        {
            List<IBL.BO.Station> baseStations = new List<IBL.BO.Station>();
            try
            {//makes temp list
                //calls on getlist of stations function from dal
                var stationsDal = dal.printStationsList().ToList();
                //in each link calls on get station which convers it to ibl station and adds it to temp list
                foreach (var s in stationsDal)
                { baseStations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            //returns temp list
            return baseStations;
        }
        #endregion GetStationsList
        #region MinBatteryRequired
        //this function checks how much battery is needed for the drone to get from point a to point b
        private int MinBatteryRequired(int droneId)
        { //gets drone 
            var drone = GetDrone(droneId);
            //if drone available
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = ClosestStation(drone.location, false, StationLocationslist());
                return (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * Distance(drone.location, location));
            }
            //if drone out for delivery
            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.parcel.parcelId);
                //if wasnt picked up and updates all the info
                if (parcel.PickedUp == null)
                {
                    int minValue;
                    IBL.BO.Customer sender = GetCustomer(parcel.SenderId);
                    var target = GetCustomer(parcel.TargetId);
                    double droneToSender = Distance(drone.location, sender.Location);
                    minValue = (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * droneToSender);
                    double senderToTarget = Distance(sender.Location, target.Location);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)parcel.Weight] * senderToTarget);
                    Location baseStationLocation = ClosestStation(target.Location, false,StationLocationslist());
                    double targetToCharge = Distance(target.Location, baseStationLocation);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * targetToCharge);
                    return minValue;
                }
                //if wasnt delievred updates all the info
                if (parcel.Delivered == null)
                {
                    int minValue;
                    var sender = GetCustomer(parcel.SenderId);
                    var target = GetCustomer(parcel.TargetId);
                    double senderToTarget = Distance(sender.Location, target.Location);
                    int batteryUsage = (int)parcel.Weight;
                    minValue = (int)(GetChargeCapacity().chargeCapacityArr[batteryUsage] * senderToTarget);
                    Location baseStationLocation = ClosestStation(target.Location, false,StationLocationslist());
                    double targetToCharge = Distance(target.Location, baseStationLocation);
                    minValue += (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * targetToCharge); 
                    return minValue;
                }
            }
            return 90;
        }
        #endregion
        #region FindStation
        //this function finds a station based on coordinates that it recives
        public int FindStation(Location location)
        {
            var station = dal.printStationsList().Where(s => s.Longitude == location.Longitude && s.Lattitude == location.Lattitude);
            if (station.Count() == 0)
                throw new IBL.BO.DoesntExistException("station with these coordinates doesnt exist\n" );
            var stationId = station.First().StationId;
            return stationId;
        }
        #endregion
        #region AddDrone
        //this function adds a drone
        public void AddDrone(IBL.BO.Drone DronetoAdd, int stationId)
        {
            //checks validity of input
            if (DronetoAdd.DroneId <= 0)
                throw new IBL.BO.InvalidInputException("drone id not valid- must be a posittive\n");
            if (DronetoAdd.MaxWeight != IBL.BO.WeightCategories.light && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.average && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.heavy)
                throw new IBL.BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");
            if(DronetoAdd.battery==0)
            DronetoAdd.battery = rand.Next(20, 40);
            if (DronetoAdd.droneStatus == 0)
            { DronetoAdd.droneStatus = DroneStatus.maintenance; }
            try
            {
                //checks if station exists
                double StationLattitude = dal.findStation(stationId).Lattitude;
                double StationLongitude = dal.findStation(stationId).Longitude;
                DronetoAdd.location = new Location(StationLattitude, StationLongitude);
            }
            catch (IDAL.DO.DoesntExistException exc)
            {
                throw new IBL.BO.DoesntExistException(exc.Message);
            }
           // builds new dronetolist
            DroneToList dtl = new DroneToList();
            dtl.droneId = DronetoAdd.DroneId;
            dtl.Model = DronetoAdd.Model;
            dtl.weight = DronetoAdd.MaxWeight;
            dtl.battery = DronetoAdd.battery;
            dtl.droneStatus = DronetoAdd.droneStatus; 
            dtl.location = new Location(0, 0);
            dtl.location.Lattitude = dal.findStation(stationId).Lattitude;
            dtl.location.Longitude = dal.findStation(stationId).Longitude;
            //build idal.do.drone
            IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            {
                DroneId = DronetoAdd.DroneId,
                Model = DronetoAdd.Model,
                MaxWeight = (IDAL.DO.WeightCategories)((int)DronetoAdd.MaxWeight)
            };
            try
            {
                //updates the list that contain info
                dal.AddDrone(newDrone);
                //if(drones.Where(d=>dtl.droneId==d.droneId).Count()==0)
                drones.Add(dtl);
                IDAL.DO.DroneCharge dc = new IDAL.DO.DroneCharge { DroneId = DronetoAdd.DroneId, StationId = stationId };
                var tempstation = GetStation(stationId);
                if (drones.Where(d => dtl.droneId == d.droneId).Count() == 0)
                    tempstation.decreaseChargeSlots();
                updateStation(tempstation.StationId, tempstation.chargeSlots, "");
                  dal.AddDroneCharge(dc);
            }
            catch (IBL.BO.AlreadyExistsException exc)
            {
                throw exc;
            }
            catch (IDAL.DO.AlreadyExistException exc)
            {
                throw new IBL.BO.AlreadyExistsException(exc.Message);
            }
        }
        #endregion
        #region OnlyDigits
        //this function ensures that  enteres is only numbers
        public bool OnlyDigits(char x)
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
        #region Distance
        //calculates distance between two location based on a mathematical calculation for coordinates
        private double Distance(IBL.BO.Location l1, IBL.BO.Location l2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(l2.Lattitude - l1.Lattitude);  // deg2rad below
            var dLon = deg2rad(l2.Longitude - l1.Longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(l1.Lattitude)) * Math.Cos(deg2rad(l2.Lattitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        #endregion
        #region GetStation
        //this function returns a station
        public IBL.BO.Station GetStation(int stationId)
        {
            try
            {
                IBL.BO.Station station = new IBL.BO.Station();   
                //gets all info it can from dal.stationlist
                IDAL.DO.Station tempStation = dal.GetStation(stationId);
                station.StationId = tempStation.StationId;
                station.name = tempStation.Name;
                station.location=new Location(tempStation.Lattitude, tempStation.Longitude);
                station.chargeSlots = tempStation.ChargeSlots;
                //finds the rest of the info from dronecharging ist
                station.DronesatStation = dal.printDroneChargeList().Where(item=>item.StationId== stationId)
                    .Select(drone => new DroneInCharging()
                    {
                        droneId = drone.DroneId,
                        battery = getDroneBattery(drone.DroneId)
                    }).ToList();
                return station;
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region GetStations
        //this function returns a list of stations
        public List<IBL.BO.Station> GetStations()
        {
            List<IBL.BO.Station> stations = new List<IBL.BO.Station>();
            try
            {//gets list of dal stations
                var stationsDal = dal.printStationsList().ToList();
                //in each link calls on get statio to conver to ibl station and adds to temp list
                foreach (var s in stationsDal)    
                { stations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return stations;
        }
        #endregion
        #region GetAvailableStationsList
        //this function returns a list of stations with available charge slots
        public List<IBL.BO.Station> GetAvailableStationsList()
        {
            //gets list of stations from dal
            List<IBL.BO.Station> stations = new List<IBL.BO.Station>();
            //converts to only lists with available charge slots
            stations.Where(station => station.chargeSlots > 0);
            try
            {

                var stationsDal = dal.printStationsList().ToList();
                //for each one calls get station to convert to ibl station
                foreach (var s in stationsDal)
                {stations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return stations;
        }
        #endregion
        #region AvailableChargingSlots
        //returns amount of chargeslots
        public int AvailableChargingSlots()
        {

            IBL.BO.Station station = new IBL.BO.Station();
            return station.chargeSlots;
        }
        #endregion
        #region StationLocationslist
        //returns a list of all the the station locations
        private List<Location> StationLocationslist()
        {
            List<Location> locations = new List<Location>();
            foreach (var station in GetStations())
            {
                //adds location of current station
                locations.Add(new Location(station.location.Lattitude, station.location.Longitude));
                
            }
            return locations;
        }
        #endregion
        #region ClosestStation
        //finds closeststation with chargeslots
        private Location ClosestStation(Location currentlocation, bool withChargeSlots, List<Location> l)//the function could also be used to check in addtion if the charge slots are more then 0
        {

            var locations = l;
            Location location = locations[0];
            //calculates distance
            double distance = Distance(locations[0], currentlocation);
            for (int i = 1; i < locations.Count; i++)
            {
                //if has chatgeslots
                if (withChargeSlots)
                {
                    var station = GetStations().ToList().Find(x => x.location.Longitude == locations[i].Longitude && x.location.Longitude == locations[i].Longitude);

                    //checks distance
                    if (Distance(locations[i], currentlocation) < distance && station.chargeSlots > 0)
                    {
                        distance = Distance(locations[i], currentlocation);
                    }
                    else
                    {
                        if (locations.Count() == 0)
                            throw new Exception("there are no stations with available charge slots");
                        locations.RemoveAt(i);
                        ClosestStation(currentlocation, withChargeSlots, locations);
                    }

                }
                else
                {
                    if (Distance(locations[i], currentlocation) < distance)
                    {
                        location = locations[i];
                        distance = Distance(locations[i], currentlocation);
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
            return drones.ToList().Find(drone => drone.droneId == droneId).battery;
        }
        #endregion
        #region AddCustomer
        //adds customer
        public void AddCustomer(IBL.BO.Customer CustomertoAdd)
        {

            CustomertoAdd.ParcelsOrdered = new List<ParcelinCustomer>();
            CustomertoAdd.ParcelsDelivered = new List<ParcelinCustomer>();
            //chelcs validity of input
            if (CustomertoAdd.CustomerId > 999999999 || CustomertoAdd.CustomerId < 100000000)
                throw new InvalidCastException("customer id not valid\n");
            if (!CustomertoAdd.Phone.All(OnlyDigits))
                throw new InvalidCastException("customer phone not valid- must contain only numbers\n");
        

            //builds idal customer
            IDAL.DO.Customer newCustomer = new IDAL.DO.Customer()
            {
                CustomerId = CustomertoAdd.CustomerId,
                Name = CustomertoAdd.Name,
                Phone = CustomertoAdd.Phone,
                Lattitude = CustomertoAdd.Location.Lattitude,
                Longitude = CustomertoAdd.Location.Longitude
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
        #region AddStation
        //adds station
        public void AddStation(IBL.BO.Station StationtoAdd)
        {

            StationtoAdd.DronesatStation = new List<DroneInCharging>();
            //checks validity of input
            if (StationtoAdd.StationId <= 0)
                throw new IBL.BO.InvalidInputException("station id not valid- must be a posittive\n");//check error
           
            if (StationtoAdd.chargeSlots <= 0)
                throw new IBL.BO.InvalidInputException("invalid amount of chargeslots- must be a positive number");


            //builds dal station
            IDAL.DO.Station newStation = new IDAL.DO.Station()
            {
                StationId = StationtoAdd.StationId,
                Name = StationtoAdd.name,
                Lattitude = StationtoAdd.location.Lattitude,
                Longitude = StationtoAdd.location.Longitude,
                ChargeSlots = StationtoAdd.chargeSlots
            };
            try
            {
                //adds it do dal station list
                dal.AddStation(newStation);
            }
            catch (IDAL.DO.AlreadyExistException exc)
            {
                throw new IBL.BO.AlreadyExistsException(exc.Message);
            }
        }
        #endregion
        #region AddParcel
        //adds parcel
        public int AddParcel(IBL.BO.Parcel parcelToAdd)
        {

            //checks validity of input
            if (!(parcelToAdd.Sender.CustomerId >= 10000000 && parcelToAdd.Sender.CustomerId <= 1000000000))
                throw new IBL.BO.InvalidInputException("the id sender number of the pardel is invalid\n");
            if (!(parcelToAdd.Target.CustomerId >= 10000000 && parcelToAdd.Target.CustomerId <= 1000000000))
                throw new IBL.BO.InvalidInputException("the id receive number of the parcel is invalid\n");
            if (!(parcelToAdd.Weight >= (IBL.BO.WeightCategories)1 && parcelToAdd.Weight <= (IBL.BO.WeightCategories)3))
                throw new IBL.BO.InvalidInputException("the given weight is not valid\n");
            if (!(parcelToAdd.Priority >= (IBL.BO.Priorities)0 && parcelToAdd.Priority <= (IBL.BO.Priorities)3))
                throw new IBL.BO.InvalidInputException("the given priority is not valid\n");
            //build idalparcel
            IDAL.DO.Parcel parcelDo = new IDAL.DO.Parcel();
            parcelDo.ParcelId = dal.getParcelId();
            parcelDo.SenderId = parcelToAdd.Sender.CustomerId;
            parcelDo.TargetId = parcelToAdd.Target.CustomerId;
            parcelDo.Weight = (IDAL.DO.WeightCategories)parcelToAdd.Weight;
            parcelDo.Priority = (IDAL.DO.Priorities)parcelToAdd.Priority;
            parcelDo.Requested = DateTime.Now;
            parcelDo.Scheduled = DateTime.MinValue;
            parcelDo.PickedUp = DateTime.MinValue;
            parcelDo.Delivered = DateTime.MinValue;
            parcelDo.DroneId = 0;
            
            try
            {
                //adds it to dal list of parcels
                 dal.AddParcel(parcelDo);
                return parcelDo.ParcelId;
            }
            catch (IDAL.DO.AlreadyExistException exp)
            {
                throw new AlreadyExistException(exp.Message);
            }
        }
        #endregion
        #region DeleteStation
        //delets station
        public void DeleteStation(int StationId)
        {
            try
            {
                //Calls on delete fun from dal
                dal.DeleteStation(StationId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteParcel
        //deletes parcel
        public void DeleteParcel(int ParcelId)
        {
            try
            {
                //calls on delete func from dal
                dal.DeleteParcel(ParcelId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteCustomer
        //delets customer
        public void DeleteCustomer(int CustomerId)
        {
            try
            {
                //calls on delete func from dal
                dal.DeleteCustomer(CustomerId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteDrone
        //delets drone
        public void DeleteDrone(int DroneId)
        {
            try
            {
                dal.DeleteDrone(DroneId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region GetCustomer
        //returns customer
        public IBL.BO.Customer GetCustomer(int customerId)
        {
            try
            {
                //gets customer from dal list
                IDAL.DO.Customer temp = dal.GetCustomer(customerId);
                //starts building bo customer
                IBL.BO.Customer customer = new IBL.BO.Customer()
                {
                    CustomerId = temp.CustomerId,
                    Name = temp.Name,
                    Phone = temp.Phone,
                    Location = new Location(temp.Lattitude, temp.Longitude)
                    {
                        Lattitude = temp.Lattitude,
                        Longitude = temp.Longitude,

                    },
                    ParcelsOrdered = dal.printParcelsList().Where(parcel => parcel.TargetId == customerId).Select(Parcel => new ParcelinCustomer()

                    {
                        ParcelId = Parcel.ParcelId,
                        Weight = (IBL.BO.WeightCategories)((int)Parcel.Weight),
                        Priority = (IBL.BO.Priorities)((int)Parcel.Priority),
                        ParcelStatus = ParcelStatus.delivered,
                        CustomerInParcel = new CustomerInParcel()
                        {
                            CustomerId = Parcel.SenderId,
                            CustomerName = dal.GetCustomer(Parcel.SenderId).Name
                        }
                    }),
            
                    ParcelsDelivered = dal.printParcelsList().Where(parcel => parcel.SenderId == customerId).Select(Parcel => new ParcelinCustomer()

                    {
                        ParcelId = Parcel.ParcelId,
                        Weight = (IBL.BO.WeightCategories)((int)Parcel.Weight),
                        Priority = (IBL.BO.Priorities)((int)Parcel.Priority),
                        ParcelStatus = ParcelStatus.delivered,
                        CustomerInParcel = new CustomerInParcel()
                        {
                            CustomerId = Parcel.TargetId,
                            CustomerName = dal.GetCustomer(Parcel.TargetId).Name
                        }

                    })
        
                };
               return customer;

            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
            catch (IDAL.DO.DoesntExistException exc)
            {
                throw new IBL.BO.DoesntExistException(exc.Message);
            }

        }
        #endregion
        #region GetParcel
        //returns parcle
        public IBL.BO.Parcel GetParcel(int parcelId)
        {

            try
            {
                //gets parcel
                IDAL.DO.Parcel temp = dal.GetParcel(parcelId);
                //builds a parcel
                IBL.BO.Parcel parcel = new IBL.BO.Parcel()
                {
                    ParcelId = parcelId,
                    Sender = new CustomerInParcel()
                    {
                        CustomerId = temp.SenderId,
                        CustomerName = dal.GetCustomer(temp.SenderId).Name
                    },
                    Target = new CustomerInParcel()
                    {
                        CustomerId = temp.TargetId,
                        CustomerName = dal.GetCustomer(temp.TargetId).Name
                    },
                    Weight = (IBL.BO.WeightCategories)((int)temp.Weight),
                    Priority = (IBL.BO.Priorities)((int)temp.Priority),
                    Requested = temp.Requested,
                    Scheduled = temp.Scheduled,
                    PickedUp = temp.PickedUp,
                    Delivered = temp.Delivered,
                    //builds a drone in parcel
                    Drone = new DroneInParcel()
                };
                //fills the drone in parcel
                parcel.Drone.droneId = temp.DroneId;
                if (temp.DroneId != 0)
                    parcel.Drone.battery = getDroneBattery(temp.DroneId);
                else
                    parcel.Drone.battery = 0;
                if (temp.DroneId != 0)
                    parcel.Drone.location = new Location(GetDrone(temp.DroneId).location.Lattitude, GetDrone(temp.DroneId).location.Longitude);
                else
                    parcel.Drone.location = new Location(0, 0);
                return parcel;

            }
            catch(IDAL.DO.DoesntExistException exc)
            {
                throw new IBL.BO.DoesntExistException( exc.Message);
            }
        }
        #endregion
        #region BLImp
        public BLImp()
        {
            {
                dal = new DAL.DalObject.DalObject();
                drones = new List<IBL.BO.DroneToList>();
                bool flag = false;
                Random rnd = new Random();
                double minBatery = 0;
                //gets lists of drones
                IEnumerable<IDAL.DO.Drone> d = dal.printDronesList();
                //gets list of parcel
                IEnumerable<IDAL.DO.Parcel> parcels = dal.printParcelsList();
                chargeCapacity chargeCapacity = GetChargeCapacity();
                //goes over every drone in the list
                foreach (var item in d)
                {
                    //builds a drone to list
                    IBL.BO.DroneToList drt = new DroneToList();
                    drt.droneId = item.DroneId;
                    drt.Model = item.Model;
                    drt.weight = (IBL.BO.WeightCategories)(int)item.MaxWeight;
                    drt.numOfParcelsDelivered = dal.printParcelsList().Count(x => x.DroneId == drt.droneId);
                    int parcelID = dal.printParcelsList().ToList().Find(x => x.DroneId == drt.droneId).ParcelId;
                    drt.parcelId = parcelID;
                    var baseStationLocations =StationLocationslist();
                    //goes over every parcel in list
                    foreach (var pr in parcels)
                    {
                        //if not yet delivered updates info
                        if (pr.DroneId == item.DroneId && pr.Delivered == DateTime.MinValue)
                        {
                            IDAL.DO.Customer sender = dal.GetCustomer(pr.SenderId);
                            IDAL.DO.Customer target = dal.GetCustomer(pr.TargetId);
                            IBL.BO.Location senderLocation = new Location(sender.Lattitude, sender.Longitude );
                            IBL.BO.Location targetLocation = new Location ( target.Lattitude, target.Longitude );
                            drt.droneStatus = DroneStatus.delivery;
                            if (pr.PickedUp == DateTime.MinValue && pr.Scheduled != DateTime.MinValue)
                            {
                                drt.location = new Location( ClosestStation(senderLocation, false, baseStationLocations).Lattitude,ClosestStation(senderLocation, false, baseStationLocations).Longitude);
                                minBatery = Distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(senderLocation, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.Weight];
                                minBatery += Distance(targetLocation, new Location(ClosestStation(targetLocation, false, baseStationLocations).Lattitude,   ClosestStation(targetLocation, false, baseStationLocations).Longitude )) * chargeCapacity.chargeCapacityArr[0];
                            }
                            if (pr.PickedUp != DateTime.MinValue && pr.Delivered == DateTime.MinValue)
                            {
                                
                                drt.location = senderLocation;
                                minBatery = Distance(targetLocation, new Location ( ClosestStation(targetLocation, false, baseStationLocations).Lattitude,  ClosestStation(targetLocation, false, baseStationLocations).Longitude )) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(drt.location, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.Weight];
                            }
                            if (minBatery > 100) { minBatery = 100; }
                            drt.battery = rnd.Next((int)minBatery, 101); // 100/;
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        int temp = rnd.Next(1, 3);
                        if (temp == 1)
                            drt.droneStatus = IBL.BO.DroneStatus.available;
                        else
                            drt.droneStatus = IBL.BO.DroneStatus.maintenance;
                        if (drt.droneStatus == IBL.BO.DroneStatus.maintenance)
                        {
                            int r = rnd.Next(0, dal.printStationsList().Count()), i = 0;
                            IDAL.DO.Station s = new IDAL.DO.Station();
                            foreach (var ite in dal.printStationsList())
                            {
                                s = ite;
                                if (i == r)
                                    break;
                                i++;
                            }
                            IDAL.DO.DroneCharge DC = new IDAL.DO.DroneCharge { DroneId=drt.droneId, StationId=s.StationId };
                            dal.AddDroneCharge(DC);
                            drt.location = new Location( s.Lattitude,  s.Longitude );
                            drt.battery = rnd.Next(0, 21); // 100/;
                        }
                        else
                        {
                            List<IDAL.DO.Customer> lst = new List<IDAL.DO.Customer>();
                            foreach (var pr in parcels)
                            {
                                if (pr.Delivered != DateTime.MinValue)
                                    lst.Add(dal.GetCustomer(pr.TargetId));
                            }
                            if (lst.Count == 0)
                            {
                                foreach (var pr in dal.printCustomersList())
                                {

                                    lst.Add(pr);
                                }
                            }
                            int l = rnd.Next(0, lst.Count());

                            drt.location = new Location (lst[l].Lattitude, lst[l].Longitude );
                            Location Location1 = new Location (lst[l].Lattitude,  lst[l].Longitude );

                            minBatery += Distance(drt.location, new Location ( ClosestStation(Location1, false, baseStationLocations).Longitude, ClosestStation(Location1, false, baseStationLocations).Lattitude )) * chargeCapacity.chargeCapacityArr[0];

                            if (minBatery > 100) { minBatery = 100; }

                            drt.battery = rnd.Next((int)minBatery, 101);
                        }

                    }
                    drones.Add(drt);

                    Console.WriteLine(drt.ToString());


                }
                

            }
            

        }
        #endregion
        #region UpdateDroneName
        //updates drone name
        public void UpdateDroneName(int droneID, string dModel)
        {
            //checks id drone exits
            int dIndex = drones.FindIndex(x => x.droneId == droneID);
            if (dIndex == 0)
            {
                throw new IBL.BO.DoesntExistException("drone does not exist");
            }
            try
            {
                //updates info
                var tempDrone = dal.findDrone(droneID);
                tempDrone.Model = dModel;
                dal.UpdateDrone(tempDrone);
                IBL.BO.DroneToList dr = drones.Find(p => p.droneId == droneID);
                drones.Remove(dr);
                dr.Model = dModel;
                drones.Add(dr);
            }
            catch (IDAL.DO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region UpdateCustomer
        //updates customer
        public void UpdateCustomer(int CustomerId,string name,string number)
        {
            //updates info
            var tempCustomer = dal.GetCustomer(CustomerId);
            if (name != "")
                tempCustomer.Name = name;
            if (number != "")    
                tempCustomer.Phone = number;
            dal.DeleteCustomer(CustomerId);
            dal.AddCustomer(tempCustomer);
        }
        #endregion
        #region ReleaseDroneFromCharge
        //this function releases drone from charge
        public void ReleaseDroneFromCharge(int droneId,int chargeTime)
        {
            
            
                var tempDrone = GetDrone(droneId);
                var temp = returnsDrone(droneId);
            //checks if drone is charging
                if (tempDrone.droneStatus == DroneStatus.maintenance)
                {
                //updates info
                    var possibleStation = GetStation(dal.printStationsList().ToList().Find(station => station.Lattitude == tempDrone.location.Lattitude && station.Longitude == tempDrone.location.Longitude).StationId);
                  //  dal.DeleteDrone(tempDrone.DroneId);
                    //drones.RemoveAll(D => D.droneId == droneId);
                BatteryUsage usage = new BatteryUsage();
                tempDrone.battery += chargeTime * usage.chargeSpeed;
                //tempDrone.droneStatus = DroneStatus.available;
                dal.DeleteDroneCharge(tempDrone.DroneId, possibleStation.StationId);
               // AddDrone(tempDrone, possibleStation.StationId);
               
                drones.ForEach(d => { if (d.droneId == droneId) { d.droneStatus = DroneStatus.available; d.battery += chargeTime * usage.chargeSpeed; } });
                

                dal.DeleteStation(possibleStation.StationId);
                possibleStation.addChargeSlots();
                AddStation(possibleStation);


            }
                else
                    throw (new UnableToCompleteRequest("Drone was not charging\n"));
            }
        #endregion
        #region updateStation
        //this function updates station
        public void updateStation(int stationID, int AvlblDCharges, string Name = "")
        {
            try
            {
                //checks if station exits
                IDAL.DO.Station stationDl = new IDAL.DO.Station();
                //updates info
                stationDl = dal.GetStation(stationID);
                if (Name !=  "")
                    stationDl.Name = Name;
                if (AvlblDCharges != 0)
                {
                    if (AvlblDCharges < 0)
                        throw new UnableToCompleteRequest("the amount of drone charging slots is invalid!\n");
                    stationDl.ChargeSlots = AvlblDCharges;
                    dal.DeleteStation(stationID);
                    dal.AddStation( stationDl);
                }
            }
            catch (IBL.BO.DoesntExistException exp)
            {
                throw exp;
            }
            catch (IDAL.DO.DoesntExistException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            catch (IBL.BO.UnableToCompleteRequest exp)
            {
                throw exp;
            }
        }
        #endregion
        #region findTheParcel
        //this function finds a parcel based on many different orders of priorities
        private IDAL.DO.Parcel findTheParcel(IBL.BO.WeightCategories we, IBL.BO.Location a, double battery, IDAL.DO.Priorities pri)
        {
            double d, x;
            IDAL.DO.Parcel theParcel = new IDAL.DO.Parcel();

            IBL.BO.Location loc = new IBL.BO.Location(0,0);
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            double far = 1000000;
            
            var parcels = dal.printParcelsList().Where(p=>p.TargetId!=0);
            var tempParcel = from item in parcels
                             where item.Priority == pri
                             select item;

            foreach (var item in tempParcel)
            {
                //checks if foudn parcel
                customer = dal.GetCustomer(item.SenderId);
                loc.Lattitude = customer.Lattitude;
                loc.Longitude = customer.Longitude;
                chargeCapacity chargeCapacity = GetChargeCapacity();
                d = Distance(a, loc);
               
                    x = Distance(loc, new IBL.BO.Location(dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Longitude));
                    double fromCusToSta = Distance(new IBL.BO.Location(dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Longitude), ClosestStation(new IBL.BO.Location(dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Longitude), false, StationLocationslist()));

                    double butteryUse = x * chargeCapacity.chargeCapacityArr[(int)item.Weight] + fromCusToSta * chargeCapacity.chargeCapacityArr[0] + d * chargeCapacity.chargeCapacityArr[0];
                    if (d < far && (battery - butteryUse) > 0 && item.Scheduled == DateTime.MinValue && weight(we, (IBL.BO.WeightCategories)item.Weight) == true)
                    {
                        far = d;
                        theParcel = item;
                        return theParcel;
                    }
                
                
                
            }
            
            //recursion
            if (pri == IDAL.DO.Priorities.emergency)
                theParcel = findTheParcel(we, a, battery, IDAL.DO.Priorities.fast);
            //recursion
            if (pri == IDAL.DO.Priorities.fast)
                theParcel = findTheParcel(we, a, battery, IDAL.DO.Priorities.regular);
            if (theParcel.ParcelId == 0)
                throw new IBL.BO.DoesntExistException("ERROR! there is not a parcel that match to the drone ");
            return theParcel;
        }
        #endregion
        #region weight
        //returns the weight cattegory
        private bool weight(IBL.BO.WeightCategories dr, IBL.BO.WeightCategories pa)
        {
            if (dr == IBL.BO.WeightCategories.heavy)
                return true;
            if (dr == IBL.BO.WeightCategories.average && (pa == IBL.BO.WeightCategories.average || pa == IBL.BO.WeightCategories.light))
                return true;
            if (dr == IBL.BO.WeightCategories.light && pa == IBL.BO.WeightCategories.light)
                return true;
            return false;
        }
        #endregion
        #region indexOfChargeCapacity
        //returns what kinds of parcel it can hold
        private int indexOfChargeCapacity(IDAL.DO.WeightCategories w)
        {
            if (w == IDAL.DO.WeightCategories.light)
                return 1;
            if (w == IDAL.DO.WeightCategories.heavy)
                return 3;
            if (w == IDAL.DO.WeightCategories.average)
                return 2;

            return 0;

        }
        #endregion
        #region MatchDroneWithPacrel
        //matches drone with parcel
        public void MatchDroneWithPacrel(int droneId)
        {

            try
            {
                var myDrone = GetDrone(droneId);
                var droneLoc = ClosestStation(myDrone.location, false, StationLocationslist());
                var station = GetStations().ToList().Find(x => x.location.Longitude == droneLoc.Longitude && x.location.Lattitude == droneLoc.Lattitude);
                if (myDrone.droneStatus != DroneStatus.available)
                    throw new unavailableException("the drone is unavailable\n");
                IDAL.DO.Parcel myParcel = findTheParcel(myDrone.MaxWeight, myDrone.location, myDrone.battery, IDAL.DO.Priorities.emergency);
                dal.attribute(myDrone.DroneId, myParcel.ParcelId);
                int index = drones.FindIndex(x => x.droneId == droneId);
               
                drones.RemoveAt(index);
                //drones.RemoveAt(index);
                myDrone.droneStatus = DroneStatus.delivery;
                
                myDrone.parcel = new ParcelInTransit();
                myDrone.parcel.parcelId = myParcel.ParcelId;
               // AddDrone(myDrone, station.StationId);
                var tempParcel = myParcel;
                tempParcel.DroneId = droneId;
                tempParcel.Scheduled = DateTime.Now;
                dal.UpdateParcel(tempParcel);
                var tempD = new DroneToList()
                {
                    droneId = myDrone.DroneId,
                    Model = myDrone.Model,
                    battery = myDrone.battery,
                    weight=myDrone.MaxWeight,
                    droneStatus = DroneStatus.delivery,
                    location = new Location(myDrone.location.Lattitude, myDrone.location.Longitude),
                    parcelId = myDrone.parcel.parcelId,
                    numOfParcelsDelivered = dal.printParcelsList().Where(p => p.ParcelId == myDrone.parcel.parcelId).Count()
                };
                drones.Add(tempD);

                //drones.ForEach(d => { if (d.droneId == myDrone.DroneId) d.droneStatus = DroneStatus.delivery;
                //    d.parcelId = myParcel.ParcelId;
                //});
            }
            catch (IBL.BO.DoesntExistException exp) { throw new IBL.BO.DoesntExistException(exp.Message); }

        }
        #endregion
        #region PickUpParcel
        //pick up parcel to deliver(doesnt actually deliver yet!!
        public void PickUpParcel(int droneId)
        {
            var tempDrone = GetDrone(droneId);
            var tempParcel = GetParcel(tempDrone.parcel.parcelId);
            //ensures was not yet picked up
            if (tempParcel.PickedUp==DateTime.MinValue)
            {
                //updates info
                //dal.DeleteDrone(tempDrone.DroneId);
                int index = drones.FindIndex(d => d.droneId == droneId);
                drones.RemoveAt(index);
                BatteryUsage usage = new BatteryUsage();
                //int amount = (int)tempParcel.Weight;
                //if(amount==1)
                //tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation)*usage.light;
                //if (amount == 2)
                //    tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation) * usage.medium;
                //if (amount == 3)
                //    tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation) * usage.heavy;
                tempDrone.location.Lattitude = tempDrone.parcel.pickupLocation.Lattitude;
                tempDrone.location.Longitude = tempDrone.parcel.pickupLocation.Longitude;
                tempDrone.parcel.parcelStatus=true;
                
                //AddDrone(tempDrone,FindStation(tempDrone.location));
                var tempD = new DroneToList()
                {
                    droneId = tempDrone.DroneId,
                    Model = tempDrone.Model,
                    battery = tempDrone.battery,
                    weight=tempDrone.MaxWeight,
                    droneStatus = DroneStatus.delivery,
                    location = new Location(tempDrone.location.Lattitude, tempDrone.location.Longitude),
                    parcelId = tempDrone.parcel.parcelId,
                    numOfParcelsDelivered = dal.printParcelsList().Where(p => p.ParcelId == tempDrone.parcel.parcelId).Count()
                };
                drones.Add(tempD);
               // dal.DeleteParcel(tempParcel.ParcelId);
                tempParcel.PickedUp = DateTime.Now;
                IDAL.DO.Parcel parcel = new IDAL.DO.Parcel()
                {
                    ParcelId = tempParcel.ParcelId,
                    SenderId = tempParcel.Sender.CustomerId,
                    TargetId = tempParcel.Target.CustomerId,
                    Weight = (IDAL.DO.WeightCategories)tempParcel.Weight,
                    Priority = (IDAL.DO.Priorities)tempParcel.Priority,
                    DroneId = tempParcel.Drone.droneId,
                    Requested = tempParcel.Requested,
                    Scheduled=tempParcel.Scheduled,
                    PickedUp = tempParcel.PickedUp,
                    Delivered=DateTime.MinValue
                };
                dal.UpdateParcel(parcel);

            }
            else
            throw (new UnableToCompleteRequest());
        }
        #endregion
        #region DeliveredParcel
        //delievrs parcel
        public void DeliveredParcel(int droneId)
        {
            var tempDrone = GetDrone(droneId);
            var tempParcel = new IBL.BO.Parcel();
            tempParcel= GetParcel(tempDrone.parcel.parcelId);
            //ensures was not yet delivered
            if (tempParcel.Delivered == DateTime.MinValue)
            {
                //updates info
                // dal.DeleteDrone(tempDrone.DroneId);
                int index = drones.FindIndex(d => d.droneId == droneId);
                drones.RemoveAt(index);
                BatteryUsage usage = new BatteryUsage();
                int amount = (int)tempParcel.Weight;
                if (amount == 1)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.light;
                if (amount == 2)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.medium;
                if (amount == 3)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.heavy;
                tempDrone.location.Lattitude = GetCustomer(tempDrone.parcel.target.CustomerId).Location.Lattitude;
                tempDrone.location.Longitude = GetCustomer(tempDrone.parcel.target.CustomerId).Location.Longitude;
                tempDrone.droneStatus = DroneStatus.available;
                var tempD = new DroneToList()
                {
                    droneId = tempDrone.DroneId,
                    Model = tempDrone.Model,
                    battery = tempDrone.battery,
                    weight=tempDrone.MaxWeight,
                    droneStatus = DroneStatus.available,
                    location = new Location(tempDrone.location.Lattitude, tempDrone.location.Longitude),
                    parcelId = 0,
                    numOfParcelsDelivered = dal.printParcelsList().Where(p => p.ParcelId == tempDrone.parcel.parcelId).Count()
                };
                drones.Add(tempD);
                IDAL.DO.Parcel parcel = new IDAL.DO.Parcel()
                {
                    ParcelId = tempParcel.ParcelId,
                    SenderId = tempParcel.Sender.CustomerId,
                    TargetId = tempParcel.Target.CustomerId,
                    Weight = (IDAL.DO.WeightCategories)tempParcel.Weight,
                    Priority = (IDAL.DO.Priorities)tempParcel.Priority,
                    DroneId = tempParcel.Drone.droneId,
                    Requested = tempParcel.Requested,
                    Scheduled=tempParcel.Scheduled,
                    PickedUp = tempParcel.PickedUp,
                    Delivered = DateTime.Now
            };
                dal.UpdateParcel(parcel);
            }
            else
            throw (new UnableToCompleteRequest());
        }
        #endregion
        #region GetDronesList
        //returns dronelist
        public List<IBL.BO.Drone> GetDronesList()
        {
            List<IBL.BO.Drone> drone = new List<IBL.BO.Drone>();
            try
            {
                //calls get drone to convert to ibl
                foreach (var d in drones)
                { drone.Add(GetDrone(d.droneId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return drone;
        }
        #endregion  
        #region GetCustomersList
        //returns customer list
        public List<IBL.BO.Customer> GetCustomersList()
        {
            List<IBL.BO.Customer> customer = new List<IBL.BO.Customer>();
            try
            {
                var customerDal = dal.printCustomersList().ToList();
                foreach (var c in customerDal)
                { customer.Add(GetCustomer(c.CustomerId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return customer;


        }
        #endregion
        #region GetParcelsList
        //returns parcel list
        public List<IBL.BO.Parcel> GetParcelsList()
        {
            List<IBL.BO.Parcel> parcel = new List<IBL.BO.Parcel>();
            try
            {
                //calls getparcel to convert
                var parcelDal = dal.printParcelsList().ToList();
                foreach (var p in parcelDal)
                { parcel.Add(GetParcel(p.ParcelId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return parcel;


        }
        #endregion
        #region GetUnmatchedParcelsList
        //returns list of unmatched parcels
        public List<IBL.BO.Parcel> GetUnmatchedParcelsList()
        {
            List<IBL.BO.Parcel> parcel = new List<IBL.BO.Parcel>();
            try
            {
                var parcelDal = dal.printParcelsList().ToList();
                //shortens list to unmatched parcel
                
                foreach (var p in parcelDal.Where(parcel => parcel.DroneId == 0))
                    //calls on get to convert
                { parcel.Add(GetParcel(p.ParcelId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException("no unmatched parcels exist\n"); }
            return parcel;


        }
        #endregion
        #region SendDroneToCharge
        //sends drone to charge at station
        public void SendDroneToCharge(int droneID) 
        {


            IBL.BO.Drone drone = new();
            IBL.BO.Station station = new();
            try
            {
                //ensures drone exists
                drone = GetDrone(droneID);
            }
            catch (IDAL.DO.DoesntExistException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            //ensures available
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("not available");
            //find closest sation to charge at
            Location stationLocation = ClosestStation(drone.location, false, StationLocationslist());
            station = GetStations().Find(x => x.location.Longitude == stationLocation.Longitude && x.location.Lattitude == stationLocation.Lattitude);
            int droneIndex = drones.ToList().FindIndex(x => x.droneId == droneID);
            //updates info
            if (station.chargeSlots > 0)
                station.decreaseChargeSlots();
            drones[droneIndex].battery -= MinBatteryRequired(drones[droneIndex].droneId);
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.maintenance;

            //try { DeleteDrone(droneID); }
            //catch (UnableToCompleteRequest exp) { throw new UnableToCompleteRequest(exp.Message); }
            //catch (IBL.BO.DoesntExistException exp)
            //{
            //    throw new IBL.BO.DoesntExistException(exp.Message);
            //}
            var temp = GetDrone(drones[droneIndex].droneId);
           // AddDrone(temp, station.StationId);
            IDAL.DO.DroneCharge DC = new DroneCharge { DroneId = droneID, StationId = station.StationId };
            dal.AddDroneCharge(DC);
        }
        #endregion
        #region GetDrone
        //returns drone
        public IBL.BO.Drone GetDrone(int id)
        {
            //ensures drone exists
            var drn = drones.Find(x => x.droneId == id);
            if (drn == null)
                throw new IBL.BO.DoesntExistException("The drone doesn't exist in system");
            //build ibl drone
            IBL.BO.Drone d = new IBL.BO.Drone();
            d.DroneId = drn.droneId;
            d.Model = drn.Model;
            d.MaxWeight = drn.weight;
            d.droneStatus = drn.droneStatus;
            d.battery = drn.battery;
            d.location = new IBL.BO.Location(0,0);
            d.location = drn.location;
            IBL.BO.ParcelInTransit pt = new IBL.BO.ParcelInTransit();
            if (drn.droneStatus == IBL.BO.DroneStatus.delivery && d.DroneId!=0)
            {
                pt.parcelId = drn.parcelId;
                IDAL.DO.Parcel p = new IDAL.DO.Parcel();
                try
                {
                    p = dal.GetParcel(drn.parcelId);
                }
                catch (Exception)
                {
                    throw new IBL.BO.DoesntExistException("The parcel doesn't exist in system");
                }
                if (p.PickedUp == DateTime.MinValue)
                    pt.parcelStatus = false;
                else
                    pt.parcelStatus = true;
                pt.priority = (IBL.BO.Priorities)p.Priority;
                pt.weight = (IBL.BO.WeightCategories)p.Weight;
                pt.sender = new IBL.BO.CustomerInParcel();
                pt.sender.CustomerId = GetCustomer(p.SenderId).CustomerId;
                pt.sender.CustomerName = GetCustomer(p.SenderId).Name;
                pt.target = new IBL.BO.CustomerInParcel();
                pt.target.CustomerId = GetCustomer(p.TargetId).CustomerId;
                pt.target.CustomerName = GetCustomer(p.TargetId).Name;
                IDAL.DO.Customer sender = dal.GetCustomer(p.SenderId);
                IDAL.DO.Customer target = dal.GetCustomer(p.TargetId);
                pt.pickupLocation = new IBL.BO.Location(sender.Lattitude, sender.Longitude);
                
                pt.targetLocation = new IBL.BO.Location(target.Lattitude,target.Longitude);
                
                pt.distance = Distance(pt.pickupLocation, pt.targetLocation);
                d.parcel = new IBL.BO.ParcelInTransit();
                d.parcel = pt;
            }
            return d;
        }
        #endregion
        #region returnsDrone
        //returns dronetolist
        public IBL.BO.DroneToList returnsDrone(int id)
        {
            DroneToList droneBo = new DroneToList();
            try
            {
               //gets drone form dal
                IDAL.DO.Drone droneDo = dal.GetDrone(id);
                DroneToList drone = drones.ToList().Find(d => d.droneId == id);
                //gets info
                droneBo.droneId = droneDo.DroneId;
                droneBo.Model = drone.Model;
                droneBo.weight = drone.weight;
                droneBo.location = drone.location;
                droneBo.battery = drone.battery;
                droneBo.droneStatus = drone.droneStatus;
                droneBo.numOfParcelsDelivered = drone.numOfParcelsDelivered;
                droneBo.numOfParcelsDelivered = dal.printParcelsList().Count(x => x.DroneId == droneBo.droneId);
                int parcelID = dal.printParcelsList().ToList().Find(x => x.DroneId == droneBo.droneId).ParcelId;
                droneBo.parcelId = parcelID;
                
            }
            catch (ArgumentNullException exp)
            {
                throw new IBL.BO.DoesntExistException(" \n");
            }
            catch (IDAL.DO.AlreadyExistException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            return droneBo;
        }
        #endregion







    }
}
                      


                   





                        


                    
                        

         
    

