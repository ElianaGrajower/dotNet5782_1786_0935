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
            internal static List<Parcel> ParcelList = new List<Parcel>();
            internal static List<Drone> DroneList = new List<Drone>();
            internal static List<Station> StationList = new List<Station>();
            internal static List<Customer> CustomerList = new List<Customer>();
            internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>();
            internal class config
            {
                public static Random assignParcelId = new Random();
            }
            public static void Initialize()
            {
                CreateStation();
                CreateCustomer();
                CreateDrone();
                CreateParcel();
            }
            static void CreateStation()
            {
                for (int i = 0; i < 2; i++)
                    StationList.Add(new Station()
                    {
                        id = DalObject.r.Next(100000000, 999999999),
                        Name = DalObject.r.Next(1, 5),
                        Longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)),   //getc
                                                                          //oordinates(-90,90),
                        Lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)),
                        ChargeSlots = DalObject.r.Next(1, 100)

                    });
            }
            static void CreateDrone()
            {
                for (int i = 0; i < 5; i++)
                    DroneList.Add(new Drone()
                    {
                        id = DalObject.r.Next(100000000, 999999999),
                        Model = "Model-" + i,
                        MaxWeight = (IDAL.DO.WeightCategories)DalObject.r.Next(1, 3),
                        Status = (IDAL.DO.DroneStatuses)DalObject.r.Next(1, 3),
                    });
            }
            static void CreateParcel()
            {
                for (int i = 0; i < 10; i++)
                    ParcelList.Add(new Parcel()
                    {
                        id = config.assignParcelId.Next(100000000 + ParcelList.Count(), 999999999),
                        SenderId = DalObject.r.Next(100000000, 999999999),
                        TargetId = DalObject.r.Next(100000000, 999999999),
                        DroneId = DalObject.r.Next(100000000, 999999999),
                        Weight = (IDAL.DO.WeightCategories)DalObject.r.Next(1, 3),
                        Priority = (IDAL.DO.Priorities)DalObject.r.Next(1, 3),
                        Requested = DateTime.Now,
                        Scheduled = DateTime.Today,
                        PickedUp = DateTime.Today,
                        Delivered = DateTime.Today,
                    });
            }
            static void CreateCustomer()
            {
                for (int i = 0; i < 10; i++)
                    CustomerList.Add(new Customer()
                    {
                        id = DalObject.r.Next(100000000, 999999999),
                        Name = "Customer-" + i,
                        Phone = "05" + DalObject.r.Next(00000000, 99999999),
                        Longitude = (DalObject.r.NextDouble() + DalObject.r.Next(-90, 89)),   //getcoordinates(-90,90),
                        Lattitude = (DalObject.r.NextDouble() + DalObject.r.Next(-180, 179)),
                    });
            }
        }
    }
}