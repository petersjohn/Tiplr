using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class InventoryItemCreate
    {
        public int InventoryId { get; set; }
        public int  ProductId { get; set; }
        public virtual Product Product{ get; set; }

        public decimal OnHandCount { get; set; }
        
        public DateTimeOffset LastModifiedDtTm { get; set; }
        public string LastModUser { get; set; }
        public virtual ApplicationUser LastModifiedUser { get; set; }


    }
}
