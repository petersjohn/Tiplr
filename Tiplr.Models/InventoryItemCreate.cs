using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InventoryItemCreate
    {
        public int InventoryId { get; set; }
        public int  ProductId { get; set; }
        public double OnHandCount { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }
        public string Id { get; set; } //userId

        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}
