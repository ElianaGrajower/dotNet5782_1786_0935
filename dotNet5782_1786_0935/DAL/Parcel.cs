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
            int Id;
            int SenderId;
            int TargetId;
            WeightCategories Weight;
            Priorities Priority;
            int DroneId;
            DateTime Requested;
            DateTime Scheduled;
            DateTime PickedUp;
            DateTime Delivered;
            public override string ToString()
            {
                return "Id: " + Id + "\n" + "Sender Id: " + SenderId + "\n" +
                    "Target Id: " + TargetId + "\n" + "Weight: " + Weight + "\n" +
                    "Priority: " + Priority + "\n" + "Drone Id: " + DroneId + "\n" +
                    "Requested: " + Requested + "\n" + "Scheduled: " + Scheduled + "\n" +
                    "Picked Up: " + PickedUp + "\n" + "Delivered: " + Delivered + "\n";

            }


        }
    }
}
