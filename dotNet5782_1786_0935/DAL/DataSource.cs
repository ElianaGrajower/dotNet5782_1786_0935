using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    namespace DALObject
    {
        class DataSource
        {
            public static Random r = new Random();
            internal static List<Parcel> ParcelList = new List<Parcel>();
            internal static List<Drone> DroneList = new List<Drone>();
            internal static List<Station> StationList = new List<Station>();
            internal static List<Customer> CustomerList = new List<Customer>();
            internal class config
            {
                
            }
            public static void Initialize()
            {
                static void CreateStation()
                {
                    for (int i = 0; i < 2; i++)
                        StationList.Add(new Station()
                        {
                            id = r.Next(100000000, 999999999),
                            Name = r.Next(1, 5),
                            Longitude = (r.NextDouble() + r.Next(-90, 89)),   //getcoordinates(-90,90),
                            Lattitude= (r.NextDouble() + r.Next(-180, 179)),
                            ChargeSlots = r.Next(100000000, 999999999)

                        });
                    static void CreateDrone()
                    {
                        for (int i = 0; i < 5; i++)
                            DroneList.Add(new Drone()
                            {
                                id = r.Next(100000000, 999999999),
                                Model = "Model " + i,
                                MaxWeight = (IDAL.DO.WeightCategories)r.Next(1, 3),
                                Status=(IDAL.DO.DroneStatuses)r.Next(1, 3),
                            }
                            ); ;
                    }


                    static void CreateParcel()
                    {
                        for (int i = 0; i < 10; i++)
                            ParcelList.Add(new Parcel()
                            {
                                id = r.Next(100000000, 999999999),
                                SenderId = r.Next(100000000, 999999999),
                                TargetId = r.Next(100000000, 999999999),
                                DroneId = r.Next(100000000, 999999999),
                                Weight = (IDAL.DO.WeightCategories)r.Next(1, 3),
                                Priority = (IDAL.DO.Priorities)r.Next(1, 3),
                                Requested=DateTime.Now,
                                Scheduled=DateTime.Today,
                                PickedUp= DateTime.Today,
                                Delivered=DateTime.Today,

                    }
                            );
                    }

                }
                static void CreateCustomer()
                {
                    for (int i = 0; i < 10; i++)
                        CustomerList.Add(new Customer()
                        {
                            id = r.Next(100000000, 999999999),
                            Name = "Customer" + i,
                            Phone = "05"+r.Next(00000000, 99999999),
                            Longitude = (r.NextDouble() + r.Next(-90, 89)),   //getcoordinates(-90,90),
                            Lattitude = (r.NextDouble() + r.Next(-180, 179)),
                        });
                }
            }
        }
    }
}