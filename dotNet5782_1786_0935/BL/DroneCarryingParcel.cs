﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneCarryingParcel
    {
       
            public int DroneId { get; set; }
            public int battery { get; set; }   
            public Location location { get; set; } 
            public override string ToString()
            {
                return String.Format($"drone id:{DroneId}, battery: {battery}, location: {location}\n ");
            }
        
    }
}
