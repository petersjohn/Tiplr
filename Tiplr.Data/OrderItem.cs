using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        [ForeignKey(nameof(InventoryItem))]
        public int InventoryItemId { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }

        [ForeignKey(nameof(Order))]
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int OrderAmt { get; set; }
        public int AmtReceived { get; set; }
        public decimal OrderItemTotalPrice { get; set; }
    }
}
