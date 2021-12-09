using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace IBL.BO
{
    public class CustomerToList
    {
        public int customerId;
        public string customerName;
        public string phone;
        public int parcelsDelivered;
        public int undeliveredParcels;
        public int recievedParcel;
        public int transitParcel;

        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId}\nCustomer Name: {customerName}\nPhone: {phone}\nParcels Delivered: {parcelsDelivered}\nundelivered Parcels: {undeliveredParcels}\nRecieved Parcel: {recievedParcel}\nTransit Parcel: {transitParcel}\n");
        }
    }


}
