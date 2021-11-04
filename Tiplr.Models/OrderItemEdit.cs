using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderItemEdit
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderAmt { get; set; }
        public int AmtReceived { get; set; }
        public decimal OrderItemTotalPrice { get; set; }
    }
}

