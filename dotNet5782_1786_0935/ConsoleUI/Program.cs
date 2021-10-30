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
                switch(choice)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    default: Console.WriteLine("ERROR INVALID CHOICE");
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
