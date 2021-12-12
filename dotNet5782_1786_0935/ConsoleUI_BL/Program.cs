using System;
using IBL.BO;
using BL;
//handel camelcase
//get rid of all the fake case  we used to improve
//case 1- exceptions all handled
//case 2 all exceptions handled


namespace ConsoleUI_BL
{
    public class Program
    {
        static BLImp Data;
        static void Main(string[] args)
        {
            Data = new BLImp();
            int choice;
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1- To add new item");
            Console.WriteLine("2- To update item");
            Console.WriteLine("3- To print item details");
            Console.WriteLine("4- To print list of items");
            string check = (Console.ReadLine());
            try
            {
                // string addingChoice = (Console.ReadLine()); 
                if (check.Length != 1)
                    throw new InvalidInputException("Choice must be from following menu!\n");

            }
            catch (InvalidInputException exc)
            {
                Console.WriteLine(exc);
            }
            choice = (int)check[0];
            while (choice!=5)
            {
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter your choice:");
                            Console.WriteLine("A- add a statsion\n " +
                                "B- add a drone\n" +
                                "C- add a new customer\n" +
                                "D- add a parcel for delivery");
                            
                            string input = (Console.ReadLine());
                            try
                            {
                                // string addingChoice = (Console.ReadLine()); 
                                if (input.Length != 1)
                                    throw new InvalidInputException("Choice must be from following menu!\n");

                            }
                            catch (InvalidInputException exc)
                            {
                                Console.WriteLine(exc);
                            }
                            char addingChoice = input[0];
                            switch (addingChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter ID of station: ");
                                        Station s = new Station() { StationId = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter name of station: ");
                                        s.name = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter lattitude and longitude of station");
                                        s.location = new Location(0, 0);
                                        s.location.Lattitude = double.Parse(Console.ReadLine());
                                        s.location.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter amount of charge slots that station has: ");
                                        s.chargeSlots = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.AddStation(s);
                                        }
                                        catch(InvalidInputException exc)
                                        {
                                            Console.WriteLine( exc.Message);
                                        }
                                        catch (AlreadyExistsException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }

                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter drone number");
                                        Drone d = new Drone() { DroneId = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter name of model: ");
                                        d.Model = Console.ReadLine();
                                        Console.WriteLine("Choose maximum weight drone can hold:\n" +
                                            "a: light\n" +
                                            "b: average\n" +
                                            "c: heavy ");
                                        char weightChoice = char.Parse(Console.ReadLine());
                                        switch (weightChoice)
                                        {
                                            case 'a':
                                                d.MaxWeight = WeightCategories.light;
                                                break;
                                            case 'b':
                                                d.MaxWeight = WeightCategories.average;
                                                break;
                                            case 'c':
                                                d.MaxWeight = WeightCategories.heavy;
                                                break;
                                        }
                                        Console.WriteLine("Enter station number");   
                                        int stationId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.AddDrone(d, stationId); //builds and adds a drone using the information the user provided
                                        }
                                        catch (InvalidInputException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        catch (AlreadyExistsException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        Console.WriteLine("drone id: " + d.DroneId + "\n");
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter Id of customer: ");
                                        Customer c = new Customer() { CustomerId = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter name of customer: ");
                                        c.Name = "Customer-" + Console.ReadLine();
                                        Console.WriteLine("Enter phone number of customer: ");
                                        c.Phone = Console.ReadLine();
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        c.Location.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        c.Location.Lattitude = double.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.AddCustomer(c); //builds and adds a customer using the information the user provided
                                        }
                                        catch (InvalidInputException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        catch (AlreadyExistsException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter Id of sender: ");
                                        Parcel p = new Parcel()
                                        {
                                            Sender = new CustomerInParcel()
                                            { 
                                                CustomerId = int.Parse(Console.ReadLine()) 
                                            }
                                        };
                                        Console.WriteLine("Enter Id of target: ");
                                        p.Target = new CustomerInParcel()
                                        {
                                            CustomerId = int.Parse(Console.ReadLine())
                                        };
                                        Console.WriteLine("Choose weight of parcel:\n" +
                                            "a: light\n" +
                                            "b: average\n" +
                                            "c: heavy ");
                                        char weightChoice = char.Parse(Console.ReadLine());
                                        switch (weightChoice)
                                        {
                                            case 'a':
                                                p.Weight = WeightCategories.light;
                                                break;
                                            case 'b':
                                                p.Weight = WeightCategories.average;
                                                break;
                                            case 'c':
                                                p.Weight = WeightCategories.heavy;
                                                break;
                                        }
                                        Console.WriteLine("Choose priority of parcel:\n" +
                                            "a: regular\n" +
                                            "b: fast\n" +
                                            "c: emergency ");
                                        char priorityChoice = char.Parse(Console.ReadLine());
                                        switch (priorityChoice)
                                        {
                                            case 'a':
                                                p.Priority = Priorities.regular;
                                                break;
                                            case 'b':
                                                p.Priority = Priorities.fast;
                                                break;
                                            case 'c':
                                                p.Priority = Priorities.emergency;
                                                break;
                                        }
                                        try
                                        {
                                            Data.AddParcel(p); //builds and adds a parcel using the information the user provided
                                        }
                                        catch (InvalidInputException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        catch (AlreadyExistsException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR INVALID CHOICE");
                                    break;
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Enter your choice:");
                            Console.WriteLine("A- update the name of a drone\n " +
                                "B- update stations details\n" +
                                "C- update customers information\n" +
                                "D- charge drone\n" +
                                "E- release drone from charging\n" +
                                "F- match up parcel to drone\n" +
                                "G- pickup parcel by drone\n" +
                                "H- deliver parcel by drone\n");
        
                            string input = (Console.ReadLine());
                            try
                            {
                                // string addingChoice = (Console.ReadLine()); 
                                if (input.Length != 1)
                                    throw new InvalidInputException("Choice must be from following menu!\n");

                            }
                            catch (InvalidInputException exc)
                            {
                                Console.WriteLine(exc);
                            }
                            char updateChoice = input[0];
                            switch (updateChoice) //chooses what to update
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter Id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter model of drone");
                                        int model = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.UpdateStationName(droneId, model);
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter id of station");
                                        int stationId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter one or more of the following, to skip press the enter key:");
                                        Console.WriteLine("Enter new name of station:");
                                        int stationName = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter new amount of charges at station:");
                                        int numOfCharges = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.updateStation(stationId, stationName, numOfCharges);
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }

                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter id of customer");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter one or more of the following, to skip press the enter key:");
                                        Console.WriteLine("Enter a new customer name:");
                                        string customerName = Console.ReadLine();
                                        Console.WriteLine("Enter a new phone number:");
                                        string phone = Console.ReadLine();
                                        try
                                        { 
                                            Data.UpdateCustomerName(customerId, customerName, phone); 
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.SendDroneToCharge(droneId);
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.UnableToCompleteRequest exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.unavailableException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (Exception exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                       
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter the amount of time that the drone has been charging:");
                                        int chargeTime = int.Parse(Console.ReadLine());
                                        try
                                        { 
                                            Data.ReleaseDroneFromCharge(droneId, chargeTime);
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.UnableToCompleteRequest exc)
                                        {
                                            Console.WriteLine(exc);
                                        }


                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.MatchDroneWithPacrel(droneId);
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.unavailableException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'G':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        try
                                        { 
                                            Data.PickUpParcel(droneId); 
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.UnableToCompleteRequest exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'H':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Data.DeliveredParcel(droneId); 
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IBL.BO.UnableToCompleteRequest exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR INVALID CHOICE");
                                    break;
                            }
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Enter your choice:");
                            Console.WriteLine("A- print station\n " +
                                "B- print drone\n" +
                                "C- print customer\n" +
                                "D- print parcel\n");
                            string input = (Console.ReadLine());
                            try
                            {
                                // string addingChoice = (Console.ReadLine()); 
                                if (input.Length != 1)
                                    throw new InvalidInputException("Choice must be from following menu!\n");

                            }
                            catch (InvalidInputException exc)
                            {
                                Console.WriteLine(exc);
                            }
                            char printChoice = input[0];
                            switch (printChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter id of station");
                                        int stationId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetStation(stationId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetDrone(droneId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter id of customer");
                                        int customerId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetCustomer(customerId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter id of parcel");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetParcel(parcelId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("ERROR INVALID CHOICE");
                                    break;
                            }
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Enter choice you want to print:");
                            Console.WriteLine("A- station list");
                            Console.WriteLine("B- drone list");
                            Console.WriteLine("C- customer list");
                            Console.WriteLine("D- parcel list");
                            Console.WriteLine("E- parcel that was not matched up to drone list");
                            Console.WriteLine("F- charge station with available charge list");
                            string input = (Console.ReadLine());
                            try
                            {
                                // string addingChoice = (Console.ReadLine()); 
                                if (input.Length != 1)
                                    throw new InvalidInputException("Choice must be from following menu!\n");

                            }
                            catch (InvalidInputException exc)
                            {
                                Console.WriteLine(exc);
                            }
                            char printListChoice = input[0];
                            switch (printListChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("List of stations:\n");
                                        try
                                        { 
                                            foreach (Station item in Data.GetStationsList()) { Console.WriteLine(item.ToString() + "\n"); }; 
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("List of drones:\n");
                                        try
                                        {
                                            foreach (Drone item in Data.GetDronesList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("List of customers:\n");
                                        try
                                        {
                                            foreach (Customer item in Data.GetCustomersList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("List of parcels:\n");
                                        try
                                        {
                                            foreach (Parcel item in Data.GetParcelsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("List of parcels that are not yet matched up to drone:\n");
                                        try
                                        {
                                            foreach (Parcel item in Data.GetUnmatchedParcelsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("List of stations with availablechargeslots:\n");
                                        try
                                        {
                                            foreach (Station item in Data.GetAvailableStationsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }

                                        break;
                                    }

                                default:
                                    Console.WriteLine("ERROR INVALID CHOICE");
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
                string help = (Console.ReadLine());
                try
                {
                    // string addingChoice = (Console.ReadLine()); 
                    if (help.Length != 1)
                        throw new InvalidInputException("Choice must be from following menu!\n");

                }
                catch (InvalidInputException exc)
                {
                    Console.WriteLine(exc);
                }
                choice = (int)help[0];


            }
        }
    }
}
