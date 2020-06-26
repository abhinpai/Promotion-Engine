namespace Lab
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lab.DTOs;
    using Lab.Stock;

    class Program
    {
        static void Main(string[] args)
        {
            IList<Stocks> stockItems = InitiateStock();
            ActivePromotionDTO activeDiscount = InitiateActivePromotion();
            CartItemDTO cartItems = GetCartItems();
            logMylogic(stockItems, activeDiscount, ApplyPromotion(stockItems, activeDiscount, cartItems));
        }

        static IList<Stocks> InitiateStock()
        {
            return new List<Stocks>
            {
                new Stocks { item = "SKU-A", unitPrice = 50 },
                new Stocks { item = "SKU-B", unitPrice = 30 },
                new Stocks { item = "SKU-C", unitPrice = 20 },
                new Stocks { item = "SKU-D", unitPrice = 15 },
            };
        }

        static CartItemDTO GetCartItems()
        {
            return new CartItemDTO
            {
                Items = new List<CartItems>
                {
                    new CartItems("SKU-A", 3, 50),
                    new CartItems("SKU-B", 5, 30),
                    new CartItems("SKU-C", 1, 20),
                    new CartItems("SKU-D", 1, 15)
                }
            };
        }

        static ActivePromotionDTO InitiateActivePromotion()
        {
            return new ActivePromotionDTO
            {
                Combined = new List<CombinedPromo>
                {
                     new CombinedPromo {
                        Product = new List<Items>
                        {
                            new Items { Name = "SKU-D"},
                            new Items { Name = "SKU-C"},
                        },
                        Price = 30,
                        Unit = 1
                    },
                     new CombinedPromo {
                        Product = new List<Items>
                        {
                            new Items { Name = "SKU-D"},
                            new Items { Name = "SKU-A"},
                        },
                        Price = 40,
                        Unit = 1
                    }
                },
                cummilativeDiscount = new List<Discount>
                {
                    new Discount { Item= "SKU-A", Unit = 3, DiscountPrice = 130 },
                    new Discount { Item= "SKU-B", Unit = 2, DiscountPrice = 45 },
                }
            };
        }

        private static void logMylogic(IList<Stocks> stocks, ActivePromotionDTO promotions, CartItemDTO cartItems)
        {
            Console.WriteLine("Total Combined Promotion: " + promotions.Combined.Count());
            Console.WriteLine("Total Individual Discount Promotion: " + promotions.cummilativeDiscount.Count());
            foreach (CartItems item in cartItems.Items)
            {
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Item: " + item.Item);
                Console.WriteLine("UnitPrice: " + item.UnitPrice);
                Console.WriteLine("Unit: " + item.Unit);
                Console.WriteLine("IsPromoApplied: " + item.IsPromoApplied);
                Console.WriteLine("DiscountAmount: " + item.DiscountAmount);
                Console.WriteLine("TotalAmount: " + item.TotalAmount);
            }
        }

        private static CartItemDTO ApplyPromotion(IList<Stocks> stockItems, ActivePromotionDTO activeDiscount, CartItemDTO cartItems)
        {
            foreach (Discount discount in activeDiscount.cummilativeDiscount)
            {
                foreach (CartItems cItem in cartItems.Items)
                {
                    if (cItem.Item == discount.Item && cItem.Unit >= 3)
                    {
                        cItem.DiscountAmount = computePrice(cItem.Unit, cItem.UnitPrice, cItem.DiscountAmount, discount.DiscountPrice, discount.Unit);
                        cItem.IsPromoApplied = true;
                    }
                }
            }

            IEnumerable<CartItems> cartitemAfterPomo = new List<CartItems>();
            foreach (CombinedPromo combinedPromo in activeDiscount.Combined)
            {
                IEnumerable<CartItems> validItems = cartItems.Items.Where(cItem => combinedPromo.Product.Any(item => cItem.Item == item.Name && !cItem.IsPromoApplied));
                cartitemAfterPomo = computeCombinedDis(validItems, combinedPromo);
            }

            cartitemAfterPomo.ToList().ForEach(item =>
            {
                cartItems.Items.Where(x => x.Item == item.Item).Select(w => w.DiscountAmount = item.DiscountAmount);
            });

            return cartItems;
        }

        private static IEnumerable<CartItems> computeCombinedDis(IEnumerable<CartItems> items, CombinedPromo combinedPromo)
        {
            bool pomoApplied = false;
            foreach (CartItems item in items)
            {
                if (!pomoApplied)
                {
                    item.DiscountAmount = computeTotal(item.Unit, item.UnitPrice, item.DiscountAmount);
                }
                item.IsPromoApplied = true;
                pomoApplied = true;
            }

            items.ToList()[0].DiscountAmount = items.ToList()[0].TotalAmount - items.ToList()[0].UnitPrice + combinedPromo.Price;
            items.ToList()[1].DiscountAmount = items.ToList()[0].TotalAmount - items.ToList()[0].UnitPrice;

            return items;
        }

        private static int computeTotal(int totalQty, int actualPrice, int totalDiscount)
        {
            if (totalQty == 0) return totalDiscount;
            totalDiscount += actualPrice;
            return computeTotal(totalQty - 1, actualPrice, totalDiscount);
        }

        private static int computePrice(int totalQty, int actualPrice, int totalDiscount, int discountPrice, int disUnit)
        {
            int leftQty = totalQty;
            if (leftQty == 0)
                return totalDiscount;
            if (leftQty >= disUnit)
            {
                totalDiscount += discountPrice;
                return computePrice(leftQty - disUnit, actualPrice, totalDiscount, discountPrice, disUnit);
            }
            totalDiscount += actualPrice;
            return computePrice(leftQty - 1, actualPrice, totalDiscount, discountPrice, disUnit);
        }
    }
}






