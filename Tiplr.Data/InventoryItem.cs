using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class InventoryItem
    {
        [Key]
        public int InventoryItemId { get; set; }
        [Required]
        [ForeignKey(nameof(Inventory))]
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public decimal OnHandCount { get; set; }
        public bool OrderedInd { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }

        public string LastModifiedById  { get; set; }

        [ForeignKey("LastModifiedById")]
        public virtual ApplicationUser LastModBy { get; set; }



    }
}
