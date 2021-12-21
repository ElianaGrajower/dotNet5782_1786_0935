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
        public string sendername; //the name of the customer who is sending the parcel
        public string recivername; //the name of the customer who is the target of the parcel
        public weightCategories weight; //light, average, heavy
        public Priorities priority; //regular, fast, emergency   
        public ParcelStatus parcelStatus; 

        public override string ToString()   
        {
            return String.Format($"Parcel Id: {parcelId} Sender name: {sendername} Reciver name: {recivername} weight: {weight} priority: {priority} Parcel Status: {parcelStatus}\n");
        }
    }
}
