using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class OrderEdit
    {
        [Required]
        public int OrderId { get; set; }
        [Display(Name = "Order Status")]
        public int? OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public decimal OrderCost { get; set; }
        public string LastUpdateUserId { get; set; }
    }
}
