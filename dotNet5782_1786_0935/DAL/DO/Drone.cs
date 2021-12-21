using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        public struct Drone
        {
            public int droneId { set; get; }
            public string model { set; get; }
            public weightCategories maxWeight; //;{ set; get; } //light,average,heavy
                                                            //public DroneStatuses Status { set; get; } //available, maintenance, delivery
                                                            //  public double Battery { set; get; }
            public override string ToString()
            {
                return String.Format($"Id: {droneId}\nModel: {model}\nMaxweight: {maxWeight}\n");
                //Status: {Status}\nBattery: {Battery}");
            }
        }
    }

