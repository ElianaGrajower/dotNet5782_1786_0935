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
            public  void AddStation( )//adds station o list
            {
                Station s = buildstation();
               DataSource.StationList.Add(s);
            }
            public void AddDrone()//adds drone to list
            {
                Drone d = buildDrone();
                DataSource.DroneList.Add(d);

            }
            public void AddCustomer()//adds customer to list
            {
                Customer c=buildcustomer();
                DataSource.CustomerList.Add(c);

            }
            public void getParcel()//adds parcel to list
            {
                Parcel p = buildParcel();
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
                Console.WriteLine("Enter level of priority: ");
                p.Priority=(Priorities)int.Parse(Console.ReadLine());
            }
            public void chargedrone(Drone d)//charges drone
            {
                d.Status = DroneStatuses.maintenance;
                int m;
                Console.WriteLine("Enter name of station you want to charge drone at:");
                printavailablecharge();
                m = int.Parse(Console.ReadLine());
                DataSource.StationList.ForEach(s => { if (s.Name == m) s.ChargeSlots--; });
                DroneCharge c=new DroneCharge();
                c.DroneId = d.id;
                DataSource.StationList.ForEach(s => { if (s.Name == m) c.StationId = s.id; });
                DataSource.DroneChargeList.Add(c);

            }
            public void releasedrone(DroneCharge c)//releases drone from charge
            {
                DataSource.DroneList.ForEach(m => { if (m.id == c.DroneId) { m.Status = DroneStatuses.available; m.Battery = 100; } });
                DataSource.StationList.ForEach(s => { if (s.id == c.StationId) s.ChargeSlots++; });
                DataSource.DroneChargeList.Remove(c);


            }



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
            public Parcel buildParcel()
            {
                Console.WriteLine("Enter Parcel weight:");
                Parcel p = new Parcel() { id = DataSource.r.Next(999999999, 100000000), Weight = (WeightCategories)int.Parse(Console.ReadLine()), };
                return p;
            }
            public Customer buildcustomer()
            {
                Console.WriteLine("Enter name of customer: ");
                Customer c = new Customer() { id = DataSource.r.Next(999999999, 100000000), Name = (Console.ReadLine()) };
                Console.WriteLine("Enter phone number of customer: ");
                c.Phone = Console.ReadLine();
                Console.WriteLine("Enter Your longitude coordinates: ");
                c.Longitude = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter Your lattitude coordinates: ");
                c.Lattitude = double.Parse(Console.ReadLine());
                return c;


            }
            public Drone buildDrone()
            {
                Console.WriteLine("Enter name of model: ");
                Drone d = new Drone() { id = DataSource.r.Next(999999999, 100000000), Model = (Console.ReadLine()) };
                Console.WriteLine("Enter maximum weight drone can hold");
                d.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                d.Battery = 100;
                d.Status = DroneStatuses.available;
                return d;
            }
            public Station buildstation()
            {
                Console.WriteLine("Enter name of station: ");
                Station s = new Station() { id = DataSource.r.Next(999999999, 100000000), Name = int.Parse(Console.ReadLine()) };
                Console.WriteLine("Enter amount of charge slots that station has: ");
                s.ChargeSlots = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Your longitude coordinates: ");
                s.Longitude = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter Your lattitude coordinates: ");
                s.Lattitude = double.Parse(Console.ReadLine());
                return s;
            }

        }
    }
}
