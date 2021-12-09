using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace IBL.BO
{
    public class ParcelinCustomer
    {
        public int ParcelId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel CustomerInParcel;
        public override string ToString()
        {
            return String.Format($"Parcel Id: {ParcelId}\nParcel Weight: {Weight}\nPriority: {Priority}\nParcel status: {ParcelStatus}\nCustomer In Parcel: {CustomerInParcel}\n");
        }

    }
}
