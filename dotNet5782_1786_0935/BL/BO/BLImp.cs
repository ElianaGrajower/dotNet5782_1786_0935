using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DAL.DalObject;
using IBL.BO;

//write all the functions- its written in targil which one we need
//start with all the basic- just all the adds and then all the gets and then deletes
//build the bl constructer its the begining of part 2
//deal with all the exceptions also in the dl
//writye the main
//leave all the updates till the end
//change the names of print func in dal to get


namespace BL
{
    //namespace BLImp
    //{
    public class BLImp
    {
       // IDAL.DO.IDal dal;
       DAL.DalObject.DalObject dal;
        

        #region OnlyDigits
        public bool OnlyDigits(char x)
        {
            if (48 <= x && x <= 57)
                return true;
            return false;

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

        #region AddDrone
        public void AddDrone(IBL.BO.Drone DronetoAdd, int StationId)
        {
            Random rnd = new Random();
            if (DronetoAdd.DroneId <= 0)
                throw new IBL.BO.InvalidInputException("drone id not valid- must be a posittive\n");
            if (DronetoAdd.MaxWeight != IBL.BO.WeightCategories.light && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.average && DronetoAdd.MaxWeight != IBL.BO.WeightCategories.heavy)
                throw new IBL.BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");//should this be frased differently?
            DronetoAdd.battery = rnd.Next(20, 40);
            DronetoAdd.droneStatus = DroneStatus.maintenance;
            Location StationLocation = (dal.findStation(StationId).Lattitude, dal.findStation(StationId).Longitude);
            DronetoAdd.location = StationLocation;

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
                    Delivered=temp.Delivered
                    /////figure out when to add this
                    //Drone = new DroneInParcel()
                    //{
                    //    droneId = temp.DroneId,
                    //    //how!?!?!?!?1?!?!!?
                    //    // battery=dal.GetDrone(temp.DroneId).
                    //    //how in the world do i know his location
                    //    // location=new Location()

                    //}

                };
                return parcel;

            }
            catch(IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion

        #region GetStation
        public IBL.BO.Station GetStation(int stationId)
        {

            try
            {
                IDAL.DO.Station temp = dal.GetStation(stationId);
                IBL.BO.Station station = new IBL.BO.Station()
                {
                    StationId = temp.StationId,
                    name = temp.Name,
                    location = new Location(temp.Lattitude, temp.Lattitude)
                    {
                        Lattitude = temp.Lattitude,
                        Longitude = temp.Lattitude,
                    },
                    chargeSlots = temp.ChargeSlots,
                    DronesatStation = dal.printDroneChargeList().Where(station => station.StationId == stationId).Select(Station => new DroneInCharging()
                    {
                        droneId = Station.DroneId
                        //battery= dal.findDrone(Station.DroneId).
                        ///////not done
                    }
                };
                return station;
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
        }
        #endregion

        #region GetDrone
        /// <summary>
        /// ///redoooo
        /// </summary>
        /// <param name="DroneId"></param>
        /// <returns></returns>
        public IBL.BO.Drone GetDrone(int DroneId) /////////////not done
        {
            try
            {
                IDAL.DO.Drone temp = dal.GetDrone(DroneId);
                IBL.BO.Drone drone = new IBL.BO.Drone()
                {
                    DroneId = temp.DroneId,
                    Model = temp.Model,
                    MaxWeight = (IBL.BO.WeightCategories)((int)temp.MaxWeight),
                    battery = temp.battery,/////
                    ParcelDroneList = dal.printParcelsList().Select
                    (parcel => new ParcelDrone()
                    {
                        DroneId = parcel.DroneId,
                        ParcelId = parcel.ParcelId,
                        ParcelWeight = (IBL.BO.WeightCategories)parcel.Weight


                    }).Where(ParcelDrone => ParcelDrone.DroneId == DroneId),

                };
                return drone;
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }

        }
        #endregion

        private IEnumerable<DroneToList> droneToLists;
        public double[] chargeCapacity;
        public BLImp()
        { 
            dal = new DAL.DalObject.DalObject();
            chargeCapacity  = dal.ChargeCapacity();
            droneToLists = new List<IBL.BO.DroneToList>();
            bool flag = false;
            Random rand = new Random();
            double minBattery = 0;
            IEnumerable<IDAL.DO.Drone> drones = dal.printDronesList();
            IEnumerable<IDAL.DO.Parcel> parcels= dal.printParcelsList();
            foreach(var item in drones)
            {
                IBL.BO.DroneToList temp = new DroneToList();
                temp.droneId = item.DroneId;
                temp.Model = item.Model;
                temp.weight = (IBL.BO.WeightCategories)((int)item.MaxWeight);
                temp.parcelId= dal.printParcelsList().ToList().Find(x => x.DroneId == temp.droneId).DroneId;


            }

    

            
        }
}
                      


                   





                        


                    
                        

         
    
}
