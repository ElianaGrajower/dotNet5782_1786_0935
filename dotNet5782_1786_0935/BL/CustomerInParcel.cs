using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace IBL.BO
{
    public class CustomerInParcel
    {
        public int customerId;
        public string customerName;
            
        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId}\nCustomer name: {customerName}\n");
        }
    }
}
