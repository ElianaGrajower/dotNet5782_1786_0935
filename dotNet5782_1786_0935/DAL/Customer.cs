using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
       public struct Customer
        {
            public int customerId { set; get; }
            public string name { set; get; }
            public string Phone { set; get; }
            public double lattitude { set; get; }
            public double longitude { set; get; }
            public override string ToString()
            {
                return String.Format($"Id: {customerId}\nname: {name}\nPhone: {Phone}\nlattitude: {lattitude}\nlongitude: {longitude}\n");
            }
        }
    }

