using System;
using IDAL.DO;
using DAL.DalObject;


namespace ConsoleUI
{
    class Program
    {
        static DalObject Data;
        static void Main(string[] args)
        {
            Data = new DalObject(); //creates an object of DalObject type
            int choice;
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1- To add new item");
            Console.WriteLine("2- To update item");
            Console.WriteLine("3- To print item details");
            Console.WriteLine("4- To print list of items");
            choice = int.Parse(Console.ReadLine());
            while (choice != 5) //loops untill 5 is chosen then the program ends
            {
                switch (choice) //chooses what activaty to do
                {
                    case 1: //adds to a item
                        {
                            Console.WriteLine("Enter your choice:\n" +
                                "A- add a station\n" +
                                "B- add a drone\n" +
                                "C- add a customer\n" +
                                "D- add a parcel");
                            char addingChoice = char.Parse(Console.ReadLine());
                            switch (addingChoice) //chooses what item to add 
                            {
                                case 'A': //adds a station
                                    {
                                        Console.WriteLine("Enter name of station: ");
                                        Station s = new Station() { Name = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter ID of station: ");
                                        s.StationId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter amount of charge slots that station has: ");
                                        s.ChargeSlots = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        s.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        s.Lattitude = double.Parse(Console.ReadLine());
                                        Data.AddStation(s); //builds and adds a station using the information the user provided
                                        break;
                                    }
                                case 'B': //adds a drone
                                    {
                                        Console.WriteLine("Enter name of model: ");
                                        Drone d = new Drone() { DroneId = DalObject.r.Next(100000000, 999999999), Model = "Model-" + (Console.ReadLine()) };
                                        Console.WriteLine("Enter maximum weight drone can hold: ");
                                        d.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                        d.Battery = 100; 
                                        d.Status = DroneStatuses.available;
                                        Data.AddDrone(d); //builds and adds a drone using the information the user provided
                                        break;
                                    }
                                case 'C': //adds a customer
                                    {
                                        Console.WriteLine("Enter name of customer: ");
                                        Customer c = new Customer() { Name = "Customer-" + (Console.ReadLine()) }; 
                                        Console.WriteLine("Enter Id of customer: ");
                                        c.CustomerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter phone number of customer: ");
                                        c.Phone = Console.ReadLine();
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        c.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        c.Lattitude = double.Parse(Console.ReadLine());
                                        Data.AddCustomer(c); //builds and adds a customer using the information the user provided
                                        break;
                                    }
                                case 'D': //adds a parcel
                                    {
                                        Console.WriteLine("Enter Parcel weight: ");
                                        Parcel p = new Parcel() { ParcelId = Data.getParcelId(), Weight = (WeightCategories)int.Parse(Console.ReadLine()), Requested = DateTime.Now, };
                                        Data.AddParcel(p); //builds and adds a parcel using the information the user provided
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");   
                                    break;
                            }
                            break;
                        }
                    case 2: //update items
                        {
                            Console.WriteLine("Enter your choice:\n" +
                                "A- match a parcel to a drone\n" +
                                "B- collect a parcel by drone\n" +
                                "C- deliver a parcel to a customer\n" +
                                "D- charge a drone\n" +
                                "E- unplug a charging drone\n" +
                                "F- find distance to charge station\n"+
                                "G- find distance from customer");
                            char updateChoice = char.Parse(Console.ReadLine());
                            switch (updateChoice) //chooses what to update
                            {
                                case 'A': //matchs a parcel to a drone
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.matchUpParcel(Data.findParcel(parcelId)) + "\n"); //matches and prints if completed successfully
                                        break;
                                    }
                                case 'B': //collects a parcel by drone
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter customer id: ");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.pickUpParcel(Data.findCustomer(customerId), Data.findParcel(parcelId))); //collects and prints if completed successfully
                                        break;
                                    }
                                case 'C': //delivers a parcel to a customer
                                    {
                                        Console.WriteLine("Enter parcel id: ");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter customer id: ");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter level of priority: ");
                                        int priorityLevel = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.deliverParcel(Data.findCustomer(customerId), Data.findParcel(parcelId), priorityLevel)); //delivers and prints if completed successfully
                                        break;
                                    }
                                case 'D': //charges a drone
                                    {
                                        Console.WriteLine("Enter drone id: ");
                                        int droneId = int.Parse(Console.ReadLine());
                                        int stationNum;
                                        Console.WriteLine("List of available charging sttaions:");
                                        Data.printStationsList().ForEach(s => { if (s.ChargeSlots != 0) Console.WriteLine(s.ToString() + "\n"); }); //prints list of available charging stations
                                        Console.WriteLine("Enter name of station you want to charge drone at:");
                                        stationNum = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.chargeDrone(Data.findDrone(droneId), stationNum)); //charges and prints if completed successfully
                                        break;
                                    }
                                case 'E': //unplugs a charging drone
                                    {
                                        Console.WriteLine("Enter drone id: ");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine(Data.releaseDrone(Data.findDroneCharge(droneId))); //unplugs and prints if completed successfully
                                        break;
                                    }
                                case 'F': //finds distance to charge station
                                    {
                                        Console.WriteLine("Enter langitude coordinates: ");
                                        double longitutde = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter latitude coordinates: ");
                                        double latitude= double.Parse(Console.ReadLine());
                                        Console.WriteLine("the distance is:");
                                        Data.printStationsList().ForEach(s => { Console.WriteLine(s.Name + ": " + Data.distance(s.Lattitude, s.Longitude, latitude, longitutde)); }); //prints the distance
                                        break;
                                    }
                                case 'G': //finds distance from customer
                                    {
                                        Console.WriteLine("Enter langitude coordinates: ");
                                        double longitutde = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter latitude coordinates: ");
                                        double latitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("the distance is:");
                                        Data.printCustomersList().ForEach(s => { Console.WriteLine(s.Name + ": " + Data.distance(s.Lattitude, s.Longitude, latitude, longitutde)); }); //prints the distances
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");
                                    break;
                            }
                            break;
                        }
                    case 3: //prints item details
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
                            switch (Choice_Print) //chooses an item
                            {
                                case 'A': //prints a charge stations details
                                    {
                                        Console.WriteLine("\n" + Data.PrintStation(Id) + "\n");
                                        break;
                                    }
                                case 'B': //prints a drones details
                                    {
                                        Console.WriteLine("\n" + Data.PrintDrone(Id) + "\n");
                                        break;
                                    }
                                case 'C': //prints a customers details
                                    {
                                        Console.WriteLine("\n" + Data.PrintCustomer(Id) + "\n");
                                        break;
                                    }
                                case 'D': //prints a parcels details
                                    {
                                        Console.WriteLine("\n" + Data.PrintParcel(Id) + "\n");
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR CHOICE NOT VALID");
                                    break;

                            }
                            break;
                        }
                    case 4: //prints list of items
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
                            switch (Choice_Print) //chooses an item
                            {
                                case 'A': //prints charge station list
                                    {
                                        Console.WriteLine("List of stations:\n");
                                        Data.printStationsList().ForEach(s => Console.WriteLine(s.ToString() +"\n"));
                                        break;
                                    }
                                case 'B': //prints drone list
                                    {
                                        Console.WriteLine("List of drones:\n");
                                        Data.printDronesList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'C': //prints customer list
                                    {
                                        Console.WriteLine("List of customers:\n");
                                        Data.printCustomersList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'D': //prints parcel list
                                    {
                                        Console.WriteLine("List of parcel:\n");
                                        Data.printParcelsList().ForEach(s => Console.WriteLine(s.ToString() + "\n"));
                                        break;
                                    }
                                case 'E': //prints parcel that was not matched up to drone list
                                    {
                                        Parcel check = Data.printParcelsList().Find(s => s.DroneId == 0);
                                        if (check.ParcelId == 0)
                                        {
                                            Console.WriteLine("All parcels matched up to drone");
                                            break;
                                        }
                                        Console.WriteLine("List of parcel not matched up with drones:\n");
                                        Data.printParcelsList().ForEach(s => { if (s.DroneId == 0) Console.WriteLine(s.ToString() + "\n"); });
                                        break;
                                    }
                                case 'F': //prints charge station with available charge list
                                    {
                                        Station check = Data.printStationsList().Find(s => s.ChargeSlots != 0);
                                        if (check.StationId==0)
                                        {
                                            Console.WriteLine("No available charge slots at any station");
                                            break;
                                        }
                                        Console.WriteLine("List of stations with available charging drone:\n");
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
