using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ParcelCustomer
    {
        public int ParcelId { get; set; }
        public int CustomerId { get; set; }
        public IBL.BO.WeightCategories weight { get; set; }
        public IBL.BO.Priorities priority { get; set; }
        public IBL.BO.ParcelStatus parcelStatus { get; set; }
        
    }
}
