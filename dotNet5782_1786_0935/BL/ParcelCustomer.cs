using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ParcelCustomer
    {
        public int ParcelId { get; set; }
        public int CustomerId { get; set; }
        public IBL.BO.WeightCategories Weight { get; set; }
        public IBL.BO.Priorities Priority { get; set; }
        public IBL.BO.ParcelStatus ParcelStatus { get; set; }
        public override string ToString()
        {
            return String.Format($"Parcel Id: {ParcelId}\nCustomer Id: {CustomerId}\nParcel Weight: {Weight}\nPriority: {Priority}\nParcel status: {ParcelStatus}\n");
        }

    }
}
