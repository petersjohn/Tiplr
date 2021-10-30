using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderItemListItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int OrderAmt { get; set; }
        public int AmtReceived { get; set; }
        public int InventoryItemId { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
    }
}
