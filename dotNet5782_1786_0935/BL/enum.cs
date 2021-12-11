using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public enum WeightCategories { light, average, heavy };
    public enum DroneStatus { available, maintenance, delivery };
    public enum Priorities { regular, fast, emergency };
    public enum ParcelStatus { created, matched, pickedUp, delivered };
}   
