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
            internal static List<Parcel> parcelsList = new List<Parcel>();
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

                }
            }
        }
    }
}