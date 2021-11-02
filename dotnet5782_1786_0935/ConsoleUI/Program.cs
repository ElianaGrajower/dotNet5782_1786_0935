﻿using System;
using IDAL.DO;
using DAL.DalObject;


namespace ConsoleUI
{
    class Program
    {
        static DalObject Data;
        static void Main(string[] args)
        {
            Data = new DalObject();
            int choice;
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1- To add new item");
            Console.WriteLine("2- To update item");
            Console.WriteLine("3- To print item details");
            Console.WriteLine("4- To print list of items");
            choice = int.Parse(Console.ReadLine());
            while (choice != 5)
            {
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter your choice:\n" +
                                "A- add a station\n" +
                                "B- add a drone\n" +
                                "C- add a customer\n" +
                                "D- add a parcel");
                            char addingChoice = char.Parse(Console.ReadLine());
                            switch (addingChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter name of station: ");
                                        Station s = new Station() { Name = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter ID of station: ");
                                        s.id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter amount of charge slots that station has: ");
                                        s.ChargeSlots = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        s.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        s.Lattitude = double.Parse(Console.ReadLine());
                                        Data.AddStation(s);
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter name of model: ");
                                        Drone d = new Drone() { id = DalObject.r.Next(100000000, 999999999), Model = "Model-" + (Console.ReadLine()) };
                                        Console.WriteLine("Enter maximum weight drone can hold: ");
                                        d.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                        d.Battery = 100; 
                                        d.Status = DroneStatuses.available;
                                        Data.AddDrone(d);
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter name of customer: ");
                                        Customer c = new Customer() { Name = "Customer-" + (Console.ReadLine()) }; 
                                        Console.WriteLine("Enter Id of customer: ");
                                        c.id = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter phone number of customer: ");
                                        c.Phone = Console.ReadLine();
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        c.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        c.Lattitude = double.Parse(Console.ReadLine());
                                        Data.AddCustomer(c);
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter Parcel weight: ");
                                        Parcel p = new Parcel() { id = Data.getParcelId(), Weight = (WeightCategories)int.Parse(Console.ReadLine()), };
                                        Data.AddParcel(p);
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");   
                                    break;
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Enter your choice:\n" +
                                "A- match a parcel to a drone\n" +
                                "B- collect a parcel by drone\n" +
                                "C- deliver a parcel to a customer\n" +
                                "D- charge a drone\n" +
                                "E- unpug a charging drone\n" +
                                "F- find distance to charge station\n"+
                                "G- find distance from customer");
                            char updateChoice = char.Parse(Console.ReadLine());
                            switch (updateChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.matchUpParcel(Data.findParcel(parcelId)) + "\n");
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter customer id: ");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.pickUpParcel(Data.findCustomer(customerId), Data.findParcel(parcelId))); 
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter customer id: ");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter level of priority: ");
                                        int priorityLevel = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.deliverParcel(Data.findCustomer(customerId), Data.findParcel(parcelId), priorityLevel)); 
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter drone id: ");
                                        int droneId = int.Parse(Console.ReadLine());
                                        int stationNum;
                                        Data.printStationsList().ForEach(s => { if (s.ChargeSlots != 0) Console.WriteLine(s.ToString() + "\n"); });
                                        Console.WriteLine("Enter name of station you want to charge drone at:");
                                        stationNum = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.chargeDrone(Data.findDrone(droneId), stationNum)); 
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("Enter drone id: ");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.releaseDrone(Data.findDroneCharge(droneId))); 
                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("Enter langitude coordinates: ");
                                        double longitutde = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter latitude coordinates: ");
                                        double latitude= double.Parse(Console.ReadLine());
                                        Console.WriteLine("the distance is:");
                                        Data.printStationsList().ForEach(s => { Console.WriteLine(s.Name + ": " + Data.distance(s.Lattitude, s.Longitude, latitude, longitutde)); });
                                        break;
                                    }
                                case 'G':
                                    {
                                        Console.WriteLine("Enter langitude coordinates: ");
                                        double longitutde = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter latitude coordinates: ");
                                        double latitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("the distance is:");
                                        Data.printCustomersList().ForEach(s => { Console.WriteLine(s.Name + ": " + Data.distance(s.Lattitude, s.Longitude, latitude, longitutde)); });
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");
                                    break;
                            }
                            break;
                        }
                    case 3:
                        {
                            char Choice_Print;
                            int Id;
                            Console.WriteLine("Enter choice you want to print:");
                            Console.WriteLine("A- charge station");
                            Console.WriteLine("B- drone");
                            Console.WriteLine("C- customer");
                            Console.WriteLine("D- parcel");
                            Choice_Print = char.Parse(Console.ReadLine());
                            Console.Write("Enter ");
                            if (Choice_Print == 'A')
                                Console.Write("charge Station ");
                            if (Choice_Print == 'B')
                                Console.Write("drone ");
                            if (Choice_Print == 'C')
                                Console.Write("customer ");
                            if (Choice_Print == 'D')
                                Console.Write("pacel ");
                            Console.WriteLine("id: ");
                            Id = int.Parse(Console.ReadLine());
                            switch (Choice_Print)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("\n" + Data.PrintStation(Id).ToString() + "\n");
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("\n" + Data.PrintDrone(Id).ToString() + "\n");
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("\n" + Data.PrintCustomer(Id).ToString() + "\n");
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("\n" + Data.PrintParcel(Id).ToString() + "\n");
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");
                                    break;

                            }
                            break;
                        }
                    case 4:
                        {
                            char Choice_Print;
                            Console.WriteLine("Enter choice you want to print:");
                            Console.WriteLine("A- charge station list");
                            Console.WriteLine("B- drone list");
                            Console.WriteLine("C- customer list");
                            Console.WriteLine("D- parcel list");
                            Console.WriteLine("E- parcel that was not matched up to drone list");
                            Console.WriteLine("F- charge station with available charge list");

                            Choice_Print = char.Parse(Console.ReadLine());
                            switch (Choice_Print)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("List of stations:");
                                        Data.printStationsList().ForEach(s => Console.WriteLine(s.ToString() +"\n"));
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("List of drones:");
                                        Data.printDronesList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("List of customers:");
                                        Data.printCustomersList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("List of parcel:");
                                        Data.printParcelsList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("List of parcel not matched up with drones:");
                                        Data.printParcelsList().ForEach(s => { if (s.DroneId == 0) Console.WriteLine(s.ToString() + "\n"); });
                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("List of stations with available charging drone:");
                                        Data.printStationsList().ForEach(s => { if (s.ChargeSlots != 0) Console.WriteLine(s.ToString() + "\n"); });
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");
                                    break;
                            }
                            break;
                        }
                    default:
                        Console.WriteLine("ERROR INVALID CHOICE");
                        break;
                }
                Console.WriteLine("Choose from the following options:");
                Console.WriteLine("1- To add new item");
                Console.WriteLine("2- To update item");
                Console.WriteLine("3- To print item details");
                Console.WriteLine("4- To print list of items");
                Console.WriteLine("5- To exit");
                choice = int.Parse(Console.ReadLine());
            }
        }
    }
}
