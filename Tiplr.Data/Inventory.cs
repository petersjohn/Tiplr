using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        public DateTimeOffset InventoryDate { get; set; }
        public bool Finalized { get; set; }
        public string CreatedByUser { get; set; }
        public string LastModUser { get; set; }
        public decimal TotalOnHandValue { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }

        [ForeignKey("CreatedByUser")]
        public virtual ApplicationUser CreatedBy { get; set; }
        [ForeignKey("LastModUser")]
        public virtual ApplicationUser LastModBy { get; set; }
      
    }
}
