namespace Products.Client
{
    using System;
    using Newtonsoft.Json;

    using Models;
    using Data;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class Application
    {
        public static void Main()
        {
            var db = new ProductsContext();

            var result = string.Empty;

            using (db)
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                //ImportUsersFromJson();
                //ImportCategoriesFromJson();
                //ImportProductsFromJson();
                //SetCategories();
                //GetProductsInPriceRangeJson();
                //GetUsersWithSuccessfullySoldProductsJson();
                //GetCategoriesByProductsCountJson();
                //GetUsersAndProductsJson();
                //ImportUsersFromXml();
                //ImportCategoriesFromXml();
                //ImportProductsFromXml();
                //ExportProductsInPriceRangeXml();
                //ExportUsersWithSuccessfullySoldProductsXml();
                //ExportCategoriesByProductXml();
                //ExportUsersAndProductsXml();
            }
        }

        private static void ExportUsersAndProductsXml()
        {
            var context = new ProductsContext();

            using (context)
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Any())
                    .OrderByDescending(u => u.SoldProducts.Count())
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Age,
                        SoldProducts = new
                        {
                            Count = u.SoldProducts.Count(),
                            Products = u.SoldProducts
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price
                                    })
                        }
                    })
                    .ToArray();

                var document = new XDocument();

                document.Add(
                    new XElement("users",
                        new XAttribute("count", users.Count())));

                foreach (var user in users)
                {
                    document.Root.Add(
                        new XElement("user",
                            new XAttribute("first-name", $"{user.FirstName}"),
                            new XAttribute("last-name", $"{user.LastName}"),
                            new XAttribute("age", $"{user.Age}"),
                                new XElement("sold-products",
                                new XAttribute("count", $"{user.SoldProducts.Count}"))));

                    var element = document.Root.Elements()
                        .SingleOrDefault(e => 
                            e.Name == "user" && 
                            e.Attribute("first-name").Value == $"{user.FirstName}" && 
                            e.Attribute("last-name").Value == $"{user.LastName}" && 
                            e.Attribute("age").Value == $"{user.Age}")
                        .Elements()
                        .SingleOrDefault(e => e.Name == "sold-products");

                    foreach (var p in user.SoldProducts.Products)
                    {
                        element.Add(
                            new XElement("product",
                                new XAttribute("name", $"{p.Name}"),
                                new XAttribute("price", $"{p.Price}")));
                    }
                }

                document.Save("Files/XMLExports/ExportsUsersAndProducts.xml");
            }
        }

        private static void ExportCategoriesByProductXml()
        {
            var context = new ProductsContext();

            using (context)
            {
                var categories = context.Categories
                    .OrderBy(c => c.CategoryProducts.Select(cp => cp.ProductId).Count())
                    .Select(c => new
                    {
                        c.Name,
                        ProductsCount = c.CategoryProducts.Select(cp => cp.ProductId).Count(),
                        AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                        TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum(),
                    })
                    .ToArray();

                var document = new XDocument();

                document.Add(new XElement("categories"));

                foreach (var category in categories)
                {
                    document.Root.Add(
                        new XElement("category",
                            new XAttribute("name", $"{category.Name}"),
                                new XElement("products-count", $"{category.ProductsCount}"),
                                new XElement("average-price", $"{category.AveragePrice}"),
                                new XElement("total-revenue", $"{category.TotalRevenue}")));

                }

                document.Save("Files/XMLExports/CategoriesByProductsCount.xml");
            }
        }

        private static void ExportUsersWithSuccessfullySoldProductsXml()
        {
            var context = new ProductsContext();

            using (context)
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Any())
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        SoldProducts = u.SoldProducts
                            .Select(sp => new
                            {
                                sp.Name,
                                sp.Price
                            })
                    })
                    .ToArray();

                var document = new XDocument();

                document.Add(new XElement("users"));

                foreach (var user in users)
                {
                    document.Root.Add(
                        new XElement("user",
                        new XAttribute("first-name", $"{user.FirstName}"),
                        new XAttribute("last-name", $"{user.LastName}"),
                            new XElement("sold-products")));

                    var products = document.Root.Elements()
                        .SingleOrDefault(e =>
                            e.Name == "user" &&
                            e.Attribute("first-name").Value == $"{user.FirstName}" &&
                            e.Attribute("last-name").Value == $"{user.LastName}")
                            .Element("sold-products");

                    foreach (var product in user.SoldProducts)
                    {
                        products
                            .Add(
                                 new XElement("product",
                                 new XElement("name", $"${product.Name}"),
                                 new XElement("price", $"{product.Price}")));
                    }
                }

                document.Save("Files/XMLExports/SoldProducts.xml");
            }
        }

        private static void ExportProductsInPriceRangeXml()
        {
            var context = new ProductsContext();

            using (context)
            {
                var products = context.Products
                    .Where(p => p.Price >= 1000 && p.Price <= 2000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                    })
                    .ToArray();

                var document = new XDocument();

                document.Add(new XElement("products"));

                foreach (var product in products)
                {
                    document.Root.Add(
                        new XElement("product",
                            new XAttribute("name", $"{product.Name}"),
                            new XAttribute("price", $"{product.Price}"),
                            new XAttribute("buyer", $"{product.BuyerName}")));
                }

                document.Save("Files/XMLExports/ProductsInRange.xml");
            }
        }

        private static void ImportProductsFromXml()
        {
            var xmlString = File.ReadAllText("Files/products.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var context = new ProductsContext();

            using (context)
            {
                var categoryProducts = new List<CategoryProduct>();

                var userIds = context.Users
                    .Select(u => u.Id)
                    .ToArray();

                var categoryIds = context.Categories
                    .Select(c => c.Id)
                    .ToArray();

                var rnd = new Random();

                foreach (var e in elements)
                {
                    var name = e.Element("name").Value;
                    var price = decimal.Parse(e.Element("price").Value);

                    var sellerIndex = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[sellerIndex];

                    var product = new Product
                    {
                        Name = name,
                        Price = price,
                        SellerId = sellerId
                    };

                    var categoryIndex = rnd.Next(0, categoryIds.Length);
                    var categoryId = userIds[categoryIndex];

                    var categoryProduct = new CategoryProduct
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    categoryProducts.Add(categoryProduct);
                }

                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();

                var productBuyers = context.Products.ToArray();

                for (int i = 0; i < productBuyers.Length / 2; i++)
                {
                    var currentProduct = productBuyers[i];

                    var buyerIndex = rnd.Next(0, userIds.Length);
                    var buyerId = userIds[buyerIndex];

                    while (currentProduct.SellerId == buyerId)
                    {
                        buyerIndex = rnd.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];
                    }

                    currentProduct.BuyerId = buyerId;
                }

                context.SaveChanges();
            }
        }

        private static void ImportCategoriesFromXml()
        {
            var xmlString = File.ReadAllText("Files/categories.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var categories = new List<Category>();

            foreach (var e in elements)
            {
                var category = new Category
                {
                    Name = e.Element("name").Value
                };

                categories.Add(category);
            }

            var context = new ProductsContext();

            context.Categories.AddRange(categories);

            context.SaveChanges();
        }

        private static void ImportUsersFromXml()
        {
            var xmlString = File.ReadAllText("Files/users.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var users = new List<User>();

            foreach (var e in elements)
            {
                var firstName = e.Attribute("firstName")?.Value;
                var lastName = e.Attribute("lastName").Value;

                int? age = null;

                if (e.Attribute("age") != null)
                {
                    age = int.Parse(e.Attribute("age").Value);
                }

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };

                users.Add(user);
            }

            var context = new ProductsContext();

            using (context)
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void GetUsersAndProductsJson()
        {
            var context = new ProductsContext();

            using (context)
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Any())
                    .OrderByDescending(u => u.SoldProducts.Count())
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Age,
                        SoldProducts = new
                        {
                            count = u.SoldProducts.Count(),
                            products = u.SoldProducts
                                .Select(sp => new
                                {
                                    sp.Name,
                                    sp.Price
                                })
                        }
                    })
                    .ToArray();

                var usersWithCount = new
                {
                    usersCount = users.Count(),
                    users = users
                };

                var jsonString = JsonConvert.SerializeObject(usersWithCount, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("Files/UsersAndProducts.json", jsonString);
            }
        }

        private static void GetCategoriesByProductsCountJson()
        {
            var context = new ProductsContext();

            var categories = context.Categories
                .Where(c => c.CategoryProducts.Count() > 0)
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoryProducts.Select(cp => cp.CategoryId).Count(),
                    AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).Average(),
                    TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum()
                })
               .ToArray();

            var jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented);

            File.WriteAllText("Files/CategoriesByProductsCount.json", jsonString);
        }

        private static void GetUsersWithSuccessfullySoldProductsJson()
        {
            var context = new ProductsContext();

            using (context)
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Any(p => p.Buyer != null))
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        SoldProducts = u.SoldProducts
                            .Where(sp => sp.Buyer != null)
                            .Select(sp => new
                            {
                                sp.Name,
                                sp.Price,
                                buyerFirstName = sp.Buyer.FirstName,
                                buyerLastName = sp.Buyer.LastName
                            })
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("Files/SuccessfulySoldProducts.json", jsonString);
            }
        }

        private static void GetProductsInPriceRangeJson()
        {
            var context = new ProductsContext();

            using (context)
            {
                var products = context.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        SellerName = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);

                File.WriteAllText("Files/PricesInRange.json", jsonString);
            }
        }

        private static void SetCategories()
        {
            var rnd = new Random();

            var context = new ProductsContext();

            using (context)
            {
                var categoryIds = context.Categories
                    .Select(c => c.Id)
                    .ToArray();

                var productIds = context.Products
                    .Select(p => p.Id)
                    .ToArray();

                var categoryProducts = new List<CategoryProduct>();

                foreach (var productId in productIds)
                {
                    var categoryIndex = rnd.Next(0, categoryIds.Length);
                    var categoryId = categoryIds[categoryIndex];

                    var currentCategoryProduct = new CategoryProduct()
                    {
                        ProductId = productId,
                        CategoryId = categoryId
                    };

                    categoryProducts.Add(currentCategoryProduct);
                }

                context.CategoryProducts.AddRange(categoryProducts);
                context.SaveChanges();
            }
        }

        private static void ImportProductsFromJson()
        {
            var path = "Files/products.json";
            var productsJson = File.ReadAllText(path);
            var products = JsonConvert.DeserializeObject<Product[]>(productsJson);

            var rnd = new Random();

            var context = new ProductsContext();

            using (context)
            {
                var userIds = context.Users.Select(u => u.Id).ToArray();

                foreach (var product in products)
                {
                    var index = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[index];

                    product.SellerId = sellerId;
                }

                for (int i = 0; i < products.Length / 2; i++)
                {
                    var index = rnd.Next(0, userIds.Length);

                    while (products[i].SellerId == userIds[index])
                    {
                        index = rnd.Next(0, userIds.Length);
                    }

                    products[i].BuyerId = userIds[index];
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

        private static void ImportCategoriesFromJson()
        {
            var path = "Files/categories.json";
            var categoriesJson = File.ReadAllText(path);
            var categories = JsonConvert.DeserializeObject<Category[]>(categoriesJson);

            var context = new ProductsContext();

            using (context)
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }

        private static void ImportUsersFromJson()
        {
            var path = "Files/users.json";
            var usersJson = File.ReadAllText(path);
            var users = JsonConvert.DeserializeObject<User[]>(usersJson);

            var context = new ProductsContext();

            using (context)
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}