﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated and needed

namespace BO
{
    public class CustomerInParcel
    {
        public int customerId { set; get; }
        public string customerName { set; get; }

        public override string ToString()
        {
            return String.Format($"Customer Id: {customerId}\nCustomer name: {customerName}\n");
        }
    }
}
