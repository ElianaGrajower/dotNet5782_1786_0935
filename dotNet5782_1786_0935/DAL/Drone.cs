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
            public int DroneId { set; get; }
            public string Model { set; get; }
            public WeightCategories MaxWeight { set; get; } //light,average,heavy
            public DroneStatuses Status { set; get; } //available, maintenance, delivery
            public double Battery { set; get; }
            public override string ToString()
            {
                return String.Format($"Id: {DroneId}\nModel: {Model}\nMaxWeight: {MaxWeight}\nStatus: {Status}\nBattery: {Battery}");
            }
        }
    }
}
