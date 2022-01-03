using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace BO
{
   public class ParcelToList
    {
        public int parcelId { get; set; }
        public string sendername { get; set; } //the name of the customer who is sending the parcel
        public string recivername { get; set; } //the name of the customer who is the target of the parcel
        public weightCategories weight { get; set; } //light, average, heavy
        public Priorities priority { get; set; } //regular, fast, emergency   
        public ParcelStatus parcelStatus { get; set; }

        public override string ToString()   
        {
            return String.Format($"Parcel Id: {parcelId} Sender name: {sendername} Reciver name: {recivername} weight: {weight} priority: {priority} Parcel Status: {parcelStatus}\n");
        }
    }
}
