using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiplr.Models
{
    public class CategoryEdit
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Active { get; set; }
    }
}
