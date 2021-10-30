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
        
        [Display(Name = "Count")]
        public double OnHandCount { get; set; }
        public DateTimeOffset LastModifiedDateTime { get; set; }
        public string Id { get; set; } //user id in ApplicationUser
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
