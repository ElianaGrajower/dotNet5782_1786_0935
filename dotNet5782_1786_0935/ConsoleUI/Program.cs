//using System;
//using DO;
//using DAL;
//////Adina Schulman 328620935
//////Eliana Grajower 328781786
//////this code allows you to build drone,parcel,station ,and customer and use different functions to use them





namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)
        {

        }
    }
}
////        {
////            Data = new DalObject(); //creates an object of DalObject type
////            int chosen;
////            Console.WriteLine("Choose from the following options:");
////            Console.WriteLine("1- To add new item");
////            Console.WriteLine("2- To update item");
////            Console.WriteLine("3- To print item details");
////            Console.WriteLine("4- To print list of items");
////            chosen = int.Parse(Console.ReadLine());
////            while (chosen != 5) //loops untill 5 is chosen then the program ends
////            {
////                switch (chosen) //chooses what activaty to do
////                {
////                    case 1: //adds to a item
////                        {
////                            Console.WriteLine("Enter your choice:\n" +
////                                "A- add a station\n" +
////                                "B- add a drone\n" +
////                                "C- add a customer\n" +
////                                "D- add a parcel");
////                            string addingChoice = (Console.ReadLine());
////                            try
////                            { 
////                              // string addingChoice = (Console.ReadLine()); 
////                                if(addingChoice.Length!=1)
////                                    throw new InvalidInputException("Choice must be from following menu!\n");

        ////                            }
        ////                            catch(InvalidInputException exc)
        ////                            {
        ////                                Console.WriteLine(exc);
        ////                            }
        ////                            char Mychoice = addingChoice[0];
        ////                            switch (Mychoice) //chooses what item to add 
        ////                            {
        ////                                case 'A': //adds a station
        ////                                    {
        ////                                        Console.WriteLine("Enter name of station: ");
        ////                                        Station s = new Station() { name =Console.ReadLine() };
        ////                                        Console.WriteLine("Enter ID of station: ");
        ////                                        s.stationId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter amount of charge slots that station has: ");
        ////                                        s.chargeSlots = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter Your longitude coordinates: ");
        ////                                        s.longitude = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter Your lattitude coordinates: ");
        ////                                        s.lattitude = double.Parse(Console.ReadLine());
        ////                                        Data.AddStation(s); //builds and adds a station using the information the user provided

        ////                                        break;
        ////                                    }
        ////                                case 'B': //adds a drone
        ////                                    {
        ////                                        Console.WriteLine("Enter name of model: ");
        ////                                        Drone d = new Drone() { droneId = DalObject.r.Next(100000000, 999999999), model = "Model-" + (Console.ReadLine()) };
        ////                                        Console.WriteLine("Enter maximum weight drone can hold: ");
        ////                                        d.maxWeight = (weightCategories)int.Parse(Console.ReadLine());
        ////                                        //  d.Battery = 100; 
        ////                                        // d.Status = DroneStatuses.available;
        ////                                        Data.AddDrone(d); //builds and adds a drone using the information the user provided
        ////                                        Console.WriteLine("drone id: " + d.droneId + "\n");
        ////                                        break;
        ////                                    }
        ////                                case 'C': //adds a customer
        ////                                    {
        ////                                        Console.WriteLine("Enter name of customer: ");
        ////                                        Customer c = new Customer() { name = "Customer-" + (Console.ReadLine()) };
        ////                                        Console.WriteLine("Enter Id of customer: ");
        ////                                        c.customerId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter phone number of customer: ");
        ////                                        c.Phone = Console.ReadLine();
        ////                                        Console.WriteLine("Enter Your longitude coordinates: ");
        ////                                        c.longitude = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter Your lattitude coordinates: ");
        ////                                        c.lattitude = double.Parse(Console.ReadLine());
        ////                                        Data.AddCustomer(c); //builds and adds a customer using the information the user provided
        ////                                        break;
        ////                                    }
        ////                                case 'D': //adds a parcel
        ////                                    {
        ////                                        Console.WriteLine("Enter Parcel weight: ");
        ////                                        Parcel p = new Parcel() { parcelId = Data.getParcelId(), weight = (weightCategories)int.Parse(Console.ReadLine()), requested = DateTime.Now, };
        ////                                        Data.AddParcel(p); //builds and adds a parcel using the information the user provided
        ////                                        Console.WriteLine("parcel id: " + p.parcelId + "\n");
        ////                                        break;
        ////                                    }
        ////                                default:
        ////                                    Console.WriteLine("ERROR CHOICE NOT VALID");
        ////                                    break;
        ////                            }
        ////                            break;
        ////                        }
        ////                    case 2: //update items
        ////                        {
        ////                            Console.WriteLine("Enter your choice:\n" +
        ////                                "A- match a parcel to a drone\n" +
        ////                                "B- collect a parcel by drone\n" +
        ////                                "C- deliver a parcel to a customer\n" +
        ////                                "D- charge a drone\n" +
        ////                                "E- unplug a charging drone\n" +
        ////                                "F- find distance to charge station\n" +
        ////                                "G- find distance from customer");
        ////                           // char updateChoice = char.Parse(Console.ReadLine());
        ////                            string updateChoice = (Console.ReadLine());
        ////                            try
        ////                            {
        ////                                // string addingChoice = (Console.ReadLine()); 
        ////                                if (updateChoice.Length != 1)
        ////                                    throw new InvalidInputException("Choice must be from following menu!\n");

        ////                            }
        ////                            catch (InvalidInputException exc)
        ////                            {
        ////                                Console.WriteLine(exc);
        ////                            }
        ////                            char Mychoice = updateChoice[0];
        ////                            switch (Mychoice) //chooses what to update
        ////                            {
        ////                                case 'A': //matchs a parcel to a drone
        ////                                    {
        ////                                        Console.WriteLine("Enter parcel id: ");
        ////                                        int parcelId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine(Data.matchUpParcel(Data.findParcel(parcelId)) + "\n"); //matches and prints if completed successfully
        ////                                        break;
        ////                                    }
        ////                                case 'B': //collects a parcel by drone
        ////                                    {
        ////                                        Console.WriteLine("Enter parcel id: ");
        ////                                        int parcelId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter sending customers id: ");
        ////                                        int customerId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine(Data.pickUpParcel(Data.findCustomer(customerId), Data.findParcel(parcelId)) + "\n"); //collects and prints if completed successfully
        ////                                        break;
        ////                                    }
        ////                                case 'C': //delivers a parcel to a customer
        ////                                    {
        ////                                        Console.WriteLine("Enter parcel id: ");
        ////                                        int parcelId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter receiving customers id: ");
        ////                                        int customerId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter level of priority: ");
        ////                                        int priorityLevel = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine(Data.deliverParcel(Data.findCustomer(customerId), Data.findParcel(parcelId), priorityLevel) + "\n"); //delivers and prints if completed successfully
        ////                                        break;
        ////                                    }
        ////                                case 'D': //charges a drone
        ////                                    {
        ////                                        Console.WriteLine("Enter drone id: ");
        ////                                        int droneId = int.Parse(Console.ReadLine());
        ////                                        string stationNum;
        ////                                        Console.WriteLine("List of available charging sttaions:");
        ////                                        foreach (Station item in Data.printStationsList()) { if (item.chargeSlots != 0) Console.WriteLine(item.ToString() + "\n"); }; //prints list of available charging stations
        ////                                        Console.WriteLine("Enter name of station you want to charge drone at:");
        ////                                        stationNum = Console.ReadLine();
        ////                                        Console.WriteLine(Data.chargeDrone(Data.findDrone(droneId), stationNum) + "\n"); //charges and prints if completed successfully
        ////                                        break;
        ////                                    }
        ////                                case 'E': //unplugs a charging drone
        ////                                    {
        ////                                        Console.WriteLine("Enter drone id: ");
        ////                                        int droneId = int.Parse(Console.ReadLine());
        ////                                        Console.WriteLine(Data.releaseDrone(Data.findDroneCharge(droneId)) + "\n"); //unplugs and prints if completed successfully
        ////                                        break;
        ////                                    }
        ////                                case 'F': //finds distance to charge station
        ////                                    {
        ////                                        Console.WriteLine("Enter langitude coordinates: ");
        ////                                        double longitutde = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter latitude coordinates: ");
        ////                                        double latitude = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("the distance is:");
        ////                                        foreach (Station item in Data.printStationsList()) { Console.WriteLine(item.name + ": " + Data.distance(item.lattitude, item.longitude, latitude, longitutde)); }; //prints the distance
        ////                                        break;
        ////                                    }
        ////                                case 'G': //finds distance from customer
        ////                                    {
        ////                                        Console.WriteLine("Enter langitude coordinates: ");
        ////                                        double longitutde = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("Enter latitude coordinates: ");
        ////                                        double latitude = double.Parse(Console.ReadLine());
        ////                                        Console.WriteLine("the distance is:");
        ////                                        foreach (Customer item in Data.printCustomersList()) { Console.WriteLine(item.name + ": " + Data.distance(item.lattitude, item.longitude, latitude, longitutde)); }; //prints the distances
        ////                                        break;
        ////                                    }
        ////                                default:
        ////                                    Console.WriteLine("ERROR CHOICE NOT VALID");
        ////                                    break;
        ////                            }
        ////                            break;
        ////                        }
        ////                    case 3: //prints item details
        ////                        {
        ////                            //char Choice_Print;
        ////                            int Id;
        ////                            Console.WriteLine("Enter choice you want to print:");
        ////                            Console.WriteLine("A- charge station");
        ////                            Console.WriteLine("B- drone");
        ////                            Console.WriteLine("C- customer");
        ////                            Console.WriteLine("D- parcel");
        ////                           // Choice_Print = char.Parse(Console.ReadLine());
        ////                            string Choice_Print = (Console.ReadLine());
        ////                            try
        ////                            {
        ////                                // string addingChoice = (Console.ReadLine()); 
        ////                                if (Choice_Print.Length != 1)
        ////                                    throw new InvalidInputException("Choice must be from following menu!\n");

        ////                            }
        ////                            catch (InvalidInputException exc)
        ////                            {
        ////                                Console.WriteLine(exc);
        ////                            }
        ////                            char Mychoice = Choice_Print[0];
        ////                            Console.Write("Enter ");
        ////                            if (Mychoice == 'A')
        ////                                Console.Write("charge Station ");
        ////                            if (Mychoice == 'B')
        ////                                Console.Write("drone ");
        ////                            if (Mychoice == 'C')
        ////                                Console.Write("customer ");
        ////                            if (Mychoice == 'D')
        ////                                Console.Write("pacel ");
        ////                            Console.WriteLine("id: ");
        ////                            Id = int.Parse(Console.ReadLine());
        ////                            switch (Mychoice) //chooses an item
        ////                            {
        ////                                case 'A': //prints a charge stations details
        ////                                    {
        ////                                        Console.WriteLine("\n" + Data.PrintStation(Id) + "\n");
        ////                                        break;
        ////                                    }
        ////                                case 'B': //prints a drones details
        ////                                    {
        ////                                        Console.WriteLine("\n" + Data.PrintDrone(Id) + "\n");
        ////                                        break;
        ////                                    }
        ////                                case 'C': //prints a customers details
        ////                                    {
        ////                                        Console.WriteLine("\n" + Data.PrintCustomer(Id) + "\n");
        ////                                        break;
        ////                                    }
        ////                                case 'D': //prints a parcels details
        ////                                    {
        ////                                        Console.WriteLine("\n" + Data.PrintParcel(Id) + "\n");
        ////                                        break;
        ////                                    }
        ////                                default:
        ////                                    Console.WriteLine("ERROR CHOICE NOT VALID");
        ////                                    break;
        ////                            }
        ////                            break;
        ////                        }
        ////                    case 4: //prints list of items
        ////                        {
        ////                           // char Choice_Print;
        ////                            Console.WriteLine("Enter choice you want to print:");
        ////                            Console.WriteLine("A- charge station list");
        ////                            Console.WriteLine("B- drone list");
        ////                            Console.WriteLine("C- customer list");
        ////                            Console.WriteLine("D- parcel list");
        ////                            Console.WriteLine("E- parcel that was not matched up to drone list");
        ////                            Console.WriteLine("F- charge station with available charge list");
        ////                            string Choice_Print = (Console.ReadLine());
        ////                            try
        ////                            {
        ////                                // string addingChoice = (Console.ReadLine()); 
        ////                                if (Choice_Print.Length != 1)
        ////                                    throw new InvalidInputException("Choice must be from following menu!\n");

        ////                            }
        ////                            catch (InvalidInputException exc)
        ////                            {
        ////                                Console.WriteLine(exc);
        ////                            }
        ////                            char Mychoice = Choice_Print[0];

        ////                            //Choice_Print = char.Parse(Console.ReadLine());
        ////                            switch (Mychoice) //chooses an item
        ////                            {
        ////                                case 'A': //prints charge station list
        ////                                    {
        ////                                        Console.WriteLine("List of stations:\n");
        ////                                        foreach (Station item in Data.printStationsList()) { Console.WriteLine(item.ToString() + "\n"); };
        ////                                        break;
        ////                                    }
        ////                                case 'B': //prints drone list
        ////                                    {
        ////                                        Console.WriteLine("List of drones:\n");
        ////                                        foreach (Drone item in Data.printDronesList()) { Console.WriteLine(item.ToString() + "\n"); };
        ////                                        break;
        ////                                    }
        ////                                case 'C': //prints customer list
        ////                                    {
        ////                                        Console.WriteLine("List of customers:\n");
        ////                                        foreach (Customer item in Data.printCustomersList()) { Console.WriteLine(item.ToString() + "\n"); };
        ////                                        break;
        ////                                    }
        ////                                case 'D': //prints parcel list
        ////                                    {
        ////                                        Console.WriteLine("List of parcel:\n");
        ////                                        foreach (Parcel item in Data.printParcelsList()) { Console.WriteLine(item.ToString() + "\n"); }
        ////                                        break;
        ////                                    }
        ////                                case 'E': //prints parcel that was not matched up to drone list 
        ////                                    {
        ////                                        bool check = true;
        ////                                        foreach (Parcel item in Data.printParcelsList())
        ////                                        {
        ////                                            if (item.droneId != 0)
        ////                                            {
        ////                                                check = false;
        ////                                                break;
        ////                                            }

        ////                                        }
        ////                                        if (check)
        ////                                        {
        ////                                            Console.WriteLine("All parcels matched up to drone");
        ////                                            break;
        ////                                        }
        ////                                        Console.WriteLine("List of parcel not matched up with drones:\n");
        ////                                        foreach (Parcel item in Data.printParcelsList()) { if (item.droneId == 0) Console.WriteLine(item.ToString() + "\n"); };
        ////                                        break;
        ////                                    }
        ////                                case 'F': //prints charge station with available charge list
        ////                                    {
        ////                                        bool check = true;
        ////                                        foreach (Station item in Data.printStationsList())
        ////                                        {
        ////                                            if (item.chargeSlots != 0)
        ////                                            {
        ////                                                check = false;
        ////                                                break;
        ////                                            }

        ////                                        }
        ////                                        if (check)
        ////                                        {
        ////                                            Console.WriteLine("No available charge slots at any station");
        ////                                            break;
        ////                                        }
        ////                                        Console.WriteLine("List of stations with available charging drone:\n");
        ////                                        foreach (Station item in Data.printStationsList()) { if (item.chargeSlots != 0) Console.WriteLine(item.ToString() + "\n"); };
        ////                                        break;
        ////                                    }
        ////                                default:
        ////                                    Console.WriteLine("ERROR CHOICE NOT VALID");
        ////                                    break;
        ////                            }
        ////                            break;
        ////                        }
        ////                    default:
        ////                        Console.WriteLine("ERROR INVALID CHOICE");
        ////                        break;
        ////                }
        ////                Console.WriteLine("Choose from the following options:");
        ////                Console.WriteLine("1- To add new item");
        ////                Console.WriteLine("2- To update item");
        ////                Console.WriteLine("3- To print item details");
        ////                Console.WriteLine("4- To print list of items");
        ////                Console.WriteLine("5- To exit");
        ////                //choice = int.Parse(Console.ReadLine());
        ////                string choice = (Console.ReadLine());
        ////                try
        ////                {
        ////                    // string addingChoice = (Console.ReadLine()); 
        ////                    if (choice.Length != 1)
        ////                        throw new InvalidInputException("Choice must be from following menu!\n");

        ////                }
        ////                catch (InvalidInputException exc)
        ////                {
        ////                    Console.WriteLine(exc);
        ////                }
        ////                chosen = choice[0];
        ////            }
  
