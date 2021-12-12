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
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1- To add new item");
            Console.WriteLine("2- To update item");
            Console.WriteLine("3- To print item details");
            Console.WriteLine("4- To print list of items");
            string input;
            bool check;
            int choice;
            input = Console.ReadLine();
            check = int.TryParse(input, out int error);
            if (check)
                choice = int.Parse(input);
            else
                choice = -1;


            while (choice!=5)
            {
                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter your choice:");
                            Console.WriteLine("A- add a statsion\n" +
                                "B- add a drone\n" +
                                "C- add a new customer\n" +
                                "D- add a parcel for delivery");
                            string data;
                            bool flag;   
                            char addingChoice;     
                            data = Console.ReadLine();
                            flag = char.TryParse(data, out char wrong);
                            if (flag)
                                addingChoice = char.Parse(data);
                            else
                                addingChoice = '!';


                            switch (addingChoice)
                            {
                                case 'A':
                                    {
                                        int id, chargeSlots,name;
                                        double longitude, latitude;
                                        Console.WriteLine("Enter ID of station: ");
                                        Station s = new Station(); 
                                        int.TryParse(Console.ReadLine(), out id);
                                        s.StationId = id;
                                        Console.WriteLine("Enter name of station: ");
                                        s.name = Console.ReadLine();
                                        Console.WriteLine("Enter lattitude and longitude of station");
                                        s.location = new Location(0, 0);
                                        double.TryParse(Console.ReadLine(), out latitude);
                                        s.location.Lattitude = latitude;
                                        double.TryParse(Console.ReadLine(), out longitude);
                                        s.location.Longitude = longitude;
                                        Console.WriteLine("Enter amount of charge slots that station has: ");
                                        int.TryParse(Console.ReadLine(), out chargeSlots);
                                        s.chargeSlots = chargeSlots;
                                        
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
                                        int droneId, stationId;
                                        
                                        Console.WriteLine("Enter drone number");
                                        Drone d = new Drone();
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        d.DroneId = droneId;
                                        Console.WriteLine("Enter name of model: ");

                                        d.Model = Console.ReadLine();
                                        Console.WriteLine("Choose maximum weight drone can hold:\n" +
                                            "1- light, 2-average, 3-heavy\n");
                                        IBL.BO.WeightCategories weightChoice;

                                        input = Console.ReadLine();
                                        check = int.TryParse(input, out error);
                                        if (check)
                                            choice = int.Parse(input);
                                        else
                                            choice = -1;
                                        weightChoice = (IBL.BO.WeightCategories)choice;
                                        switch (weightChoice)
                                        {
                                            case WeightCategories.light:
                                                d.MaxWeight = WeightCategories.light;
                                                break;
                                            case WeightCategories.average:
                                                d.MaxWeight = WeightCategories.average;
                                                break;
                                            case WeightCategories.heavy:
                                                d.MaxWeight = WeightCategories.heavy;
                                                break;
                                            
                                        }
                                                Console.WriteLine("Enter station number");
                                        int.TryParse(Console.ReadLine(), out stationId);

                                        try
                                        {
                                            Data.AddDrone(d,stationId); //builds and adds a drone using the information the user provided
                                        }
                                        catch (InvalidInputException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                            Console.WriteLine("unable to add ");
                                        }
                                        catch (AlreadyExistsException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                            Console.WriteLine("unable to add ");
                                        }
                                        catch (DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        Console.WriteLine("drone id: " + d.DroneId + "\n");
                                        break;
                                    }
                                case 'C':
                                    {
                                        int id;
                                        double latitude, longitude;
                                        Console.WriteLine("Enter Id of customer: ");
                                        Customer c = new Customer();
                                        int.TryParse(Console.ReadLine(), out id);
                                        c.CustomerId = id;
                                        Console.WriteLine("Enter name of customer: ");
                                        c.Name =  Console.ReadLine();
                                        Console.WriteLine("Enter phone number of customer: ");
                                        
                                        c.Phone = Console.ReadLine();
                                        c.Location = new Location(0, 0);
                                        Console.WriteLine("Enter Your lattitude coordinates: ");
                                        double.TryParse(Console.ReadLine(), out latitude);
                                        c.Location.Lattitude = latitude;
                                        Console.WriteLine("Enter Your longitude coordinates: ");
                                        double.TryParse(Console.ReadLine(), out longitude);
                                        c.Location.Longitude = longitude;

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
                                        catch(InvalidCastException exc)
                                        {
                                            Console.WriteLine(exc.Message);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        int id;
                                        Console.WriteLine("Enter Id of sender: ");
                                        int.TryParse(Console.ReadLine(), out id);
                                        Parcel p = new Parcel()
                                        {
                                            Sender = new CustomerInParcel()
                                            {
                                                
                                                  CustomerId = id
                                            }
                                        };
                                        Console.WriteLine("Enter Id of target: ");
                                        int.TryParse(Console.ReadLine(), out id);
                                        p.Target = new CustomerInParcel()
                                        {
                                            CustomerId = id
                                        };
                                        Console.WriteLine("Choose  weight of parcel:\n" +
                                            " 1-light,2- average, 3-heavy\n");
                                        IBL.BO.WeightCategories weightChoice;

                                        input = Console.ReadLine();
                                        check = int.TryParse(input, out error);
                                        if (check)
                                            choice = int.Parse(input);
                                        else
                                            choice = -1;
                                        weightChoice = (IBL.BO.WeightCategories)choice;
                                        switch (weightChoice)
                                        {
                                            case WeightCategories.light:
                                                p.Weight = WeightCategories.light;
                                                break;
                                            case WeightCategories.average:
                                                p.Weight = WeightCategories.average;
                                                break;
                                            case WeightCategories.heavy:
                                                p.Weight = WeightCategories.heavy;
                                                break;

                                        }
                                        Console.WriteLine("Choose priority of parcel:\n" +
                                          " 1-regular,2- fast,3- emergency \n");
                                        IBL.BO.Priorities priorities;

                                        input = Console.ReadLine();
                                        check = int.TryParse(input, out error);
                                        if (check)
                                            choice = int.Parse(input);
                                        else
                                            choice = -1;
                                        priorities = (IBL.BO.Priorities)choice;
                                        switch (priorities)
                                        {
                                            case Priorities.regular:
                                                p.Priority= Priorities.regular;
                                                break;
                                            case Priorities.fast:
                                                p.Priority = Priorities.fast;
                                                break;
                                            case Priorities.emergency:
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
                            Console.WriteLine("A- update the name of a drone\n" +
                                "B- update stations details\n" +
                                "C- update customers information\n" +
                                "D- charge drone\n" +
                                "E- release drone from charging\n" +
                                "F- match up parcel to drone\n" +
                                "G- pickup parcel by drone\n" +
                                "H- deliver parcel by drone\n");
                            string data;
                            bool flag;
                            char updateChoice;
                            data = Console.ReadLine();
                            flag = char.TryParse(data, out char bad);
                            if (flag)
                                updateChoice = char.Parse(data);
                            else
                                updateChoice = '!';
                            switch (updateChoice) //chooses what to update
                            {
                                case 'A':
                                    {
                                        int droneId;
                                        Console.WriteLine("Enter Id of drone");
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        
                                        Console.WriteLine("Enter model of drone");
                                        string model = Console.ReadLine();
                                        try
                                        {
                                            Data.UpdateDroneName(droneId, model);
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
                                        int stationId;
                                         int.TryParse(Console.ReadLine(),out stationId);
                                        Console.WriteLine("Enter one or more of the following, to skip press the enter key:");
                                        Console.WriteLine("Enter new name of station:");
                                        string stationName = Console.ReadLine();
                                        Console.WriteLine("Enter new amount of charges at station:");
                                        int numOfCharges;
                                        int.TryParse(Console.ReadLine(), out numOfCharges);
                                        try
                                        {
                                            Data.updateStation(stationId, numOfCharges,stationName);
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
                                case 'C':
                                    {

                                        Console.WriteLine("Enter id of customer");
                                        int customerId ;
                                        int.TryParse(Console.ReadLine(), out customerId);
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
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);
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
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        Console.WriteLine("Enter the amount of time that the drone has been charging:");
                                        int chargeTime;
                                        int.TryParse(Console.ReadLine(), out chargeTime);
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
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);

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
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);
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
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);
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
                            Console.WriteLine("A- print station\n" +
                                "B- print drone\n" +
                                "C- print customer\n" +
                                "D- print parcel\n");
                            string data;
                            bool flag;
                            char printChoice;
                            data = Console.ReadLine();
                            flag = char.TryParse(input, out char bad);
                            if (flag)
                                printChoice = char.Parse(data);
                            else
                                printChoice = '!';
                            switch (printChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter id of station");
                                        int stationId;
                                        int.TryParse(Console.ReadLine(), out stationId);
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetStation(stationId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IDAL.DO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId;
                                        int.TryParse(Console.ReadLine(), out droneId);
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetDrone(droneId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IDAL.DO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter id of customer");
                                        int customerId;
                                        int.TryParse(Console.ReadLine(), out customerId);
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetCustomer(customerId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IDAL.DO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter id of parcel");
                                        int parcelId;
                                        int.TryParse(Console.ReadLine(), out parcelId);
                                        try
                                        {
                                            Console.WriteLine("\n" + Data.GetParcel(parcelId).ToString() + "\n");
                                        }
                                        catch (IBL.BO.DoesntExistException exc)
                                        {
                                            Console.WriteLine(exc);
                                        }
                                        catch (IDAL.DO.DoesntExistException exc)
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
                            string data;
                            bool flag;
                            char printListChoice;
                            data = Console.ReadLine();
                            flag = char.TryParse(data, out char bad);
                            if (flag)
                                printListChoice = char.Parse(data);
                            else
                                printListChoice = '!';
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
                input = Console.ReadLine();
                check = int.TryParse(input, out error);
                if (check)
                    choice = int.Parse(input);
                else
                    choice = -1;






            }
        }
    }
}
