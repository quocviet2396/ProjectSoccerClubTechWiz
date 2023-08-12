using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectModels
{
    public class CartItem
    {
        public Products Product { get; set; }
        public int Qty { get; set; }

        public CartItem(Products Product, int Qty) { 
            this.Product = Product;
            this.Qty = Qty;
        }
    }
}
