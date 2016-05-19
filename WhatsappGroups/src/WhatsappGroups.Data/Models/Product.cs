using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WhatsappGroups.Data.Models
{
    public class Product
    {
        [Display(Name = "Product ID")]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Product Description")]
        public string  Description { get; set; }
        [Display(Name = "Product Price")]
        public decimal Price { get; set; }
    }
}
