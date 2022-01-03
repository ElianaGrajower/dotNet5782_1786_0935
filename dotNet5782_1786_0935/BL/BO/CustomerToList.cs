using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace BO
{
    public class CustomerToList
    {
        public int customerId { get; set; }
        public string customerName { get; set; }
        public string phone { get; set; }
        public int parcelsdelivered { get; set; }
        public int undeliveredParcels { get; set; }
        public int recievedParcel { get; set; }
        public int transitParcel { get; set; }
        public bool isCustomer { get; set; }

        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId} Customer name: {customerName} Phone: {phone} Parcels delivered: {parcelsdelivered} undelivered Parcels: {undeliveredParcels} Recieved Parcel: {recievedParcel} Transit Parcel: {transitParcel}\n");
        }
    }


}
