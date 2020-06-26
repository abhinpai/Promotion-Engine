using System.Collections.Generic;

namespace Lab.DTOs
{
    public class ActivePromotionDTO
    {
        public IList<CombinedPromo> Combined;
        public IList<Discount> cummilativeDiscount;
    }

    public class CombinedPromo
    {
        public IList<Items> Product;
        public int Price;
        public int Unit;
    }

    public class Items
    {
        public string Name;
    }

   public class Discount
    {
        public string Item;
        public int Unit;
        public int DiscountPrice;
    }
}


