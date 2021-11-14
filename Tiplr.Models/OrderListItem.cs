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
       // public virtual Inventory Inventory { get; set; }
        [Display(Name = "Inventory Date")]
        public DateTimeOffset InventoryDate { get; set; }
        [Display(Name = "Status")]
        public bool Finalized { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        [Display(Name = "Order Cost")]
        public decimal OrderCost { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        [Display(Name = "Order Date")]
        public DateTimeOffset OrderDate { get; set; }

    }
}
