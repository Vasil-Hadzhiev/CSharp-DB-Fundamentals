using System;
using System.IO;
using FastFood.Data;
using System.Linq;
using FastFood.Models.Enums;
using FastFood.DataProcessor.Dto.Export.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using FastFood.Models;
using FastFood.DataProcessor.Dto.Export.Xml;
using Microsoft.EntityFrameworkCore;

namespace FastFood.DataProcessor
{
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
            var inputCategories = categoriesString.Split(",");

            var categories = new List<CategoryDto>();

            foreach (var cat in inputCategories)
            {
                var currentCategory = context.Categories
                    .Single(c => c.Name == cat);

                var catDto = new CategoryDto
                {
                    Name = currentCategory.Name
                };

                categories.Add(catDto);
            }

            return null;
        }
    }
}