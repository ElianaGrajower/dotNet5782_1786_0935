using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace BO
{
    public class ParcelinCustomer
    {
        public int parcelId { get; set; }   
        public weightCategories weight { get; set; }
        public Priorities priority { get; set; }
        public ParcelStatus parcelStatus { get; set; }
        public CustomerInParcel customerInParcel;
        public override string ToString()
        {
            return String.Format($"Parcel Id: {parcelId} Parcel weight: {weight} priority: {priority} Parcel status: {parcelStatus} Customer In Parcel: {customerInParcel}\n");
        }

    }
}
