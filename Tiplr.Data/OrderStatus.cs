using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        public string OrderStatusDisplay { get; set; }
        public bool Active { get; set; }
    }
}
