using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderItemEdit
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Display(Name = "Ordered Quantity")]
        public int OrderAmt { get; set; }
        [Display(Name = "Quantity Received")]
        public int AmtReceived { get; set; }
        public decimal OrderItemTotalPrice { get; set; }
    }
}

