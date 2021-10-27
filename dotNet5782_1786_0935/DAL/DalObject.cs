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
            public DalObject() { DataSource.Initialize(); }// default constructer calls on initialize func
            public  void AddStation(Station s )//adds station o list
            {
               DataSource.StationList.Add(s);
             
            }
            public void AddDrone(Drone d)//adds drone to list
            {
                DataSource.DroneList.Add(d);

            }
            public void AddCustomer(Customer c)//adds customer to list
            {
                DataSource.CustomerList.Add(c);

            }
            public void getParcel(Parcel p)//adds parcel to list
            {
                DataSource.ParcelList.Add(p);
            }
            public void printStations()
            {
                DataSource.StationList.ForEach(s => Console.WriteLine(s.ToString()));
            }
            public void printDrones()
            {
                DataSource.DroneList.ForEach(d => Console.WriteLine(d.ToString()));
            }
            public void printCustomers()
            {
                DataSource.CustomerList.ForEach(c => Console.WriteLine(c.ToString()));
            }
            public void printParcels()
            {
                DataSource.ParcelList.ForEach(p => Console.WriteLine(p.ToString()));
            }
            public void printavailablecharge()
            {
                DataSource.StationList.ForEach(s=> { if (s.ChargeSlots < 100) s.ToString(); });
            }
            public void printnotassigned()//prints all parcel not yet assigned to drone
            {
                DataSource.ParcelList.ForEach(s => { if (s.DroneId==0) s.ToString(); });

            }
            public void matchupparcel(Parcel p)//matches up package with drone
            {
                Drone d = new Drone();
                d=(DataSource.DroneList.Find(d => d.Status == DroneStatuses.available));
                DataSource.DroneList.Remove(m => m.id == d.id);
                p.DroneId = d.id;
                p.Scheduled = DateTime.Now;
                d.Status = DroneStatuses.delivery;


                
            }
            public void pickupparcel(Customer c,Parcel p)//matches up packg with sender of pckg
            {
                p.SenderId = c.id;
                p.PickedUp = DateTime.Now;
                Drone d = new Drone();
                d = (DataSource.DroneList.Find(d => d.id== p.SenderId));
                d.Status= DroneStatuses.available;


            }
            public void deliverparcel(Customer c, Parcel p)
            {
                p.TargetId = c.id;
                p.Delivered = DateTime.Now;
            }




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
