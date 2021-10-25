using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            int id { set; get; }
            string Model { set; get; }
            WeightCategories MaxWeight { set; get; }
            DroneStatuses Status { set; get; }
            double Battery { set; get; }
        }
    }
}
