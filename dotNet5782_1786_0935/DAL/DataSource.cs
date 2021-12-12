using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    namespace DalObject
    {
        internal class DataSource
        {
            internal static List<Parcel> ParcelList = new List<Parcel>(); //a list of parcels
            internal static List<Drone> DroneList = new List<Drone>(); //a list of drones
            internal static List<Station> StationList = new List<Station>(); //a list of stations
            internal static List<Customer> CustomerList = new List<Customer>(); //a list of costumers
            internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>(); //a list of all the drones that are being charged
            internal class config
            {
                public static int assignParcelId = 1001; //assigns a random id to a parcel
                public static double available=0;
                public static double lightLoad=0.1;
                public static double mediumLoad=0.2;
                public static double heavyLoad=0.3;
                public static double chargeSpeed=0.3;

            }
            public static void Initialize()
            {
                CreateStation(); //creats a station with random information
                CreateDrone(); //creats a drone with random information
                CreateParcel(); //creats a parcel with random information
                CreateCustomer(); //creats a customer with random information

            }

            static void CreateStation() //creats a station with random information
            {
                for (int i = 0; i < 2; i++) //creates 2 stations with information
                    StationList.Add(new Station()
                    {
                        StationId = DalObject.r.Next(100000000, 999999999),
                        Name =  DalObject.r.Next(1, 99).ToString(),
                        Longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90) 
                        Lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                        ChargeSlots = DalObject.r.Next(1, 100)
                    });
            }
            static void CreateDrone() //creats a drone with random information
            {
                for (int i = 0; i < 5; i++) //creates 5 drones with information
                    DroneList.Add(new Drone()
                    {
                        DroneId = DalObject.r.Next(100000000, 999999999),
                        Model = "Model-" + i,
                        // Battery=100,
                        MaxWeight = (IDAL.DO.WeightCategories)DalObject.r.Next(1, 3), //chooses a max weight from light, average, heavy
                                                                                      //  Status = 0, //chooses a status from available, maintenance, delivery
                    });
            }

            static void CreateParcel() //creats a parcel with random information
            {
                for (int i = 0; i < 10; i++) //creates 10 parcels with information
                    ParcelList.Add(new Parcel()
                    {
                        ParcelId = config.assignParcelId++,
                        Weight = (IDAL.DO.WeightCategories)DalObject.r.Next(0, 2), //chooses a weight from light, average, heavy
                        Requested = DateTime.Now,

                    });
            }
            static void CreateCustomer() //creats a customer with random information
            {
                for (int i = 0; i < 10; i++) //creates 10 customers with information
                    CustomerList.Add(new Customer()
                    {
                        CustomerId = DalObject.r.Next(100000000, 999999999),
                        Name = "Customer-" + i,
                        Phone = "05" + DalObject.r.Next(00000000, 99999999),
                        Longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90)
                        Lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                    });
            }
        }
    }
}