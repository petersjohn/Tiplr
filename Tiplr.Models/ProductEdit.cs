using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class ProductEdit
    {
        public int ProductId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters allowed")]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum of 150 characters allowed")]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        public int? CategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [Required]
        public string CountBy { get; set; }
        [Required]
        public string OrderBy { get; set; }
        [Required]
        [Display(Name = "Case/Pack Price")]
        public decimal CasePackPrice { get; set; }
        [Required]
        public int UnitsPerPack { get; set; }
       
        public int Par { get; set; }

        public bool Active { get; set; }
    }
}
