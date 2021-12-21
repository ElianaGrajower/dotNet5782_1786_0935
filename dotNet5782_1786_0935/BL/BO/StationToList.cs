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
        public int stationId;
        public string name;  
        public int numberOfAvailableSlots;
        public int numberOfSlotsInUse;  

        public override string ToString()
        {
            return String.Format($"Station Id: {stationId} name: {name} Nmber Of Available Slots: {numberOfAvailableSlots} Number Of Slots In Use: {numberOfSlotsInUse}\n");
        }
    }
}
