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
            public static Random r = new Random();
            internal static List<Parcel> ParcelList = new List<Parcel>();
            internal static List<Drone> DroneList = new List<Drone>();
            internal static List<Station> StationList = new List<Station>();
            internal static List<Customer> CustomerList = new List<Customer>();
            internal static List<DroneCharge> DroneChargeList = new List<DroneCharge>();
            internal class config
            {
                
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
                            id = r.Next(999999999, 100000000),
                            Name = r.Next(1, 5),
                            Longitude = (r.NextDouble() + r.Next(89, -90)),   //getcoordinates(-90,90),
                            Lattitude = (r.NextDouble() + r.Next(179, -180)),
                            ChargeSlots = r.Next(1, 100)

                        });
                }
                static void CreateDrone()
                {
                    for (int i = 0; i < 5; i++)
                        DroneList.Add(new Drone()
                        {
                            id = r.Next(999999999, 100000000),
                            Model = "Model " + i,
                            MaxWeight = (IDAL.DO.WeightCategories)r.Next(3, 1),
                            Status = (IDAL.DO.DroneStatuses)r.Next(3, 1),
                        });
                }
                static void CreateParcel()
                {
                    for (int i = 0; i < 10; i++)
                        ParcelList.Add(new Parcel()
                        {
                            id = r.Next(999999999, 100000000 + ParcelList.Count()),
                            SenderId = r.Next(999999999, 100000000),
                            TargetId = r.Next(999999999, 100000000),
                            DroneId = r.Next(999999999, 100000000),
                            Weight = (IDAL.DO.WeightCategories)r.Next(3, 1),
                            Priority = (IDAL.DO.Priorities)r.Next(3, 1),
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
                            id = r.Next(999999999, 100000000),
                            Name = "Customer" + i,
                            Phone = "05" + r.Next(99999999, 00000000),
                            Longitude = (r.NextDouble() + r.Next(89, -90)),   //getcoordinates(-90,90),
                            Lattitude = (r.NextDouble() + r.Next(179, -180)),
                        });
                }
            //}
            
        }
    }
}