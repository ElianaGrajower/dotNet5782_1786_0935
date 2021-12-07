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
        public  class BLImp
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
        public void AddDrone(IBL.BO.Drone drone)
        {

            IDAL.DO.Drone newDrone = new IDAL.DO.Drone()
            {
                DroneId = drone.DroneId,
                Model = drone.Model,
                MaxWeight = (IDAL.DO.WeightCategories)((int)drone.MaxWeight)
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
        public void AddCustomer(IBL.BO.Customer CustomertoAdd)
        {
           
            CustomertoAdd.ParcelsOrdered = new List<ParcelCustomer>();
            CustomertoAdd.ParcelsDelivered= new List<ParcelCustomer>();
            if (CustomertoAdd.CustomerId > 999999999 || CustomertoAdd.CustomerId < 100000000)
                throw new InvalidCastException("customer id not valid\n");
            if(!CustomertoAdd.Phone.All(onlydigits))
                throw new InvalidCastException("customer phone not valid- must contain only numbers\n");
            if(CustomertoAdd.Location.Lattitude<30.5 || CustomertoAdd.Location.Lattitude>34.5)
                throw new InvalidCastException("lattitude coordinates out of range\n");
            if (CustomertoAdd.Location.Longitude < 34.3 || CustomertoAdd.Location.Longitude > 35.5)
                throw new InvalidCastException("longitude coordinates out of range\n");



            IDAL.DO.Customer newCustomer = new IDAL.DO.Customer()
            {
                CustomerId = CustomertoAdd.CustomerId,
                Name= CustomertoAdd.Name,
               Phone= CustomertoAdd.Phone,
               Lattitude= CustomertoAdd.Location.Lattitude,
               Longitude= CustomertoAdd.Location.Longitude
            };
            try
            {

                dal.AddCustomer(newCustomer);
            }
            catch (AlreadyExistException exc)
            {
                throw  exc;
            }
        }
        public void AddStation(IBL.BO.Station StationtoAdd)
        {

            StationtoAdd.DronesatStation = new List<DroneCharging>();
            StationtoAdd.DronesLeftStation = new List<PastCharges>();
            if (StationtoAdd.StationId <=0)
                throw new IBL.BO.InvalidInputException("station id not valid- must be a posittive\n");//check error
            if (StationtoAdd.Location.Lattitude < 30.5 || StationtoAdd.Location.Lattitude > 34.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-lattitude coordinates out of range\n");
            if (StationtoAdd.Location.Longitude < 34.3 || StationtoAdd.Location.Longitude > 35.5)
                throw new IBL.BO.InvalidInputException("station coordinates not valid-longitude coordinates out of range\n");
            if (StationtoAdd.ChargeSlots <= 0)
                throw  new IBL.BO.InvalidInputException("invalid amount of chargeslots- must be a positive number");



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
                throw  exc;
            }
        }


    }
    // }

}
