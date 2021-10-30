using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class ProductListItem
    {
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Inventory Category")]
        public int? CategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        [Display(Name = "Pack Size/Count")]
        public int UnitsPerPack { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public int Par { get; set; }

        public bool Active { get; set; }
    }
}
