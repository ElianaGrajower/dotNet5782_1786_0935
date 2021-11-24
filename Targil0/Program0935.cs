using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0935();
            Welcome1786();
            Console.ReadKey();
        }
        static partial void Welcome1786();
        private static void Welcome0935()
        {
            Console.Write("Enter your name: ");
            string name;
            name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application ", name);
            Console.WriteLine("Press any key to continue...");
            
        }
    }
}
