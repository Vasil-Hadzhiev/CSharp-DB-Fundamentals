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
            }
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
                context.AddRange(users);
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