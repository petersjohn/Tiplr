using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InvItemEdit
    {
        public int InventoryItemId { get; set; }
        public int InventoryId { get; set; }

        [Display(Name = "Count")]
        public decimal OnHandCount { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Display(Name = "Ordered")]
        public bool OrderedInd { get; set; }
        public DateTimeOffset LastModifiedDateTime { get; set; }
        public string LastModBy { get; set; } //user id in ApplicationUser
        public virtual ApplicationUser LastModifiedBy { get; set; }
    }
}
