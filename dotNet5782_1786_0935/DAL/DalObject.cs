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
                DataSource.StationList.ForEach(s=> { if (s.ChargeSlots >0) s.ToString(); });
            }
            public void printnotassigned()//prints all parcel not yet assigned to drone
            {
                DataSource.ParcelList.ForEach(s => { if (s.DroneId==0) s.ToString(); });

            }
            public void matchupparcel(Parcel p)//matches up package with drone
            {
                Drone d = new Drone();
                d=(DataSource.DroneList.Find(t=> t.Status == DroneStatuses.available));
                DataSource.DroneList.RemoveAll(m => m.id == d.id);
                p.DroneId = d.id;
                p.Scheduled = DateTime.Now;
                d.Status = DroneStatuses.delivery;
                DataSource.DroneList.Add(d);




            }
            public void pickupparcel(Customer c,Parcel p)//matches up packg with sender of pckg
            {
                p.SenderId = c.id;
                p.PickedUp = DateTime.Now;
                Drone d = new Drone();
                d = (DataSource.DroneList.Find(t => t.id== p.SenderId));
                d.Status = DroneStatuses.delivery;
                DataSource.DroneList.RemoveAll(m => m.id == d.id);
                DataSource.DroneList.Add(d);



            }
            public void deliverparcel(Customer c, Parcel p)//matches up parcel with buyer
            {
                p.TargetId = c.id;
                p.Delivered = DateTime.Now;
            }
            public void chargedrone(Drone d)//charges drone
            {
                d.Status = DroneStatuses.maintenance;
                d.Battery = 100;
                int i = 1;
                int m;
                Console.WriteLine("Enter number of station you want to charge drone at:");
                DataSource.StationList.ForEach(s => { if (s.ChargeSlots > 0) Console.WriteLine( i++ +": "+s.Name); });
                m = int.Parse(Console.ReadLine());
                DataSource.StationList.ForEach(s => { if (s.ChargeSlots > 0 && m == i++) s.ChargeSlots--; });
            }




        }
    }
}
