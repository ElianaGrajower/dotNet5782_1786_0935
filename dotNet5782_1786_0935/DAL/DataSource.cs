using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DAL
{
    namespace DalObject
    {
        internal static  class DataSource
        {
            internal static List<Parcel> ParcelList = new List<Parcel>(); //a list of parcels
            internal static List<Drone> DroneList = new List<Drone>(); //a list of drones
            internal static List<Station> StationList = new List<Station>(); //a list of stations
            internal static List<Customer> CustomerList = new List<Customer>(); //a list of costumers
            internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>(); //a list of all the drones that are being charged
            internal class config
            {
                public static int assignparcelId = 1001; //assigns a random id to a parcel
                public static double available=0;
                public static double lightLoad=0.01;
                public static double mediumLoad=0.02;
                public static double heavyLoad=0.03;
                public static double chargeSpeed=0.5;

            }
            public static void Initialize()
            {
                createStation(); //creats a station with random information
                createDrone(); //creats a drone with random information
                
                createCustomer(); //creats a customer with random information
                createParcel(); //creats a parcel with random information

            }

            static void createStation() //creats a station with random information
            {
                for (int i = 0; i < 2; i++) //creates 2 stations with information
                    StationList.Add(new Station()
                    {
                        stationId = DalObject.r.Next(100000000, 999999999),
                        name =  DalObject.r.Next(1, 99).ToString(),
                        longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90) 
                        lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                        chargeSlots = DalObject.r.Next(1, 100)
                    });
            }
            static void createDrone() //creats a drone with random information
            {
                for (int i = 0; i < 5; i++) //creates 5 drones with information
                    DroneList.Add(new Drone()
                    {
                        droneId = DalObject.r.Next(100000000, 999999999),
                        model = "Model-" + i,
                        // Battery=100,
                        maxWeight = (DO.weightCategories)DalObject.r.Next(1, 3), //chooses a max weight from light, average, heavy
                                                                                      //  Status = 0, //chooses a status from available, maintenance, delivery
                    });
            }

           
            static void createCustomer() //creats a customer with random information
            {
                for (int i = 0; i < 10; i++) //creates 10 customers with information
                    CustomerList.Add(new Customer()
                    {
                        customerId = DalObject.r.Next(100000000, 999999999),
                        name = "Customer-" + i,
                        Phone = "05" + DalObject.r.Next(00000000, 99999999),
                        longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90)
                        lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                    });
            }
            static void createParcel() //creats a parcel with random information
            {
                for (int i = 0; i < 10; i++) //creates 10 parcels with information
                    ParcelList.Add(new Parcel()
                    {
                        parcelId = config.assignparcelId++,
                        weight = (DO.weightCategories)DalObject.r.Next(0, 2), //chooses a weight from light, average, heavy
                        requested = DateTime.Now,
                        senderId = CustomerList[0].customerId,
                        targetId = CustomerList[4].customerId,
                        droneId = DroneList[1].droneId

                    });
            }
        }
    }
}