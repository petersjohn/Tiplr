using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InventoryCountItem
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        //public virtual Inventory Inventory { get; set; }
        [Display(Name = "Inventory Date")]
        public DateTimeOffset InventoryDate { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "Ordered")]
        public bool OrderedInd { get; set; }
        public int? OrderId { get; set; }
        public virtual Product Product { get; set; }
        [Display(Name = "Count")]
        public decimal OnHandCount { get; set; }
        public int ParVolume { get; set; }
    }


}



