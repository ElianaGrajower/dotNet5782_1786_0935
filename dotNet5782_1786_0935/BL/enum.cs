﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum weightCategories { light=1, average, heavy,clear };
    public enum DroneStatus { available=1, maintenance, delivery,clear };
    public enum Priorities { regular=1, fast, emergency };
    public enum ParcelStatus { created=1, matched, pickedUp, delivered };
}   
