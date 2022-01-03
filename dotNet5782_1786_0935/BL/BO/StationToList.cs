using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace BO
{
    public class StationToList
    {
        public int stationId { get; set; }
        public string name { get; set; }
        public int numberOfAvailableSlots { get; set; }
        public int numberOfSlotsInUse { get; set; }

        public override string ToString()
        {
            return String.Format($"Station Id: {stationId} name: {name} Nmber Of Available Slots: {numberOfAvailableSlots} Number Of Slots In Use: {numberOfSlotsInUse}\n");
        }
    }
}
