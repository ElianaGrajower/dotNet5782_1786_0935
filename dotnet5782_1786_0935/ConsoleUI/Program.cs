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
            int choice;
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1- To add new item");
            Console.WriteLine("2- To update item");
            Console.WriteLine("3- to print item details");
            Console.WriteLine("4-to print list of items");
            choice = int.Parse(Console.ReadLine());
            while (choice != 5)
            {
                switch (choice)
                {
                    case 1:
                    case 2:
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
                            if (Choice_Print == 'b')
                                Console.Write("drone ");
                            if (Choice_Print == 'C')
                                Console.Write("customer ");
                            if (Choice_Print == 'D')
                                Console.Write("pacel ");
                            Console.WriteLine("id");
                            Id = int.Parse(Console.ReadLine());

                            switch (Choice_Print)
                            {
                                case 'A':
                                    {
                                        Data.PrintStation(Id);
                                        break;
                                    }
                                case 'B':
                                    {
                                        Data.PrintDrone(Id);
                                        break;
                                    }
                                case 'C':
                                    {
                                        Data.PrintCustomer(Id);
                                        break;
                                    }
                                case 'D':
                                    {
                                        Data.PrintParcel(Id);
                                        break;
                                    }
                                default: break;

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
                            Choice_Print = char.Parse(Console.ReadLine());
                            switch (Choice_Print)
                            {
                                case 'A':
                                    {
                                        Data.printStationsList();
                                        break;
                                    }
                                case 'B':
                                    {
                                        Data.printDronesList();
                                        break;
                                    }
                                case 'C':
                                    {
                                        Data.printCustomersList();
                                        break;
                                    }
                                case 'D':
                                    {
                                        Data.printParcelsList();
                                        break;
                                    }
                                default:
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
                Console.WriteLine("3- to print item details");
                Console.WriteLine("4-to print list of items");
                Console.WriteLine("5-to exit");
            }

            //  IDAL.DO.BaseStation baseStation = new IDAL.DO.BaseStation();
            //   Console.WriteLine(baseStation);
        }
    }
}
