using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        [ForeignKey(nameof(Inventory))]
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public decimal OrderCost { get; set; }
        public int? OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string LastModifiedById { get; set; }
        [ForeignKey("LastModifiedById")]
        public virtual ApplicationUser LastModBy { get; set; }
        public virtual ICollection<OrderItem> orderItems { get; set; }

        
        




    }
}
