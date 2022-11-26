using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feetup.Models
{
    public class ShoppingCartModel
    {

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        public Nullable<decimal> ProductPrice { get; set; }


        public string ProductColor { get; set; }


        public string ProductImage { get; set; }


        public string ProductBrand { get; set; }


        public int CartID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public IEnumerable<ShoppingCartModel> listofshoppingCartModels { get; set;}

    }
}