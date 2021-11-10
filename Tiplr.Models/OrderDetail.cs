using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int InventoryId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public virtual Inventory Inventory { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public decimal OrderCost { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser{ get; set; }

    }
}
