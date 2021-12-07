using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//add 2 string to all the classes
namespace IBL.BO
{
    public class Customer
    {
        public int CustomerId { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        public Location Location{ set; get; }
        public List<ParcelCustomer> ParcelsOrdered;
        public List<ParcelCustomer> ParcelsDelivered;
        public override string ToString()
        {
            return String.Format($"Id: {CustomerId}\nName: {Name}\nPhone: {Phone}\nLocation: {Location}\n");
        }

    }
}
