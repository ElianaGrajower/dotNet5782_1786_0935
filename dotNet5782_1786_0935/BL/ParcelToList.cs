using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace IBL.BO
{
   public class ParcelToList
    {
        public int parcelId;
        public string senderName;
        public string reciverName;
        public WeightCategories weight;
        public Priorities priority;
        public ParcelStatus parcelStatus;

        public override string ToString()   
        {
            return String.Format($"Parcel Id: {parcelId}\nSender Name: {senderName}\nReciver Name: {reciverName}\nWeight: {weight}\nPriority: {priority}\nParcel Status: {parcelStatus}\n");
        }
    }
}
