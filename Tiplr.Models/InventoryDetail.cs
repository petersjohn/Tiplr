using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InventoryDetail
    {
        public int InventoryId { get; set; }
        [Display(Name = "Inventory Date")]
        public DateTimeOffset InventoryDate {get; set;}
        [Display(Name = "Inventory Finalized?")]
        public bool Finalized { get; set; }
        public string LastModUser { get; set; }
        public virtual ApplicationUser LastModifiedUser { get; set; }
        public string CreatedByUser { get; set; }
        public virtual ApplicationUser CreateUser { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }
        
        

    }
}
