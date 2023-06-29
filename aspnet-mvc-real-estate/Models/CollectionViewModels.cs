using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspnet_mvc_real_estate.Models
{
    public class CollectionViewModels
    {
        collection collection { get; set; }
        string thumb { get; set; }
        public CollectionViewModels()
        {
            // Empty Constructor
        }
        public CollectionViewModels(collection collection, string thumb)
        {
            this.collection = collection;
            this.thumb = thumb;
        }
    }
}