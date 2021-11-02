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
            public int id { set; get; }
            public string Model { set; get; }
            public WeightCategories MaxWeight { set; get; }
            public DroneStatuses Status { set; get; }
            public double Battery { set; get; }
            public override string ToString()
            {
                return String.Format($"id: {id }\nModel: {Model}\nMaxWeight: {MaxWeight}\nStatus: {Status}\nBattery: {Battery}");
            }
        }
    }
}
