using System;
using System.Collections.Generic;

namespace Lab.DTOs
{
    public class CartItemDTO
    {
        public IList<CartItems> Items;
    }

    public class CartItems
    {
        public string Item { get; set; }
        public int Unit { get; set; }
        public int UnitPrice { get; set; }
        public int TotalAmount { get; set; }
        public int DiscountAmount { get; set; } = 0;
        public Boolean IsPromoApplied { get; set; } = false;

        public CartItems(string item, int unit, int unitPrice)
        {
            Item = item;
            Unit = unit;
            UnitPrice = unitPrice;
            TotalAmount = unit * UnitPrice;
        }
    }


}
