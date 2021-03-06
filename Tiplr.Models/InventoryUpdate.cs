using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Models
{
    public class InventoryUpdate
    {
        public int InventoryId { get; set; }
        [Display(Name = "Inventory Finalized?")]
        public bool Finalized { get; set; }
        public decimal TotalOnHandValue { get; set; }
        public string LastModifiedBy { get; set; }

    }
}
