using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiplr.Data;

namespace Tiplr.Models
{
    public class CategoryListItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Active { get; set; }
    }
}
