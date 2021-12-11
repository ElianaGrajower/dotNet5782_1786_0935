using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL.DalObject;
using IBL.BO;

//deal with all the exceptions also in the dl
//writye the main
//change the names of print func in dal to get


namespace BL
{
    //namespace BLImp
    //{
    public class BLImp
    {
        // IDAL.DO.IDal dal;
        //private IEnumerable<DroneToList> droneToLists;
        public double[] chargeCapacity;
        private List<IBL.BO.Drone> drones;
        DAL.DalObject.DalObject dal;
        public static Random rand = new Random();
        #region GetChargeCapacity
        public chargeCapacity GetChargeCapacity()
        {
        
            double[] arr = dal.ChargeCapacity();
            var chargeCapacity=new chargeCapacity {  pwrLight=arr[0], pwrAverge= arr[1], pwrAvailable= arr[2],pwrHeavy= arr[3],pwrRateLoadingDrone= arr[4], chargeCapacityArr=arr};
            return chargeCapacity;
        }
        #endregion
        #region GetStationsList
        public List<IBL.BO.Station> GetStationsList()
        {
            List<IBL.BO.Station> baseStations = new List<IBL.BO.Station>();
            try
            {
                var stationsDal = dal.printStationsList().ToList();
                foreach (var s in stationsDal)
                { baseStations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return baseStations;
        }
        #endregion GetStationsList
        #region MinBatteryRequired
        private int MinBatteryRequired(int droneId)
        {
            var drone = GetDrone(droneId);
            if (drone.droneStatus == DroneStatus.available)
            {
                Location location = ClosestStation(drone.location, false, StationLocationslist());
                return (int)(GetChargeCapacity().chargeCapacityArr[(int)GetChargeCapacity().pwrAvailable] * Distance(drone.location, location));
            }

            if (drone.droneStatus == DroneStatus.delivery)
            {
                IDAL.DO.Parcel parcel = dal.GetParcel(drone.parcel.parcelId);
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
        #region getUsedChargingPorts
        private int getUsedChargingPorts(int StationId)
        {
            try
            {
                return dal.GetStation(StationId).ChargeSlots - dal.GetStation(StationId).availableChargeSlots;  //they did it AvailableChargingSlots (id)
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region FindStation
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
        public void AddDrone(IBL.BO.Drone DronetoAdd, int StationId)
        {
            if (DronetoAdd.DroneId <= 0)
                throw new IBL.BO.InvalidInputException("drone id not valid- must be a posittive\n");
            if (DronetoAdd.MaxWeight != IBL.BO.WeightCategories.light && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.average && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.heavy)
                throw new IBL.BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");//should this be frased differently?
            DronetoAdd.battery = rand.Next(20, 40);
            DronetoAdd.droneStatus = DroneStatus.maintenance;
            double StationLattitude = dal.findStation(StationId).Lattitude;
            double StationLongitude = dal.findStation(StationId).Longitude;
            DronetoAdd.location = new Location(StationLattitude, StationLongitude);

            IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            {
                DroneId = DronetoAdd.DroneId,
                Model = DronetoAdd.Model,
                MaxWeight = (IDAL.DO.WeightCategories)((int)DronetoAdd.MaxWeight)
            };
            try
            {
                dal.AddDrone(newDrone);
            }
            catch (AlreadyExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region OnlyDigits
        public bool OnlyDigits(char x)
        {
            if (48 <= x && x <= 57)
                return true;
            return false;

        }
        #endregion
        #region deg2rad
        private static double deg2rad(double val)
        {
            return (Math.PI / 180) * val;
        }
        #endregion
        #region Distance
        private double Distance(IBL.BO.Location l1, IBL.BO.Location l2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(l2.Lattitude - l1.Lattitude);  // deg2rad below
            var dLon = deg2rad(l2.Longitude - l1.Longitude);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(l1.Lattitude)) * Math.Cos(deg2rad(l2.Lattitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        #endregion
        #region GetStation
        public IBL.BO.Station GetStation(int stationId)
        {
            try
            {
                IBL.BO.Station station = new IBL.BO.Station();
                var tempStation = dal.GetStation(stationId);
                station.StationId = tempStation.StationId;
                station.name = tempStation.Name;
                station.chargeSlots = station.chargeSlots;
                station.location.Lattitude = tempStation.Lattitude;
                station.location.Longitude = tempStation.Longitude;
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
        public List<IBL.BO.Station> GetStations()
        {
            List<IBL.BO.Station> stations = new List<IBL.BO.Station>();
            try
            {
                var stationsDal = dal.printStationsList().ToList();
                foreach (var s in stationsDal)    
                { stations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return stations;
        }
        #endregion
        #region GetAvailableStationsList
        public List<IBL.BO.Station> GetAvailableStationsList()
        {
            List<IBL.BO.Station> stations = new List<IBL.BO.Station>();
            stations.Where(station => station.chargeSlots > 0);
            try
            {
                var stationsDal = dal.printStationsList().ToList();
                foreach (var s in stationsDal)
                {stations.Add(GetStation(s.StationId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return stations;
        }
        #endregion
        #region AvailableChargingSlots
        public int AvailableChargingSlots()
        {
            IBL.BO.Station station = new IBL.BO.Station();
            return station.chargeSlots;
        }
        #endregion
        #region StationLocationslist
        private List<Location> StationLocationslist()
        {
            List<Location> locations = new List<Location>();
            foreach (var station in GetStations())
            {
                locations.Add(new Location(station.location.Lattitude, station.location.Longitude));
                
            }
            return locations;
        }
        #endregion
        #region ClosestStation
        private Location ClosestStation(Location currentlocation, bool withChargeSlots, List<Location> l)//the function could also be used to check in addtion if the charge slots are more then 0
        {

            var locations = l;
            Location location = locations[0];
            double distance = Distance(locations[0], currentlocation);
            for (int i = 1; i < locations.Count; i++)
            {
                if (withChargeSlots)
                {
                    var station = GetStations().ToList().Find(x => x.location.Longitude == locations[i].Longitude && x.location.Longitude == locations[i].Longitude);
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
        private double getDroneBattery(int droneId)
        {
            return drones.ToList().Find(drone => drone.DroneId == droneId).battery;
        }
        #endregion
        #region getRandomCordinatesBL
        private static double getRandomCordinatesBL(double num1, double num2)
        {
            return (rand.NextDouble() * (num2 - num1) + num1);
        }
        #endregion
        #region AddCustomer
        public void AddCustomer(IBL.BO.Customer CustomertoAdd)
        {

            CustomertoAdd.ParcelsOrdered = new List<ParcelinCustomer>();
            CustomertoAdd.ParcelsDelivered = new List<ParcelinCustomer>();
            if (CustomertoAdd.CustomerId > 999999999 || CustomertoAdd.CustomerId < 100000000)
                throw new InvalidCastException("customer id not valid\n");
            if (!CustomertoAdd.Phone.All(OnlyDigits))
                throw new InvalidCastException("customer phone not valid- must contain only numbers\n");
            if (CustomertoAdd.Location.Lattitude < 30.5 || CustomertoAdd.Location.Lattitude > 34.5)
                throw new InvalidCastException("lattitude coordinates out of range\n");
            if (CustomertoAdd.Location.Longitude < 34.3 || CustomertoAdd.Location.Longitude > 35.5)
                throw new InvalidCastException("longitude coordinates out of range\n");



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

                dal.AddCustomer(newCustomer);
            }
            catch (AlreadyExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region AddStation
        public void AddStation(IBL.BO.Station StationtoAdd)
        {

            StationtoAdd.DronesatStation = new List<DroneInCharging>();

            if (StationtoAdd.StationId <= 0)
                throw new IBL.BO.InvalidInputException("station id not valid- must be a posittive\n");//check error
            if (StationtoAdd.location.Lattitude < 30.5 || StationtoAdd.location.Lattitude > 34.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-lattitude coordinates out of range\n");
            if (StationtoAdd.location.Longitude < 34.3 || StationtoAdd.location.Longitude > 35.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-longitude coordinates out of range\n");
            if (StationtoAdd.chargeSlots <= 0)
                throw new IBL.BO.InvalidInputException("invalid amount of chargeslots- must be a positive number");



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

                dal.AddStation(newStation);
            }
            catch (AlreadyExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region AddParcel
        public void AddParcel(IBL.BO.Parcel ParceltoAdd)
        {
            if (ParceltoAdd.ParcelId <= 0)
                throw new IBL.BO.InvalidInputException("parcel id not valid- must be a posittive\n");
            if (ParceltoAdd.Sender.CustomerId > 999999999 || ParceltoAdd.Sender.CustomerId < 100000000)
                throw new InvalidCastException("sender id not valid\n");
            if (ParceltoAdd.Target.CustomerId > 999999999 || ParceltoAdd.Target.CustomerId < 100000000)
                throw new InvalidCastException("target id not valid\n");
            if (ParceltoAdd.Weight != IBL.BO.WeightCategories.light && ParceltoAdd.Weight != IBL.BO.WeightCategories.average && ParceltoAdd.Weight != IBL.BO.WeightCategories.heavy)
                throw new IBL.BO.InvalidInputException("invalid weight- must light,average or heavy");
            ParceltoAdd.Requested = DateTime.Now;
            ParceltoAdd.Scheduled = new DateTime(0, 0);
            ParceltoAdd.PickedUp = new DateTime(0, 0);
            ParceltoAdd.Delivered = new DateTime(0, 0);
            ParceltoAdd.Drone.droneId = 0;
            IDAL.DO.Parcel newParcel = new IDAL.DO.Parcel()
            {
                ParcelId = ParceltoAdd.ParcelId,
                SenderId = ParceltoAdd.Sender.CustomerId,
                TargetId = ParceltoAdd.Target.CustomerId,
                Weight = (IDAL.DO.WeightCategories)((int)ParceltoAdd.Weight),
                Priority = (IDAL.DO.Priorities)((int)ParceltoAdd.Priority),
                DroneId = 0,
                Requested = ParceltoAdd.Requested,
                Scheduled = ParceltoAdd.Scheduled,
                PickedUp = ParceltoAdd.PickedUp,
                Delivered = ParceltoAdd.Delivered,
                Fragile = ParceltoAdd.Fragile,
            };
            try
            {
                dal.AddParcel(newParcel);
            }
            catch (AlreadyExistException exc)
            {
                throw exc;

            }
        }
        #endregion
        #region DeleteStation
        public void DeleteStation(int StationId)
        {
            try
            {
                dal.DeleteStation(StationId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteParcel
        public void DeleteParcel(int ParcelId)
        {
            try
            {
                dal.DeleteParcel(ParcelId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteCustomer
        public void DeleteCustomer(int CustomerId)
        {
            try
            {
                dal.DeleteCustomer(CustomerId);
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion
        #region DeleteDrone
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
        public IBL.BO.Customer GetCustomer(int customerId)
        {
            try
            {
                IDAL.DO.Customer temp = dal.GetCustomer(customerId);
                IBL.BO.Customer customer = new IBL.BO.Customer()
                {
                    CustomerId = temp.CustomerId,
                    Name = temp.Name,
                    Phone = temp.Phone,
                    Location = new Location(temp.Lattitude, temp.Lattitude)
                    {
                        Lattitude = temp.Lattitude,
                        Longitude = temp.Lattitude,

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
        }
        #endregion
        #region GetParcel
        public IBL.BO.Parcel GetParcel(int parcelId)
        {

            try
            {
                IDAL.DO.Parcel temp = dal.GetParcel(parcelId);
                IBL.BO.Parcel parcel = new IBL.BO.Parcel()
                {
                    ParcelId=temp.ParcelId,
                    Sender=new CustomerInParcel()
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
                    Requested=temp.Requested,
                    Scheduled=temp.Scheduled,
                    PickedUp=temp.PickedUp,
                    Delivered=temp.Delivered,
                    
                    Drone = new DroneInParcel()
                    {
                        droneId = temp.DroneId,
                        battery = getDroneBattery(temp.DroneId),
                        location = new Location(GetDrone(temp.DroneId).location.Lattitude, GetDrone(temp.DroneId).location.Longitude)

                    }




                    

                };
                return parcel;

            }
            catch(IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion
        #region BLImp
        public BLImp()
        {
            {
                dal = new DAL.DalObject.DalObject();
                drones = new List<IBL.BO.Drone>();
                bool flag = false;
                Random rnd = new Random();
                double minBatery = 0;
                IEnumerable<IDAL.DO.Drone> d = dal.printDronesList();
                IEnumerable<IDAL.DO.Parcel> parcels = dal.printParcelsList();
                chargeCapacity chargeCapacity = GetChargeCapacity();
                foreach (var item in d)
                {
                    IBL.BO.DroneToList drt = new DroneToList();
                    drt.droneId = item.DroneId;
                    drt.Model = item.Model;
                    drt.weight = (IBL.BO.WeightCategories)(int)item.MaxWeight;
                    drt.numOfParcelsDelivered = dal.printParcelsList().Count(x => x.DroneId == drt.droneId);
                    int parcelID = dal.printParcelsList().ToList().Find(x => x.DroneId == drt.droneId).ParcelId;
                    drt.parcelId = parcelID;
                    var baseStationLocations =StationLocationslist();
                    foreach (var pr in parcels)
                    {
                        if (pr.DroneId == item.DroneId && pr.Delivered == DateTime.MinValue)
                        {
                            IDAL.DO.Customer sender = dal.GetCustomer(pr.SenderId);
                            IDAL.DO.Customer target = dal.GetCustomer(pr.TargetId);
                            IBL.BO.Location senderLocation = new Location(sender.Lattitude, sender.Longitude );
                            IBL.BO.Location targetLocation = new Location ( target.Lattitude, target.Longitude );
                            drt.droneStatus = DroneStatus.delivery;
                            if (pr.PickedUp == DateTime.MinValue && pr.Scheduled != DateTime.MinValue)//החבילה שויכה אבל עדיין לא נאספה
                            {
                                drt.location = new Location( ClosestStation(senderLocation, false, baseStationLocations).Lattitude,ClosestStation(senderLocation, false, baseStationLocations).Longitude);
                                minBatery = Distance(drt.location, senderLocation) * chargeCapacity.chargeCapacityArr[0];
                                minBatery += Distance(senderLocation, targetLocation) * chargeCapacity.chargeCapacityArr[(int)pr.Weight];
                                minBatery += Distance(targetLocation, new Location(ClosestStation(targetLocation, false, baseStationLocations).Lattitude,   ClosestStation(targetLocation, false, baseStationLocations).Longitude )) * chargeCapacity.chargeCapacityArr[0];
                            }
                            if (pr.PickedUp != DateTime.MinValue && pr.Delivered == DateTime.MinValue)//החבילה נאספה אבל עדיין לא הגיעה ליעד
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

                            drt.battery = rnd.Next((int)minBatery, 101);/// 100//*/;
                        }

                    }
                    drones.Add(GetDrone(drt.droneId));

                    Console.WriteLine(drt.ToString());


                }
                

            }
            

        }
        #endregion
        #region UpdateStationName
        public void UpdateStationName(int stationId, int name)
        {
            var tempStation = GetStation(stationId);
            dal.DeleteStation(stationId);
            tempStation.name = name;
            AddStation(tempStation);
        }
        #endregion
        #region UpdateCustomerName
        public void UpdateCustomerName(int CustomerId,string name,string number)
        {
            var tempCustomer = GetCustomer(CustomerId);
            if (name != " ")
                tempCustomer.Name = name;
            if (number != " ")
                tempCustomer.Phone = number;
        }
        #endregion
        #region ReleaseDroneFromCharge
        public void ReleaseDroneFromCharge(int droneId,int chargeTime)
        {
            var tempDrone = GetDrone(droneId);
            if (tempDrone.droneStatus == DroneStatus.maintenance)
            {
                dal.DeleteDrone(tempDrone.DroneId);
                BatteryUsage usage = new BatteryUsage();
                tempDrone.battery = chargeTime * usage.chargeSpeed;
                AddDrone(tempDrone,FindStation(tempDrone.location));
                var possibleStation = GetStation(dal.printStationsList().ToList().Find(station => station.Lattitude == tempDrone.location.Lattitude && station.Longitude == tempDrone.location.Longitude).StationId);
                dal.DeleteStation(possibleStation.StationId);
                possibleStation.addChargeSlots();
                AddStation(possibleStation);
                dal.DeleteDroneCharge(droneId, possibleStation.StationId);
                
            }
            else
                throw (new UnableToCompleteRequest());
        }
        #endregion
        #region updateStation
        public void updateStation(int stationID, int AvlblDCharges, int Name = ' ')
        {
            try
            {
                IDAL.DO.Station stationDl = new IDAL.DO.Station();
                stationDl = dal.GetStation(stationID);
                if (Name !=  ' ')
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
        }
        #endregion
        #region findTheParcel
        private IDAL.DO.Parcel findTheParcel(IBL.BO.WeightCategories we, IBL.BO.Location a, double buttery, IDAL.DO.Priorities pri)
        {


            double d, x;
            IDAL.DO.Parcel theParcel = new IDAL.DO.Parcel();

            IBL.BO.Location loc = new IBL.BO.Location(0,0);
            IDAL.DO.Customer customer = new IDAL.DO.Customer();
            double far = 1000000;
            // bool flug = false;

            //השאילתא אחראית למצוא את כל החבילות בעדיפות המבוקשת
            var parcels = dal.printParcelsList();
            var tempParcel = from item in parcels
                             where item.Priority == pri
                             select item;

            foreach (var item in tempParcel)
            {
                customer = dal.GetCustomer(item.SenderId);
                loc.Lattitude = customer.Lattitude;
                loc.Longitude = customer.Longitude;
                chargeCapacity chargeCapacity = GetChargeCapacity();
                d = Distance(a, loc);//המרחק בין מיקום נוכחי למיקום השולח
                x = Distance(loc, new IBL.BO.Location (dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Longitude ));//המרחק בין מיקום שולח למיקום יעד
                double fromCusToSta = Distance(new IBL.BO.Location (dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Longitude ), ClosestStation(new IBL.BO.Location ( dal.GetCustomer(item.TargetId).Lattitude, dal.GetCustomer(item.TargetId).Lattitude ), false, StationLocationslist()));
                double butteryUse = x * chargeCapacity.chargeCapacityArr[(int)item.Weight] + fromCusToSta * chargeCapacity.chargeCapacityArr[0] + d * chargeCapacity.chargeCapacityArr[0];
                if (d < far && (buttery - butteryUse) > 0 && item.Scheduled == DateTime.MinValue && weight(we, (IBL.BO.WeightCategories)item.Weight) == true)
                {
                    far = d;
                    theParcel = item;
                    return theParcel;
                }
            }
            //if (v.Count() > 0)//if there is a parcel.priority. ....
            //flug = true;

            if (pri == IDAL.DO.Priorities.emergency)//אם לא מצא בעדיפות הכי גבוהה מחפש בעדיפות מתחתיה
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Priorities.fast);

            if (pri == IDAL.DO.Priorities.fast)
                theParcel = findTheParcel(we, a, buttery, IDAL.DO.Priorities.regular);
            if (theParcel.ParcelId == 0)
                throw new IBL.BO.DoesntExistException("ERROR! there is not a parcel that match to the drone ");
            return theParcel;
        }
        #endregion
        #region weight
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
        private int indexOfChargeCapacity(IDAL.DO.WeightCategories w)
        {
            if (w == IDAL.DO.WeightCategories.light)
                return 1;
            if (w == IDAL.DO.WeightCategories.heavy)
                return 2;
            if (w == IDAL.DO.WeightCategories.average)
                return 3;

            return 0;

        }
        #endregion
        #region MatchDroneWithPacrel
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
                int index = drones.FindIndex(x => x.DroneId == droneId);
                DeleteDrone(myDrone.DroneId);
                drones.RemoveAt(index);
                myDrone.droneStatus = DroneStatus.delivery;
                myDrone.parcel.parcelId = myParcel.ParcelId;
                AddDrone(myDrone, station.StationId);
            }
            catch (IBL.BO.DoesntExistException exp) { throw new IBL.BO.DoesntExistException(exp.Message); }

        }
        #endregion
        #region PickUpParcel
        public void PickUpParcel(int droneId)
        {
            var tempDrone = GetDrone(droneId);
            var tempParcel = GetParcel(tempDrone.parcel.parcelId);
            //var tempDrone = GetDrone(droneId);
            if (tempParcel.PickedUp==new DateTime(0,0))
            {
                dal.DeleteDrone(tempDrone.DroneId);
                BatteryUsage usage = new BatteryUsage();
                int amount = (int)tempParcel.Weight;
                if(amount==0)
                tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation)*usage.light;
                if (amount == 1)
                    tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation) * usage.medium;
                if (amount == 2)
                    tempDrone.battery -= Distance(tempDrone.location, tempDrone.parcel.pickupLocation) * usage.heavy;
                tempDrone.location.Lattitude = tempDrone.parcel.pickupLocation.Lattitude;
                tempDrone.location.Longitude = tempDrone.parcel.pickupLocation.Longitude;
                tempDrone.parcel.parcelStatus=true;
                AddDrone(tempDrone,FindStation(tempDrone.location));
                dal.DeleteParcel(tempParcel.ParcelId);
                tempParcel.PickedUp = DateTime.Now;
                AddParcel(tempParcel);

            }
            throw (new UnableToCompleteRequest());
        }
        #endregion
        #region DeliveredParcel
        public void DeliveredParcel(int droneId)
        {
            var tempDrone = GetDrone(droneId);
            var tempParcel = GetParcel(tempDrone.parcel.parcelId);
            if (tempParcel.Delivered == new DateTime(0, 0))
            {
                dal.DeleteDrone(tempDrone.DroneId);
                BatteryUsage usage = new BatteryUsage();
                int amount = (int)tempParcel.Weight;
                if (amount == 0)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.light;
                if (amount == 1)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.medium;
                if (amount == 2)
                    tempDrone.battery -= Distance(tempDrone.location, GetCustomer(tempDrone.parcel.target.CustomerId).Location) * usage.heavy;
                tempDrone.location.Lattitude = GetCustomer(tempDrone.parcel.target.CustomerId).Location.Lattitude;
                tempDrone.location.Longitude = GetCustomer(tempDrone.parcel.target.CustomerId).Location.Longitude;
                tempDrone.droneStatus = DroneStatus.available;
                AddDrone(tempDrone, FindStation(tempDrone.location));
                dal.DeleteParcel(tempParcel.ParcelId);
                tempParcel.Delivered = DateTime.Now;
            }
            throw (new UnableToCompleteRequest());
        }
        #endregion
        #region GetDroneList
        public List<IBL.BO.Drone> GetDronesList()
        {
            List<IBL.BO.Drone> drone = new List<IBL.BO.Drone>();
            try
            {
                var droneDal = dal.printDronesList().ToList();
                foreach (var d in droneDal)
                { drones.Add(GetDrone(d.DroneId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return drone;


        }
        #endregion  
        #region GetCustomersList
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
        public List<IBL.BO.Parcel> GetParcelsList()
        {
            List<IBL.BO.Parcel> parcel = new List<IBL.BO.Parcel>();
            try
            {
                var parcelDal = dal.printParcelsList().ToList();
                foreach (var p in parcelDal)
                { parcel.Add(GetParcel(p.ParcelId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return parcel;


        }
        #endregion
        #region GetUnmatchedParcelsList
        public List<IBL.BO.Parcel> GetUnmatchedParcelsList()
        {
            List<IBL.BO.Parcel> parcel = new List<IBL.BO.Parcel>();
            try
            {
                var parcelDal = dal.printParcelsList().ToList();
                parcelDal.Where(parcel => parcel.DroneId == 0);
                foreach (var p in parcelDal)
                { parcel.Add(GetParcel(p.ParcelId)); }
            }
            catch (ArgumentException) { throw new IBL.BO.DoesntExistException(); }
            return parcel;


        }
        #endregion
        #region SendDroneToCharge
        public void SendDroneToCharge(int droneID) //int StationID)//have to send the closest sation that has available sattions
        {

            IBL.BO.Drone drone = new();
            IBL.BO.Station station = new();
            try
            {
                drone = GetDrone(droneID);
            }
            catch (IDAL.DO.DoesntExistException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            if (drone.droneStatus != DroneStatus.available)
                throw new unavailableException("not available");
            Location stationLocation = ClosestStation(drone.location, false, StationLocationslist());//not sure where and what its from
            station = GetStations().Find(x => x.location.Longitude == stationLocation.Longitude && x.location.Lattitude == stationLocation.Lattitude);
            int droneIndex = drones.ToList().FindIndex(x => x.DroneId == droneID);
            //var droneBL=GetDrones().ToList().Find(x => x.id == droneID);
            if (station.chargeSlots > 0)
                station.decreaseChargeSlots();
            drones[droneIndex].battery = MinBatteryRequired(drones[droneIndex].DroneId);//not sure that if it needs to be 100%
            drones[droneIndex].location = station.location;
            drones[droneIndex].droneStatus = DroneStatus.maintenance;

            try { DeleteDrone(droneID); }
            catch (UnableToCompleteRequest exp) { throw new UnableToCompleteRequest(exp.Message); }
            catch (IBL.BO.DoesntExistException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            var temp=GetDrone(drones[droneIndex].DroneId);
            AddDrone(temp, station.StationId);
            IDAL.DO.DroneCharge DC = new DroneCharge { DroneId = droneID, StationId = station.StationId };
            dal.AddDroneCharge(DC);
        }
        #endregion
        #region GetDrone
        public IBL.BO.Drone GetDrone(int id)
        {
            try
            {
                IBL.BO.Drone droneBo = new IBL.BO.Drone();
                IDAL.DO.Drone droneDo = dal.GetDrone(id);
                var drone = drones.ToList().Find(d => d.DroneId == id);
                droneBo.DroneId = droneDo.DroneId;
                droneBo.Model = drone.Model;
                droneBo.MaxWeight = drone.MaxWeight;
                droneBo.location = drone.location;
                droneBo.battery = drone.battery;
                droneBo.droneStatus = drone.droneStatus;
               // droneBo.N = drone.numOfParcelsDelivered;
               // droneBo.numOfParcelsDelivered = dal.printParcelsList().Count(x => x.DroneId == droneBo.droneId);
                int parcelID = dal.printParcelsList().ToList().Find(x => x.DroneId == droneBo.DroneId).ParcelId;
                var tempParcel = GetParcel(parcelID);
                droneBo.parcel.parcelId = parcelID;
                if (tempParcel.Delivered != new DateTime(0, 0))
                    droneBo.parcel.parcelStatus = true;
                else
                    droneBo.parcel.parcelStatus = false;
                droneBo.parcel.priority = tempParcel.Priority;
                droneBo.parcel.weight = tempParcel.Weight;
                droneBo.parcel.sender = tempParcel.Sender;
                droneBo.parcel.target = tempParcel.Target;
                droneBo.parcel.pickupLocation = GetCustomer(tempParcel.Sender.CustomerId).Location;
                droneBo.parcel.targetLocation = GetCustomer(tempParcel.Target.CustomerId).Location;
                droneBo.parcel.distance = Distance(GetCustomer(tempParcel.Sender.CustomerId).Location, GetCustomer(tempParcel.Target.CustomerId).Location);
                return droneBo;
            }
            catch (ArgumentNullException exp)
            {
                throw new IBL.BO.DoesntExistException(exp.Message);
            }
            catch (IBL.BO.DoesntExistException exp)
            {
                throw exp;
            }
        }
        #endregion








    }
}
                      


                   





                        


                    
                        

         
    

