﻿//using System;
//using BO;
//using BlApi;
//using DalApi;
//using BL;
////Adina Schulman 328620935
////Eliana Grajower 328781786



//namespace ConsoleUI_BL
//{
//    public class Program
//    {
//        static IBL bl = BlFactory.GetBl();
//        static void Main(string[] args)
//        {





//            // bl = new BL();
//            Console.WriteLine("Choose from the following options:");
//            Console.WriteLine("1- To add new item");
//            Console.WriteLine("2- To update item");
//            Console.WriteLine("3- To print item details");
//            Console.WriteLine("4- To print list of items");
//            Console.WriteLine("5- To exit");
//            string input;
//            bool check;
//            int choice;
//            input = Console.ReadLine();
//            check = int.TryParse(input, out int error);
//            if (check)
//                choice = int.Parse(input);
//            else
//                choice = -1;


//            while (choice != 5)
//            {
//                switch (choice)
//                {
//                    case 1:
//                        {
//                            Console.WriteLine("Enter your choice:");
//                            Console.WriteLine("A- add a statsion\n" +
//                                "B- add a drone\n" +
//                                "C- add a new customer\n" +
//                                "D- add a parcel for delivery");
//                            string bl;
//                            bool flag;
//                            char addingChoice;
//                            bl = Console.ReadLine();
//                            flag = char.TryParse(bl, out char wrong);
//                            if (flag)
//                                addingChoice = char.Parse(bl);
//                            else
//                                addingChoice = '!';


//                            switch (addingChoice)
//                            {
//                                case 'A':
//                                    {
//                                        int id, chargeSlots, name;
//                                        double longitude, latitude;
//                                        Console.WriteLine("Enter ID of station: ");
//                                        Station s = new Station();
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        s.stationId = id;
//                                        Console.WriteLine("Enter name of station: ");
//                                        s.name = Console.ReadLine();
//                                        Console.WriteLine("Enter lattitude and longitude of station:");
//                                        s.location = new Location(0, 0);
//                                        double.TryParse(Console.ReadLine(), out latitude);
//                                        s.location.lattitude = latitude;
//                                        double.TryParse(Console.ReadLine(), out longitude);
//                                        s.location.longitude = longitude;
//                                        Console.WriteLine("Enter amount of charge slots that station has: ");
//                                        int.TryParse(Console.ReadLine(), out chargeSlots);
//                                        s.chargeSlots = chargeSlots;

//                                        try
//                                        {
//                                            bl.addStation(s);
//                                        }
//                                        catch (InvalidInputException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);

//                                        }
//                                        catch (AlreadyExistsException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }

//                                        break;
//                                    }
//                                case 'B':
//                                    {
//                                        int droneId, stationId;

//                                        Console.WriteLine("Enter drone number");
//                                        Drone d = new Drone();
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        d.droneId = droneId;
//                                        Console.WriteLine("Enter name of model: ");

//                                        d.model = Console.ReadLine();
//                                        Console.WriteLine("Choose maximum weight drone can hold:\n" +
//                                            "1- light, 2-average, 3-heavy");
//                                        BO.weightCategories weightChoice;

//                                        input = Console.ReadLine();
//                                        check = int.TryParse(input, out error);
//                                        if (check)
//                                            choice = int.Parse(input);
//                                        else
//                                            choice = -1;
//                                        weightChoice = (BO.weightCategories)choice;
//                                        switch (weightChoice)
//                                        {
//                                            case weightCategories.light:
//                                                d.maxWeight = weightCategories.light;
//                                                break;
//                                            case weightCategories.average:
//                                                d.maxWeight = weightCategories.average;
//                                                break;
//                                            case weightCategories.heavy:
//                                                d.maxWeight = weightCategories.heavy;
//                                                break;

//                                        }
//                                        Console.WriteLine("Enter station number:");
//                                        int.TryParse(Console.ReadLine(), out stationId);

//                                        try
//                                        {
//                                            bl.addDrone(d, stationId); //builds and adds a drone using the information the user provided
//                                        }
//                                        catch (InvalidInputException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                            Console.WriteLine("unable to add ");
//                                        }
//                                        catch (AlreadyExistsException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                            Console.WriteLine("unable to add ");
//                                        }
//                                        catch (DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        Console.WriteLine("drone id: " + d.droneId + "\n");
//                                        break;
//                                    }
//                                case 'C':
//                                    {
//                                        int id;
//                                        double latitude, longitude;
//                                        Console.WriteLine("Enter Id of customer: ");
//                                        Customer c = new Customer();
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        c.customerId = id;
//                                        Console.WriteLine("Enter name of customer: ");
//                                        c.name = Console.ReadLine();
//                                        Console.WriteLine("Enter phone number of customer: ");

//                                        c.phone = Console.ReadLine();
//                                        c.location = new Location(0, 0);
//                                        Console.WriteLine("Enter Your lattitude coordinates: ");
//                                        double.TryParse(Console.ReadLine(), out latitude);

//                                        Console.WriteLine("Enter Your longitude coordinates: ");
//                                        double.TryParse(Console.ReadLine(), out longitude);
//                                        c.location = new Location(latitude, longitude);

//                                        try
//                                        {
//                                            bl.addCustomer(c); //builds and adds a customer using the information the user provided
//                                        }
//                                        catch (InvalidInputException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (AlreadyExistsException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (InvalidCastException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'D':
//                                    {
//                                        int id;
//                                        Console.WriteLine("Enter Id of sender: ");
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        Parcel p = new Parcel()
//                                        {

//                                            sender = new CustomerInParcel()
//                                            {

//                                                customerId = id
//                                            }


//                                        };

//                                        Console.WriteLine("Enter Id of target: ");
//                                        int.TryParse(Console.ReadLine(), out id);
//                                        p.target = new CustomerInParcel()
//                                        {
//                                            customerId = id
//                                        };
//                                        Console.WriteLine("Choose  weight of parcel:\n" +
//                                            "1-light,2- average, 3-heavy");
//                                        BO.weightCategories weightChoice;

//                                        input = Console.ReadLine();
//                                        check = int.TryParse(input, out error);
//                                        if (check)
//                                            choice = int.Parse(input);
//                                        else
//                                            choice = -1;
//                                        weightChoice = (BO.weightCategories)choice;
//                                        switch (weightChoice)
//                                        {
//                                            case weightCategories.light:
//                                                p.weight = weightCategories.light;
//                                                break;
//                                            case weightCategories.average:
//                                                p.weight = weightCategories.average;
//                                                break;
//                                            case weightCategories.heavy:
//                                                p.weight = weightCategories.heavy;
//                                                break;

//                                        }
//                                        Console.WriteLine("Choose priority of parcel:\n" +
//                                          "1-regular,2- fast,3- emergency");
//                                        BO.Priorities priorities;

//                                        input = Console.ReadLine();
//                                        check = int.TryParse(input, out error);
//                                        if (check)
//                                            choice = int.Parse(input);
//                                        else
//                                            choice = -1;
//                                        priorities = (BO.Priorities)choice;
//                                        switch (priorities)
//                                        {
//                                            case Priorities.regular:
//                                                p.priority = Priorities.regular;
//                                                break;
//                                            case Priorities.fast:
//                                                p.priority = Priorities.fast;
//                                                break;
//                                            case Priorities.emergency:
//                                                p.priority = Priorities.emergency;
//                                                break;

//                                        }
//                                        try
//                                        {
//                                            int t = bl.addParcel(p); //builds and adds a parcel using the information the user provided
//                                            Console.WriteLine("Parcel ID: " + t);
//                                        }
//                                        catch (InvalidInputException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (AlreadyExistsException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (InvalidCastException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                default:
//                                    Console.WriteLine("ERROR INVALID CHOICE");
//                                    break;
//                            }
//                            break;
//                        }
//                    case 2:
//                        {
//                            Console.WriteLine("Enter your choice:");
//                            Console.WriteLine("A- update the name of a drone\n" +
//                                "B- update stations details\n" +
//                                "C- update customers information\n" +
//                                "D- charge drone\n" +
//                                "E- release drone from charging\n" +
//                                "F- match up parcel to drone\n" +
//                                "G- pickup parcel by drone\n" +
//                                "H- deliver parcel by drone\n");
//                            string bl;
//                            bool flag;
//                            char updateChoice;
//                            bl = Console.ReadLine();
//                            flag = char.TryParse(bl, out char bad);
//                            if (flag)
//                                updateChoice = char.Parse(bl);
//                            else
//                                updateChoice = '!';
//                            switch (updateChoice) //chooses what to update
//                            {
//                                case 'A':
//                                    {
//                                        int droneId;
//                                        Console.WriteLine("Enter Id of drone:");
//                                        int.TryParse(Console.ReadLine(), out droneId);

//                                        Console.WriteLine("Enter model of drone:");
//                                        string model = Console.ReadLine();
//                                        try
//                                        {
//                                            bl.UpdateDronename(droneId, model);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'B':
//                                    {

//                                        Console.WriteLine("Enter id of station:");
//                                        int stationId;
//                                        int.TryParse(Console.ReadLine(), out stationId);
//                                        Console.WriteLine("Enter one or more of the following, to skip press the enter key:");
//                                        Console.WriteLine("Enter new name of station:");
//                                        string stationname = Console.ReadLine();
//                                        Console.WriteLine("Enter new amount of charges at station:");
//                                        int numOfCharges;
//                                        int.TryParse(Console.ReadLine(), out numOfCharges);
//                                        try
//                                        {
//                                            bl.updateStation(stationId, numOfCharges, stationname);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.UnableToCompleteRequest exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }


//                                        break;
//                                    }
//                                case 'C':
//                                    {

//                                        Console.WriteLine("Enter id of customer:");
//                                        int customerId;
//                                        int.TryParse(Console.ReadLine(), out customerId);
//                                        Console.WriteLine("Enter one or more of the following, to skip press the enter key:");
//                                        Console.WriteLine("Enter a new customer name:");
//                                        string customername = Console.ReadLine();
//                                        Console.WriteLine("Enter a new phone number:");
//                                        string phone = Console.ReadLine();
//                                        try
//                                        {
//                                            bl.UpdateCustomer(customerId, customername, phone);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'D':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        try
//                                        {
//                                            bl.SendDroneToCharge(droneId);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.UnableToCompleteRequest exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.unavailableException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (Exception exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }

//                                        break;
//                                    }
//                                case 'E':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        Console.WriteLine("Enter the amount of time that the drone has been charging:");
//                                        int chargeTime;
//                                        int.TryParse(Console.ReadLine(), out chargeTime);
//                                        try
//                                        {
//                                            bl.ReleaseDroneFromCharge(droneId, chargeTime);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.UnableToCompleteRequest exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.InvalidInputException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        //catch (BO.AlreadyExistsException exc)
//                                        //{
//                                        //    Console.WriteLine(exc.Message);
//                                        //}


//                                        break;
//                                    }
//                                case 'F':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);

//                                        try
//                                        {
//                                            bl.MatchDroneWithPacrel(droneId);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.unavailableException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.AlreadyExistsException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'G':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        try
//                                        {
//                                            bl.pickUpParcel(droneId);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.UnableToCompleteRequest exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'H':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        try
//                                        {
//                                            bl.deliveredParcel(droneId);
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (BO.UnableToCompleteRequest exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                default:
//                                    Console.WriteLine("ERROR INVALID CHOICE");
//                                    break;
//                            }
//                            break;
//                        }
//                    case 3:
//                        {
//                            Console.WriteLine("Enter your choice:");
//                            Console.WriteLine("A- print station\n" +
//                                "B- print drone\n" +
//                                "C- print customer\n" +
//                                "D- print parcel\n");
//                            string bl;
//                            bool flag;
//                            char printChoice;
//                            bl = Console.ReadLine();
//                            flag = char.TryParse(input, out char bad);
//                            if (flag)
//                                printChoice = char.Parse(bl);
//                            else
//                                printChoice = '!';
//                            switch (printChoice)
//                            {
//                                case 'A':
//                                    {
//                                        Console.WriteLine("Enter id of station:");
//                                        int stationId;
//                                        int.TryParse(Console.ReadLine(), out stationId);
//                                        try
//                                        {
//                                            Console.WriteLine("\n" + bl.getStation(stationId).ToString() + "\n");
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (DO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'B':
//                                    {
//                                        Console.WriteLine("Enter id of drone:");
//                                        int droneId;
//                                        int.TryParse(Console.ReadLine(), out droneId);
//                                        try
//                                        {
//                                            Console.WriteLine("\n" + bl.getDrone(droneId).ToString() + "\n");
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (DO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'C':
//                                    {
//                                        Console.WriteLine("Enter id of customer:");
//                                        int customerId;
//                                        int.TryParse(Console.ReadLine(), out customerId);
//                                        try
//                                        {
//                                            Console.WriteLine("\n" + bl.getCustomer(customerId).ToString() + "\n");
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (DO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                case 'D':
//                                    {
//                                        Console.WriteLine("Enter id of parcel:");
//                                        int parcelId;
//                                        int.TryParse(Console.ReadLine(), out parcelId);
//                                        try
//                                        {
//                                            Console.WriteLine("\n" + bl.getParcel(parcelId).ToString() + "\n");
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        catch (DO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine(exc.Message);
//                                        }
//                                        break;
//                                    }
//                                default:
//                                    Console.WriteLine("ERROR INVALID CHOICE");
//                                    break;
//                            }
//                            break;
//                        }
//                    case 4:
//                        {
//                            Console.WriteLine("Enter choice you want to print:");
//                            Console.WriteLine("A- station list");
//                            Console.WriteLine("B- drone list");
//                            Console.WriteLine("C- customer list");
//                            Console.WriteLine("D- parcel list");
//                            Console.WriteLine("E- parcel that was not matched up to drone list");
//                            Console.WriteLine("F- charge station with available charge list");
//                            string bl;
//                            bool flag;
//                            char printListChoice;
//                            bl = Console.ReadLine();
//                            flag = char.TryParse(bl, out char bad);
//                            if (flag)
//                                printListChoice = char.Parse(bl);
//                            else
//                                printListChoice = '!';
//                            switch (printListChoice)
//                            {
//                                case 'A':
//                                    {
//                                        Console.WriteLine("List of stations:\n");
//                                        try
//                                        {
//                                            foreach (StationToList item in bl.getStationsList()) { Console.WriteLine(item.ToString() + "\n"); };
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("no stations exist\n");
//                                        }
//                                        break;
//                                    }
//                                case 'B':
//                                    {
//                                        Console.WriteLine("List of drones:\n");
//                                        try
//                                        {
//                                            bl.getDronesList().ForEach(item => { Console.WriteLine(item.ToString() + "\n"); });
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("no drones exist\n");
//                                        }
//                                        break;
//                                    }
//                                case 'C':
//                                    {
//                                        Console.WriteLine("List of customers:\n");
//                                        try
//                                        {
//                                            foreach (CustomerToList item in bl.getCustomersList()) { Console.WriteLine(item.ToString() + "\n"); };
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("no customer exists\n");
//                                        }
//                                        break;
//                                    }
//                                case 'D':
//                                    {
//                                        Console.WriteLine("List of parcels:\n");
//                                        try
//                                        {
//                                            foreach (ParcelToList item in bl.getParcelsList()) { Console.WriteLine(item.ToString() + "\n"); };
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("No parcels exist\n");
//                                        }
//                                        break;
//                                    }
//                                case 'E':
//                                    {
//                                        Console.WriteLine("List of parcels that are not yet matched up to drone:\n");
//                                        try
//                                        {
//                                            foreach (ParcelToList item in bl.getParcelsList()) { if (item.parcelStatus == ParcelStatus.created) Console.WriteLine(item.ToString() + "\n"); };
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("no such parcels exist\n");
//                                        }
//                                        break;
//                                    }
//                                case 'F':
//                                    {
//                                        Console.WriteLine("List of stations with availablechargeSlots:\n");
//                                        try
//                                        {
//                                            foreach (StationToList item in bl.getStationsList()) { if (item.numberOfAvailableSlots > 0) Console.WriteLine(item.ToString() + "\n"); };
//                                        }
//                                        catch (BO.DoesntExistException exc)
//                                        {
//                                            Console.WriteLine("no such stations exist\n");
//                                        }

//                                        break;
//                                    }

//                                default:
//                                    Console.WriteLine("ERROR INVALID CHOICE");
//                                    break;
//                            }
//                            break;
//                        }
//                    default:
//                        Console.WriteLine("ERROR INVALID CHOICE");
//                        break;
//                }
//                Console.WriteLine("Choose from the following options:");
//                Console.WriteLine("1- To add new item");
//                Console.WriteLine("2- To update item");
//                Console.WriteLine("3- To print item details");
//                Console.WriteLine("4- To print list of items");
//                Console.WriteLine("5- To exit");
//                input = Console.ReadLine();
//                check = int.TryParse(input, out error);
//                if (check)
//                    choice = int.Parse(input);
//                else
//                    choice = -1;
//            }
//        }
//    }
//}






   
