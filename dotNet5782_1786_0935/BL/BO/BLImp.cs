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


namespace BL
{
    //namespace BLImp
    //{
    public class BLImp
    {
        //IDAL.DO.IDal dal;
        DAL.DalObject.DalObject dal;
        //public BLImp()
        //{
        //    dal = new DalObject();
        //}


        //build all exceptions to ensure that all drone info valid and logical
        //for example id positive numb of 9 digits

        public bool onlydigits(char x)
        {
            if (48 <= x && x <= 57)
                return true;
            return false;

        }

        //not remotley done
        public void AddCustomer(IBL.BO.Customer CustomertoAdd)
        {

            CustomertoAdd.ParcelsOrdered = new List<ParcelCustomer>();
            CustomertoAdd.ParcelsDelivered = new List<ParcelCustomer>();
            if (CustomertoAdd.CustomerId > 999999999 || CustomertoAdd.CustomerId < 100000000)
                throw new InvalidCastException("customer id not valid\n");
            if (!CustomertoAdd.Phone.All(onlydigits))
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
        public void AddParcel(IBL.BO.Parcel ParceltoAdd)
        {
            if (ParceltoAdd.ParcelId <= 0)
                throw new IBL.BO.InvalidInputException("parcel id not valid- must be a posittive\n");
            if (ParceltoAdd.SenderId > 999999999 || ParceltoAdd.SenderId < 100000000)
                throw new InvalidCastException("sender id not valid\n");
            if (ParceltoAdd.TargetId > 999999999 || ParceltoAdd.TargetId < 100000000)
                throw new InvalidCastException("target id not valid\n");
            if (ParceltoAdd.Weight != IBL.BO.WeightCategories.light && ParceltoAdd.Weight != IBL.BO.WeightCategories.average && ParceltoAdd.Weight != IBL.BO.WeightCategories.heavy)
                throw new IBL.BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");//should this be frased differently?

            IDAL.DO.Parcel newParcel = new IDAL.DO.Parcel()
            {
                ParcelId = ParceltoAdd.ParcelId,
                SenderId = ParceltoAdd.SenderId,
                TargetId = ParceltoAdd.TargetId,
                Weight = (IDAL.DO.WeightCategories)((int)ParceltoAdd.Weight),
                Priority = (IDAL.DO.Priorities)((int)ParceltoAdd.Priority),
                Fragile = ParceltoAdd.Fragile
                //do we need to add droneInfo to idal? should the times be in here?
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
                        droneId = Station.DroneId,
                        //battery= dal.findDrone(Station.DroneId).
                      
                    }
                };
                return station;
            }
            catch (IBL.BO.DoesntExistException exc)
            {
                throw exc;
            }
        }


        /// <summary>
        /// ///redoooo
        /// </summary>
        /// <param name="DroneId"></param>
        /// <returns></returns>
        public IBL.BO.Drone GetDrone(int DroneId)
        {
            try
            {
                IDAL.DO.Drone temp = dal.GetDrone(DroneId);
                IBL.BO.Drone drone = new IBL.BO.Drone()
                {
                    DroneId = temp.DroneId,
                    Model = temp.Model,
                    MaxWeight = (IBL.BO.WeightCategories)((int)temp.MaxWeight),
                    battery=temp.battery,/////
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

    }
}