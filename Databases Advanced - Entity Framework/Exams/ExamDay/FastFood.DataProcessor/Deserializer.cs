namespace FastFood.DataProcessor
{
    using System;   
    using Newtonsoft.Json;    
    using System.Text;    
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using System.Globalization;

    using FastFood.DataProcessor.Dto.Import;
    using FastFood.Models.Enums;
    using FastFood.Models;
    using FastFood.Data;

    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();

            var validEmployees = new List<Employee>();

            foreach (var empDto in deserializedEmployees)
            {
                if (!IsValid(empDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var position = context.Positions
                    .SingleOrDefault(p => p.Name == empDto.Position);

                if (position == null)
                {
                    position = new Position
                    {
                        Name = empDto.Position
                    };

                    context.Positions.Add(position);
                    context.SaveChanges();
                }

                var employee = new Employee
                {
                    Name = empDto.Name,
                    Age = empDto.Age,
                    Position = position
                };

                validEmployees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, empDto.Name));
            }

            context.Employees.AddRange(validEmployees);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var sb = new StringBuilder();

            var validItems = new List<Item>();

            foreach (var itemDto in deserializedItems)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var itemExists = validItems
                    .Any(i => i.Name == itemDto.Name);

                if (itemExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = context.Categories
                    .SingleOrDefault(c => c.Name == itemDto.Category);

                if (category == null)
                {
                    category = new Category
                    {
                        Name = itemDto.Category
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                }

                var item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
                };

                validItems.Add(item);

                sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
            }

            context.Items.AddRange(validItems);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

            var sb = new StringBuilder();

            var validOrders = new List<Order>();

            foreach (var orderDto in deserializedOrders)
            {
                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var itemsAreValid = orderDto.Items.All(IsValid);

                if (!itemsAreValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var employee = context.Employees
                    .SingleOrDefault(e => e.Name == orderDto.Employee);

                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var allItemsExist = true;

                foreach (var item in orderDto.Items)
                {
                    allItemsExist = context.Items
                        .Any(i => i.Name == item.Name);

                    if (!allItemsExist)
                    {
                        break;
                    }
                }

                if (!allItemsExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var orderItems = new List<OrderItem>();

                foreach (var itemDto in orderDto.Items)
                {
                    var item = context.Items
                        .SingleOrDefault(i => i.Name == itemDto.Name);

                    if (orderItems.Any(oi => oi.Item == item))
                    {
                        var currentOrderItem = orderItems.Single(oi => oi.Item == item);
                        currentOrderItem.Quantity += itemDto.Quantity;
                    }
                    else
                    {
                        var orderItem = new OrderItem
                        {
                            Item = item,
                            Quantity = itemDto.Quantity
                        };

                        orderItems.Add(orderItem);
                    }
                }

                var date = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var type = Enum.Parse<OrderType>(orderDto.Type);

                var order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = type,
                    OrderItems = orderItems
                };

                foreach (var orderItem in orderItems)
                {
                    orderItem.Order = order;

                    var currentItem = context.Items.Single(i => i.Name == orderItem.Item.Name);
                    currentItem.OrderItems.Add(orderItem);
                }

                validOrders.Add(order);
                sb.AppendLine($"Order for {orderDto.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(validOrders);
            context.SaveChanges();

            var result = sb.ToString();

            return result;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            return isValid;
        }
    }
}