using System;
using FastFood.Data;
using System.Linq;
using System.Text;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
            var item = context.Items
                .SingleOrDefault(i => i.Name == itemName);

            var sb = new StringBuilder();

            if (item == null)
            {
                sb.AppendLine($"Item {itemName} not found!");
            }
            else
            {
                var oldPrice = item.Price;
                item.Price = newPrice;
                sb.AppendLine($"{itemName} Price updated from ${oldPrice:f2} to ${newPrice}");
                context.SaveChanges();
            }

            var result = sb.ToString();

            return result;
	    }
    }
}