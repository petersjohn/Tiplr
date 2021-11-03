using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderListItem
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }

    }
}
