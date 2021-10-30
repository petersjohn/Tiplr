using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class ProductDetail
    {
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        public string ProductName { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Product Category")]
        public int? CategoryId { get; set; }
        [Display(Name = "Count Units")]
        public virtual ProductCategory ProductCategory { get; set; }
        public string CountBy { get; set; }
        [Display(Name = "Ordered By Units")]
        public string OrderBy { get; set; }
        [Display(Name = "Units per ordered pack")]

        public decimal CasePackPrice { get; set; }
        public int UnitsPerPack { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public int Par { get; set; }
        [Display(Name = "Last Updated")]
        public DateTimeOffset LastModifiedDtTm { get; set; }
        [Display(Name = "Last Updated By")]
        public string UserId { get; set; }
    }
}
