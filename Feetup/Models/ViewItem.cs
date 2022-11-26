using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feetup.Models
{
    public class ViewItem
    {

        public int ProductID { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductDescription { get; set; }
        
        
        public Nullable<decimal> ProductPrice { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SellerID { get; set; }

        
        public string ProductColor { get; set; }

        
        public string ProductImage { get; set; }

        
        public string ProductBrand { get; set; }

        public string SellerPhone { get; set; }
        public string SellerEmail { get; set; }

        public string SellerName { get; set; }
       
        public string CategoryName { get; set; }
    }
}