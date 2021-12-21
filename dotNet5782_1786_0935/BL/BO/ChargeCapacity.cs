using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class chargeCapacity
    {
        public double pwrAvailable { get; set; }
        public double pwrLight { get; set; }    
        public double pwrAverge { get; set; }
        public double pwrHeavy { get; set; }
        public double pwrRateLoadingDrone { get; set; }
        public double[] chargeCapacityArr { get; set; }

    }
}
