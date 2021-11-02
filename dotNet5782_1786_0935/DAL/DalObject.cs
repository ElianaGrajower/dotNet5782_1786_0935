﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

//*************maybe we need to take this off and then the doube dalObject works************** 
namespace DAL
{
    namespace DalObject
    {
        public class DalObject
        {
            public DalObject() { DataSource.Initialize(); } // default constructer calls on initialize func
            public void AddStation(Station s) //adds station to list
            {
                DataSource.StationList.Add(s);
            }
            public void AddDrone(Drone d) //adds drone to list
            {
                DataSource.DroneList.Add(d);
            }
            public void AddCustomer(Customer c) //adds customer to listnn
            {
                DataSource.CustomerList.Add(c);
            }
            public void AddParcel(Parcel p) //adds parcel to list
            {
                DataSource.ParcelList.Add(p);
            }
            public List<Station> printStationsList() 
            {
                return DataSource.StationList;
            }
            public List<Drone> printDronesList() 
            {
                return DataSource.DroneList;
            }
            public List<Customer> printCustomersList()
            {
                return DataSource.CustomerList;
                
            }
            public List<Parcel> printParcelsList()
            {
                return DataSource.ParcelList; 
            }
            public List<Station> printAvailableCharge()
            {
                return DataSource.StationList;
               
            }
            
            public string matchUpParcel(Parcel p) //matches up package with drone
            {
                string empty = "your request was completed successfully";
                Drone d = new Drone();
                d = (DataSource.DroneList.Find(t => t.Status == DroneStatuses.available && t.MaxWeight >= p.Weight));
                DataSource.DroneList.RemoveAll(m => m.id == d.id);
                if (d.id != 0)
                {
                    p.DroneId = d.id;
                    p.Scheduled = DateTime.Now;
                    d.Status = DroneStatuses.delivery;
                    DataSource.DroneList.Add(d);
                    DataSource.ParcelList.RemoveAll(m => m.id == p.id);
                    DataSource.ParcelList.Add(p);
                    return empty;
                }
                else
                    return "NOT FOUND";
            }
            public string pickUpParcel(Customer c,Parcel p) //matches up packg with sender of pckg
            {
                string empty = "your request was completed successfully";
                p.SenderId = c.id;
                p.PickedUp = DateTime.Now;
                Drone d = new Drone();
                d = (DataSource.DroneList.Find(t => t.id == p.SenderId));
                if (d.id != 0 && p.id != 0)
                {
                    d.Status = DroneStatuses.delivery;
                    DataSource.DroneList.RemoveAll(m => m.id == d.id);
                    DataSource.DroneList.Add(d);
                    DataSource.ParcelList.RemoveAll(m => m.id == p.id);
                    DataSource.ParcelList.Add(p);
                    return empty;
                }
                else
                    return "NOT FOUND";
            }
            public string deliverParcel(Customer c, Parcel p, int priorityLevel) //matches up parcel with buyer
            {
                string empty = "your request was completed successfully";
                p.TargetId = c.id;
                p.Delivered = DateTime.Now;
                p.Priority = (Priorities)priorityLevel;
                if (p.id != 0)
                {
                    DataSource.ParcelList.RemoveAll(m => m.id == p.id);
                    DataSource.ParcelList.Add(p);
                    return empty;
                }
                else
                    return "NOT FOUND";
            }
            public string chargeDrone(Drone d,int stationNum) //charges drone
            {
                string empty = "your request was completed successfully";
                d.Status = DroneStatuses.maintenance;
                Station s = DataSource.StationList.Find(s => (s.Name == stationNum));
                if (s.id != 0)
                {
                    DataSource.StationList.Remove(s);
                    s.ChargeSlots--;
                    DroneCharge c = new DroneCharge();
                    c.DroneId = d.id;
                    c.StationId = s.id;
                    DataSource.DroneChargeList.Add(c);
                    DataSource.StationList.Add(s);
                    DataSource.DroneList.Add(d);
                    return empty;
                }
                else
                    return "NOT FOUND";
            }
            public string releaseDrone(DroneCharge c) //releases drone from charge
            {
                string empty = "your request was completed successfully";
                Drone d = DataSource.DroneList.Find(m => (m.id == c.DroneId));
                if (d.id != 0)
                {
                    d.Status = DroneStatuses.available;
                    d.Battery = 100;
                    DataSource.DroneList.RemoveAll(m => (m.id == c.DroneId));
                    DataSource.DroneList.Add(d);
                }
                else
                    return "NOT FOUND";
                Station s = DataSource.StationList.Find(s => (s.id == c.StationId));
                if (s.id != 0)
                {
                    s.ChargeSlots++;
                    DataSource.StationList.RemoveAll(s => (s.id == c.StationId));
                    DataSource.StationList.Add(s);
                }
                else
                    return "NOT FOUND";
                DataSource.DroneChargeList.Remove(c);
                return empty;

            }
            public string PrintStation(int stationId) //prints a station
            {
                return findStation(stationId).ToString();
            }
            public string PrintDrone(int droneId) //prints a drone
            {
                return findDrone(droneId).ToString();
            }
            public string PrintCustomer(int customerId) //prints a customer
            {            
                return findCustomer(customerId).ToString();
            }
            public string PrintParcel(int parcelId) //prints a parcel
            {
                return findParcel(parcelId).ToString();
            }
            public Parcel findParcel(int parcelId) //finds a parcel using its id
            {
                Parcel notFound = new Parcel();
                for (int i = 0; i < DataSource.ParcelList.Count(); i++)
                {
                    if (DataSource.ParcelList[i].id == parcelId)
                    {
                        return (DataSource.ParcelList[i]);
                    }
                }
                return notFound;
            }
            public Customer findCustomer(int customerId) //finds a customer using its id
            {
                Customer notFound = new Customer();
                for (int i = 0; i < DataSource.CustomerList.Count(); i++)
                {
                    if (DataSource.CustomerList[i].id == customerId)
                    {
                        return (DataSource.CustomerList[i]);
                    }
                }
                return notFound;
            }
            public Drone findDrone(int droneId) //finds a drone using its id
            {
                Drone notFound = new Drone();
                for (int i = 0; i < DataSource.DroneList.Count(); i++)
                {
                    if (DataSource.DroneList[i].id == droneId)
                    {
                        return (DataSource.DroneList[i]);
                    }
                }
                return notFound;
            }
            public Station findStation(int stationId) //finds a drone using its id
            {
                Station notFound = new Station();
                for (int i = 0; i < DataSource.StationList.Count(); i++)
                {
                    if (DataSource.StationList[i].id == stationId)
                    {
                        return (DataSource.StationList[i]);
                    }
                }
                return notFound;
            }
            public DroneCharge findDroneCharge(int droneChargeId) //finds a drone charge using its id
            {
                DroneCharge notFound = new DroneCharge();
                for (int i = 0; i < DataSource.DroneChargeList.Count(); i++)
                {
                    if (DataSource.DroneChargeList[i].DroneId == droneChargeId)
                    {
                        return (DataSource.DroneChargeList[i]);
                    }
                }
                return notFound;
            }
            public static Random r = new Random(); ///can we have this twice?????????????
            public int getParcelId()
            {
                return DataSource.config.assignParcelId.Next(100000000+DataSource.ParcelList.Count(), 999999999);
            }
            public double distance(double x1,double y1, double x2, double y2)
            {
                x1 = x1 - x2;
                y1 = y1 - y2;
                x1 = Math.Pow(x1, 2);
                y1 = Math.Pow(y1, 2);
                x1 = x1 + y1;
                return Math.Sqrt(x1);
            }
            
           

        }
    }
}
