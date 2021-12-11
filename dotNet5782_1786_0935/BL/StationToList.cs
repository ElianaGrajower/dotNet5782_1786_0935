using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed
namespace IBL.BO
{
    class StationToList
    {
        public int stationId;
        public int name;  
        public int numberOfAvailableSlots;
        public int numberOfSlotsInUse;  

        public override string ToString()
        {
            return String.Format($"Station Id: {stationId}\nName: {name}\nNmber Of Available Slots: {numberOfAvailableSlots}\nNumber Of Slots In Use: {numberOfSlotsInUse}\n");
        }
    }
}
