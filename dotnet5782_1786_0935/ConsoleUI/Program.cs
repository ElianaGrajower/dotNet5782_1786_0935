using System;
using IDAL.DO;
using DAL;


namespace ConsoleUI
{
    class Program
    {
        //static DalObject.DalObject Data;
        static void Main(string[] args)
        {
            /*static*/ DalObject.DalObject Data;

            Data = new DalObject.DalObject();
            Console.WriteLine("enter a ststions id: ");
            int stationId = int.Parse(Console.ReadLine());
            Console.WriteLine(Data.PrintStation(stationId));

            Console.WriteLine("enter a drones id: ");
            int droneId = int.Parse(Console.ReadLine());
            Console.WriteLine(Data.PrintDrone(droneId));

            Console.WriteLine("enter a customer id: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.WriteLine(Data.PrintCustomer(customerId));

            Console.WriteLine("enter a parcel id: ");
            int parcelId = int.Parse(Console.ReadLine());
            Console.WriteLine(Data.PrintParcel(parcelId));

            //  IDAL.DO.BaseStation baseStation = new IDAL.DO.BaseStation();
            //   Console.WriteLine(baseStation);
        }
    }
}
