using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderFinalize
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public Guid LastUpdateUserId { get; set; }
        public Guid FinalizeUser { get; set; } //may need to revis
    }
}
