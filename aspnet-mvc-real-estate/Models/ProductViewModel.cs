using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspnet_mvc_real_estate.Models
{
    public class ProductViewModel
    {
        public product product { get; set; }
        public string thumb { get; set; }
        public ProductViewModel()
        {
            // Empty Constructor
        }
        public ProductViewModel(product product, string thumb)
        {
            this.product = product;
            this.thumb = thumb;
        }
    }
}