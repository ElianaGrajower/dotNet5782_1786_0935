using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public enum WeightCategories { light=1, average, heavy };
    public enum DroneStatus { available=1, maintenance, delivery };
    public enum Priorities { regular=1, fast, emergency };
    public enum ParcelStatus { created=1, matched, pickedUp, delivered };
}   
