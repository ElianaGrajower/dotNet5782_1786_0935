using System;
using IBL.BO;
using BL;

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
            choice = int.Parse(Console.ReadLine());
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
                            char addingChoice = char.Parse(Console.ReadLine());
                            switch (addingChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter ID of station: ");
                                        Station s = new Station() { StationId = int.Parse(Console.ReadLine()) };
                                        Console.WriteLine("Enter name of station: ");
                                        s.name = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter lattitude and longitude of station");
                                        s.location.Lattitude = double.Parse(Console.ReadLine());
                                        s.location.Longitude = double.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter amount of charge slots that station has: ");
                                        s.chargeSlots = int.Parse(Console.ReadLine());
                                        Data.AddStation(s);

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
                                        Data.AddDrone(d,stationId); //builds and adds a drone using the information the user provided
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
                                        Data.AddCustomer(c); //builds and adds a customer using the information the user provided
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
                                        Data.AddParcel(p); //builds and adds a parcel using the information the user provided
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
                            char updateChoice = char.Parse(Console.ReadLine());
                            switch (updateChoice) //chooses what to update
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter Id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter model of drone");
                                        int model = int.Parse(Console.ReadLine());
                                        Data.UpdateStationName(droneId, model);
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
                                        //if nothing is put in it should not update it- this isnt done!!!!!!!!!!!!***********
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
                                        Data.UpdateCustomerName(customerId, customerName, phone);
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Data.SendDroneToCharge(droneId);
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Enter the amount of time that the drone has been charging:");
                                        double chargeTime = double.Parse(Console.ReadLine());
                                        //dont know the name
                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Data.MatchDroneWithPacrel(droneId);
                                        break;
                                    }
                                case 'G':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Data.PickUpParcel(droneId);
                                        break;
                                    }
                                case 'H':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Data.DeliveredParcel(droneId);
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
                            char printChoice = char.Parse(Console.ReadLine());
                            switch (printChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("Enter id of station");
                                        int stationId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("\n" + Data.GetStation(stationId).ToString() + "\n");
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("Enter id of drone");
                                        int droneId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("\n" + Data.GetDrone(droneId).ToString() + "\n");
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("Enter id of customer");
                                        int customerId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("\n" + Data.GetCustomer(customerId).ToString() + "\n");
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("Enter id of parcel");
                                        int parcelId = int.Parse(Console.ReadLine());
                                        Console.WriteLine("\n" + Data.GetParcel(parcelId).ToString() + "\n");
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
                            char printListChoice = char.Parse(Console.ReadLine());
                            switch (printListChoice)
                            {
                                case 'A':
                                    {
                                        Console.WriteLine("List of stations:\n");
                                        foreach (Station item in Data.GetStationsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }
                                case 'B':
                                    {
                                        Console.WriteLine("List of drones:\n");
                                        foreach (Drone item in Data.GetDronesList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }
                                case 'C':
                                    {
                                        Console.WriteLine("List of customers:\n");
                                        foreach (Customer item in Data.GetCustomersList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }
                                case 'D':
                                    {
                                        Console.WriteLine("List of parcels:\n");
                                        foreach (Parcel item in Data.GetParcelsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }
                                case 'E':
                                    {
                                        Console.WriteLine("List of parcels that are not yet matched up to drone:\n");
                                        foreach (Parcel item in Data.GetUnmatchedParcelsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }
                                case 'F':
                                    {
                                        Console.WriteLine("List of stations with availablechargeslots:\n");
                                        foreach (Station item in Data.GetAvailableStationsList()) { Console.WriteLine(item.ToString() + "\n"); };
                                        break;
                                    }


                            }
                            break;
                        }
                    default:
                        Console.WriteLine("ERROR INVALID CHOICE");
                        break;
                }


            }
        }
    }
}
