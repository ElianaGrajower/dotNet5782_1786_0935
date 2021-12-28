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
        public int customerId;
        public string customerName;
        public string phone;
        public int parcelsdelivered;
        public int undeliveredParcels;
        public int recievedParcel;
        public int transitParcel;
        public bool isCustomer;
            
        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId} Customer name: {customerName} Phone: {phone} Parcels delivered: {parcelsdelivered} undelivered Parcels: {undeliveredParcels} Recieved Parcel: {recievedParcel} Transit Parcel: {transitParcel}\n");
        }
    }


}
