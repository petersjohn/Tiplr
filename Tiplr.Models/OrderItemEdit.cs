using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Models
{
    public class OrderItemEdit
    {
        public int OrderItemItemId { get; set; }
        public int OrderAmt { get; set; }
        public int AmtReceived { get; set; }
        public decimal OrderItemTotalPrice { get; set; }
    }
}

