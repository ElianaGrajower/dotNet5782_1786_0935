using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed
namespace BO
{
    public class Customer
    {
        public int customerId { set; get; }
        public string name { set; get; }    
        public string phone { set; get; }
        public Location location{ set; get; }
        public IEnumerable<ParcelinCustomer> parcelsdelivered;
        public IEnumerable<ParcelinCustomer> parcelsOrdered;
        
        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId}\nCustomer name: {name}\nPhone: {phone}\nLocation: {location}\n");
        }

    }
}
