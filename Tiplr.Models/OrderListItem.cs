using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderListItem
    {
        public int OrderId { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }

    }
}
