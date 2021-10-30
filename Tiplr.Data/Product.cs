using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Data
{
    public class Product
    { 
        [Key]
        public int ProductId { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage = "Maximum of 50 characters allowed")]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum of 150 characters allowed")]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [ForeignKey(nameof(ProductCategory))]
        public int? CategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [Required]
        public string CountBy { get; set; }
        [Required]
        public string OrderBy { get; set; }
        [Required]
        public int UnitsPerPack { get; set; }
        [Required]
        public decimal CasePackPrice { get; set; }

        public decimal UnitPrice
        {
            /*get
            {
                var price = Math.Round((CasePackPrice / UnitsPerPack), 2);
                return price;
            }*/
            get; set;
            
        }
        public int Par { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset CreatedDtTm { get; set; }
        public DateTimeOffset LastModifiedDtTm { get; set; }
        public DateTimeOffset? InactiveDtTm { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        [Display(Name = "UserId")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
