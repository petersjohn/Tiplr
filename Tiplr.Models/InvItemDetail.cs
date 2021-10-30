using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InvItemDetail
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Display(Name = "Count")]
        public double OnHandCount { get; set; }
        public string UpdtUser { get; set; }

    }
}
