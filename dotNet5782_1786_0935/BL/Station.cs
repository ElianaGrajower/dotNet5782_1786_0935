using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
//updated and needed
namespace IBL.BO
{
    public class Station
    {
        public int StationId { set; get; }
        public int Name { set; get; }
        public Location Location;
        public int ChargeSlots { set; get; } //the amount of available charge slots in the station
        public void decreaseChargeSlots() { ChargeSlots--; }
        public void addChargeSlots() { ChargeSlots++; }
        public List<DroneCharging> DronesatStation;//id battery
       // public List<PastCharges> DronesLeftStation;  //extra
        public override string ToString()
        {
            return String.Format($"Id: {StationId}\nName: {Name}\nLocatiob: {Location}\nChargeSlots: {ChargeSlots}");
        }
    }
}
