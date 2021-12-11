using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class StationDrone   
    {
        
        public int StationName;
        public int StationId;
        public int DroneBatteryPercentage;
        //timestamp
        public override string ToString()
        {
            return String.Format($"Station Name: {StationName}\nStation Id: {StationId}\nDrone Battery: {DroneBatteryPercentage}\n");
        }
    }
}
