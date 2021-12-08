using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed
namespace IBL.BO
{
    public class Customer
    {
        public int CustomerId { set; get; }
        public string Name { set; get; }
        public string Phone { set; get; }
        public Location Location{ set; get; }
        public IEnumerable<ParcelinCustomer> ParcelsOrdered;
        public IEnumerable<ParcelinCustomer> ParcelsDelivered;
        public override string ToString()
        {
            return String.Format($"Id: {CustomerId}\nName: {Name}\nPhone: {Phone}\nLocation: {Location}\n");
        }

    }
}
