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
        public class DalObject
        {
            public DalObject() { DataSource.Initialize(); }

            


            public void PrintStation()
            {
                Console.WriteLine("enter a ststions id: ");
                int stationId = int.Parse(Console.ReadLine());
                for (int i=0; i < DataSource.StationList.Count(); i++)
                {
                    if (DataSource.StationList[i].id == stationId)
                    {
                        Console.WriteLine(DataSource.StationList[i].ToString());
                        return;
                    }
                }
            }
            public void PrintDrone()
            {
                Console.WriteLine("enter a drones id: ");
                int droneId = int.Parse(Console.ReadLine());
                for (int i = 0; i < DataSource.DroneList.Count(); i++)
                {
                    if (DataSource.DroneList[i].id == droneId)
                    {
                        Console.WriteLine(DataSource.DroneList[i].ToString());
                        return;
                    }
                }
            }
            public void PrintCustomer()
            {
                Console.WriteLine("enter a customer id: ");
                int customerId = int.Parse(Console.ReadLine());
                for (int i = 0; i < DataSource.CustomerList.Count(); i++)
                {
                    if (DataSource.CustomerList[i].id == customerId) 
                    {
                        Console.WriteLine(DataSource.CustomerList[i].ToString());
                        return;
                    }
                }
            }
            public void PrintParcel()
            {
                Console.WriteLine("enter a parcel id: ");
                int parcelId = int.Parse(Console.ReadLine());
                for (int i = 0; i < DataSource.ParcelList.Count(); i++)
                {
                    if (DataSource.ParcelList[i].id == parcelId) 
                    {
                        Console.WriteLine(DataSource.ParcelList[i].ToString());
                        return;
                    }
                }
            }

        }
    }
}
