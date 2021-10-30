using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public decimal OrderCost { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser{ get; set; }

    }
}
