using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using Dal;
using DalApi;


namespace DAL
{

    internal static class DataSource
    {   internal static string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J","1111aaa" };
        internal static string[] stationName = { "Raanana", "Tel Aviv" };
        internal static string[] droneName = { "Reaper", "Shadow", "Grey Eagle", "Global Hawk", "Pioneer", "Fire Scout", "Snowgoose", "Hunter", "Stalker", "GNAT", "Wing Loong II", "AVENGER", "Apollo Earthly", "AirHaven", "indRazer", "Godspeed", "Phantom", "Novotek", "Tri-Propeller", "WikiDrone" };
        internal static string[] customerName = { "Michael", "Hannah", "Fred", "Sam", "Tom", "Jessie", "George", "Tiffany", "Elizabeth", "Rachel" };
        internal static List<Parcel> ParcelList = new List<Parcel>(); //a list of parcels
        internal static List<Drone> DroneList = new List<Drone>(); //a list of drones
        internal static List<Station> StationList = new List<Station>(); //a list of stations
        internal static List<Customer> CustomerList = new List<Customer>(); //a list of costumers
        internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>(); //a list of all the drones that are being charged
        internal class config
        {
            public static int assignparcelId = 1001; //assigns a random id to a parcel
            public static double available = 0;
            public static double lightLoad = 0.01;
            public static double mediumLoad = 0.02;
            public static double heavyLoad = 0.03;
            public static double chargeSpeed = 0.5;

        }
        #region Initialize
        public static void Initialize()
        {
            createStation(); //creats a station with random information
            createDrone(); //creats a drone with random information
            createCustomer(); //creats a customer with random information
            createParcel(); //creats a parcel with random information

        }
        #endregion
        #region createStation
        static void createStation() //creats a station with random information
        {
            for (int i = 0; i < 2; i++) //creates 2 stations with information
                StationList.Add(new Station()
                {
                    stationId = DalObject.r.Next(100000000, 999999999),
                    name = stationName[i],
                    longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90) 
                    lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                    chargeSlots = DalObject.r.Next(1, 100),
                    active = true
                });
        }
        #endregion
        #region createDrone
        static void createDrone() //creats a drone with random information
        {
            for (int i = 0; i < 20; i++) //creates 5 drones with information
                DroneList.Add(new Drone()
                {
                    droneId = DalObject.r.Next(100000000, 999999999),
                    model = droneName[i],
                    maxWeight = (DO.weightCategories)DalObject.r.Next(1, 3),
                    active = true 
                });
        }
        #endregion
        #region createCustomer
        static void createCustomer() //creats a customer with random information
        {
            string p = letters[10];
            for (int i = 0; i < 8; i++) //creates 8 customers with information
                CustomerList.Add(new Customer()
                {
                    customerId = DalObject.r.Next(100000000, 999999999),
                    name = customerName[i],
                    Phone = "05" + DalObject.r.Next(00000000, 99999999),
                    longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90)
                    lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                    password = letters[i] + p,
                    isCustomer = true,
                    active=true
                }) ;
            for (int i = 8; i < 10; i++) //creates 2 workers
                CustomerList.Add(new Customer()
                {
                    customerId = DalObject.r.Next(100000000, 999999999),
                    name = customerName[i],
                    Phone = "05" + DalObject.r.Next(00000000, 99999999),
                    longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)), //gets coordinates for (-90 - 90)
                    lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)), //gets coordinates for (-180 - 180)
                    password = letters[i] + p,
                    isCustomer = false,
                    active = true
                });
        }
        #endregion
        #region createParcel
        static void createParcel() //creats a parcel with random information
        {
            for (int i = 0; i < 10; i++) //creates 10 parcels with information
                ParcelList.Add(new Parcel()
                {
                    parcelId = config.assignparcelId++,
                    weight = (DO.weightCategories)DalObject.r.Next(1, 3), //chooses a weight from light, average, heavy
                    priority = (DO.Priorities)DalObject.r.Next(1, 3),
                    requested = DateTime.Now,
                    scheduled = DateTime.Now,//this was just added its untested
                    pickedUp = DateTime.MinValue,
                    delivered = DateTime.MinValue,
                    senderId = CustomerList[i].customerId,
                    targetId = CustomerList[3].customerId,
                    droneId = DroneList[i].droneId,
                    active = true,


                });
        }
        #endregion
    }

}