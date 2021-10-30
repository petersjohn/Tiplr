using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Models
{
    public class InventoryDetail
    {
        public int InventoryId { get; set; }
        [Display(Name = "Inventory Date")]
        public DateTimeOffset InventoryDate {get; set;}
        [Display(Name = "Inventory Finalized?")]
        public bool Finalized { get; set; }
        public Guid CreatedByUser { get; set; }
        public string CreateUser { get; set; } //just playing around with this to see if it is viable
        public DateTimeOffset LastModifiedDtTm { get; set; }
        public Guid UpdtUser { get; set; }
        public string UpdtUserStr { get; set; }

    }
}
