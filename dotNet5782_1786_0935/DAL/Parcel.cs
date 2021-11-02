using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int id { set; get; }
            public int SenderId { set; get; }
            public int TargetId { set; get; }
            public WeightCategories Weight { set; get; }
            public Priorities Priority { set; get; }
            public int DroneId { set; get; }
            public DateTime Requested { set; get; }
            public DateTime Scheduled { set; get; }
            public DateTime PickedUp { set; get; }
            public DateTime Delivered { set; get; }
            public override string ToString()
            {
                return String.Format($" Parcel Id: {id}\nSender Id: {SenderId}\nTarget Id: {TargetId}\nWeight: {Weight}\nPriority: {Priority}\nDrone Id: {DroneId}\nRequested: { Requested}\nScheduled: {Scheduled}\nPicked Up: { PickedUp }\nDelivered: { Delivered}");
                
                



                

            }
            


        }
    }
}
