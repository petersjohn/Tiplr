using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Guid CreatedByUser { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }
        public Guid UpdtUser { get; set; }
    }
}
