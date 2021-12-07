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
        // IDAL.DO.IDal dal;
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

            StationtoAdd.DronesatStation = new List<DroneCharging>();
            StationtoAdd.DronesLeftStation = new List<PastCharges>();
            if (StationtoAdd.StationId <= 0)
                throw new IBL.BO.InvalidInputException("station id not valid- must be a posittive\n");//check error
            if (StationtoAdd.Location.Lattitude < 30.5 || StationtoAdd.Location.Lattitude > 34.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-lattitude coordinates out of range\n");
            if (StationtoAdd.Location.Longitude < 34.3 || StationtoAdd.Location.Longitude > 35.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-longitude coordinates out of range\n");
            if (StationtoAdd.ChargeSlots <= 0)
                throw new IBL.BO.InvalidInputException("invalid amount of chargeslots- must be a positive number");



            IDAL.DO.Station newStation = new IDAL.DO.Station()
            {
                StationId = StationtoAdd.StationId,
                Name = StationtoAdd.Name,
                Lattitude = StationtoAdd.Location.Lattitude,
                Longitude = StationtoAdd.Location.Longitude
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
        public void AddDrone(IBL.BO.Drone DronetoAdd)
        {
            DronetoAdd.ListofDroneStations = new List<StationDrone>();
            DronetoAdd.ListofDroneParcels = new List<DroneInParcel>();
            if(DronetoAdd.DroneId <= 0)
                throw new IBL.BO.InvalidInputException("drone id not valid- must be a posittive\n");
            if (DronetoAdd.MaxWeight!=IBL.BO.WeightCategories.light&& DronetoAdd.MaxWeight!=IBL.BO.WeightCategories.average&& DronetoAdd.MaxWeight != IBL.BO.WeightCategories.heavy) 
                throw new IBL.BO.InvalidInputException("invalid weight- must light(0),average(1) or heavy(2)");//should this be frased differently?

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


    }
    // }

}
