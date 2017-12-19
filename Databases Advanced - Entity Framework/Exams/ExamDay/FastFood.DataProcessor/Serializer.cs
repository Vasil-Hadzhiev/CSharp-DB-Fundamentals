namespace FastFood.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using System.Xml.Serialization;
    using System.Xml;
    using Formatting = Newtonsoft.Json.Formatting;

    using FastFood.Models;
    using FastFood.Data;
    using FastFood.Models.Enums;
    using FastFood.DataProcessor.Dto.Export.Json;
    using FastFood.DataProcessor.Dto.Export.Xml;

    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var type = Enum.Parse<OrderType>(orderType);

            var employee = context.Employees
                .Where(e => e.Name == employeeName)
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders.Where(o => o.Type == type)
                        .Select(o => new OrderDto
                        {
                            Customer = o.Customer,
                            Items = o.OrderItems.
                                Select(oi => new ItemDto
                                {
                                    Name = oi.Item.Name,
                                    Price = oi.Item.Price,
                                    Quantity = oi.Quantity
                                })
                                .ToArray(),                               
                            TotalPrice = o.OrderItems.Select(oi => oi.Item.Price * oi.Quantity).Sum()
                        })
                        .OrderByDescending(o => o.TotalPrice)
                        .ThenByDescending(o => o.Items.Count())
                        .ToArray()
                })
                .Select(e => new EmployeeDto
                {
                    Name = e.Name,
                    Orders = e.Orders,
                    TotalMade = e.Orders.Select(o => o.TotalPrice).Sum()
                }).
                First();


            var json = JsonConvert.SerializeObject(employee, Formatting.Indented);

            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var categories = categoriesString.Split(',');

            var categoryStatistics = context.Items
                .Where(i => categories.Any(c => c == i.Category.Name))
                .GroupBy(i => i.Category.Name)
                .Select(g => new CategoryDto
                {
                    Name = g.Key,
                    MostPopularItem = g.Select(i => new MostPopularItemDto
                    {
                        Name = i.Name,
                        TotalMade = i.OrderItems.Sum(oi => oi.Quantity * oi.Item.Price),
                        TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                    })
                        .OrderByDescending(i => i.TotalMade)
                        .ThenByDescending(i => i.TimesSold)
                        .First()
                })
                .OrderByDescending(dto => dto.MostPopularItem.TotalMade)
                .ThenByDescending(dto => dto.MostPopularItem.TimesSold)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
            serializer.Serialize(new StringWriter(sb), categoryStatistics, namespaces);

            var result = sb.ToString();

            return result;
        }
    }
}