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
                Station s = buildstation();//creats a new station by calling on buildtation func
               DataSource.StationList.Add(s);//adds the station to list of station
            }
            public void AddDrone()//adds drone to list
            {
                Drone d = buildDrone();//creates new drone by calling on new drone func
                DataSource.DroneList.Add(d);//adds new drone to drone list

            }
            public void AddCustomer()//adds customer to listnn
            {
                Customer c=buildcustomer();//creates new customer by calling on build customer func
                DataSource.CustomerList.Add(c);//adds enw customer to customer list

            }
            public void getParcel()//adds parcel to list
            {
                Parcel p = buildParcel();//builds new parcel b ycalling on buikd parcel function
                DataSource.ParcelList.Add(p);//adds new parcel to build func
            }
            public void printStationsList()
            {
                DataSource.StationList.ForEach(s => Console.WriteLine(s.ToString()));
            }
            public void printDronesList()
            {
                DataSource.DroneList.ForEach(d => Console.WriteLine(d.ToString()));
            }
            public void printCustomersList()
            {
                DataSource.CustomerList.ForEach(c => Console.WriteLine(c.ToString()));
            }
            public void printParcelsList()
            {
                DataSource.ParcelList.ForEach(p => Console.WriteLine(p.ToString()));
            }
            public void printAvailableCharge()
            {
                DataSource.StationList.ForEach(s=> { if (s.ChargeSlots >0) s.ToString(); });
            }
            public void printNotAssigned()//prints all parcel not yet assigned to drone
            {
                DataSource.ParcelList.ForEach(s => { if (s.DroneId==0) s.ToString(); });
            }
            public void matchUpParcel(Parcel p)//matches up package with drone
            {
                Drone d = new Drone();
                d=(DataSource.DroneList.Find(t=> t.Status == DroneStatuses.available));
                DataSource.DroneList.RemoveAll(m => m.id == d.id);
                p.DroneId = d.id;
                p.Scheduled = DateTime.Now;
                d.Status = DroneStatuses.delivery;
                DataSource.DroneList.Add(d);
            }
            public void pickUpParcel(Customer c,Parcel p)//matches up packg with sender of pckg
            {
                p.SenderId = c.id;
                p.PickedUp = DateTime.Now;
                Drone d = new Drone();
                d = (DataSource.DroneList.Find(t => t.id== p.SenderId));
                d.Status = DroneStatuses.delivery;
                DataSource.DroneList.RemoveAll(m => m.id == d.id);
                DataSource.DroneList.Add(d);
            }
            public void deliverParcel(Customer c, Parcel p)//matches up parcel with buyer
            {
                p.TargetId = c.id;
                p.Delivered = DateTime.Now;
                Console.WriteLine("Enter level of priority: ");
                p.Priority=(Priorities)int.Parse(Console.ReadLine());
            }
            public void chargeDrone(Drone d)//charges drone
            {
                d.Status = DroneStatuses.maintenance;
                int m;
                Console.WriteLine("Enter name of station you want to charge drone at:");
                printAvailableCharge();
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
            public string PrintStation(int stationId) //prints a station
            {
                for (int i=0; i < DataSource.StationList.Count(); i++)
                {
                    if (DataSource.StationList[i].id == stationId)
                    {
                        return (DataSource.StationList[i].ToString());
                    }
                }
                return ("ERROR id doesn't match a station");
            }
            public string PrintDrone(int droneId) //prints a drone
            {
                for (int i = 0; i < DataSource.DroneList.Count(); i++)
                {
                    if (DataSource.DroneList[i].id == droneId)
                    {
                        return (DataSource.DroneList[i].ToString());
                    }
                }
                return ("ERROR id doesn't match a drone");
            }
            public string PrintCustomer(int customerId) //prints a customer
            {            
                for (int i = 0; i < DataSource.CustomerList.Count(); i++)
                {
                    if (DataSource.CustomerList[i].id == customerId) 
                    {
                        return (DataSource.CustomerList[i].ToString());
                    }
                }
                return ("ERROR id doesn't match a customer");
            }
            public string PrintParcel(int parcelId) //prints a parcel
            {
                for (int i = 0; i < DataSource.ParcelList.Count(); i++)
                {
                    if (DataSource.ParcelList[i].id == parcelId) 
                    {
                        return (DataSource.ParcelList[i].ToString());
                    }
                }
                return ("ERROR id doesn't match a parcel");
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
