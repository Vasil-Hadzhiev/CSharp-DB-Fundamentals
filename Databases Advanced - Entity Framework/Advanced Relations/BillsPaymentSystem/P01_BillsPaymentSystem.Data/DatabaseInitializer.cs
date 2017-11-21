namespace P01_BillsPaymentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data.Models;
    using P01_BillsPaymentSystem.Data.Models.Enums;
    using System;
    using System.Linq;

    public class DatabaseInitializer
    {
        public static void ResetDatabase()
        {
            var db = new BillsPaymentSystemContext();

            using (db)
            {
                db.Database.EnsureDeleted();

                db.Database.Migrate();

                SeedDatabase(db);
            }
        }

        private static void SeedDatabase(BillsPaymentSystemContext db)
        {
            CreateUsers(db);
            CreateCreditCards(db);
            CreateBankAccounts(db);
            CreatePaymentMethods(db);
        }

        private static void CreateUsers(BillsPaymentSystemContext db)
        {

            var users = new User[]
            {
                new User
                {
                    FirstName = "Barry",
                    LastName = "Allen",
                    Email = "TheFlash@gmail.com",
                    Password = "Speedster"
                },

                new User
                {
                    FirstName = "Oliver",
                    LastName = "Queen",
                    Email = "Arrow@gmail.com",
                    Password = "GreenHood"
                },

                new User
                {
                    FirstName = "Michael",
                    LastName = "Scofield",
                    Email = "EscapeArtist@gmail.com",
                    Password = "ILoveSara"
                },

                new User
                {
                    FirstName = "Lincoln",
                    LastName = "Burrows",
                    Email = "BigBro@gmail.com",
                    Password = "Burrows420"
                }
            };

            db.Users.AddRange(users);
            db.SaveChanges();

        }

        private static void CreateCreditCards(BillsPaymentSystemContext db)
        {

            var creditCards = new CreditCard[]
            {
                new CreditCard
                {
                    ExpirationDate = DateTime.ParseExact("01.01.2022", "dd.MM.yyyy", null),
                    Limit = 5000m,
                    MoneyOwed = 3500m
                },

                new CreditCard
                {
                    ExpirationDate = DateTime.ParseExact("01.01.2022", "dd.MM.yyyy", null),
                    Limit = 5000m,
                    MoneyOwed = 3500m
                },

                new CreditCard
                {
                    ExpirationDate = DateTime.ParseExact("01.01.2025", "dd.MM.yyyy", null),
                    Limit = 10000m,
                    MoneyOwed = 1000m
                },

                new CreditCard
                {
                    ExpirationDate = DateTime.ParseExact("01.01.2025", "dd.MM.yyyy", null),
                    Limit = 500_000m,
                    MoneyOwed = 0m
                },

                new CreditCard
                {
                    ExpirationDate = DateTime.ParseExact("01.01.2020", "dd.MM.yyyy", null),
                    Limit = 100_000m,
                    MoneyOwed = 1000m
                }
            };

            db.CreditCards.AddRange(creditCards);
            db.SaveChanges();

        }

        private static void CreateBankAccounts(BillsPaymentSystemContext db)
        {

            var bankAccounts = new BankAccount[]
            {
                new BankAccount
                {
                    Balance = 1_000_000m,
                    BankName = "Star City Bank",
                    SwiftCode = "A0R1R2R3O4W5"
                },

                new BankAccount
                {
                    Balance = 10000m,
                    BankName = "Big Bank",
                    SwiftCode = "SWIFT420"
                },

                new BankAccount
                {
                    Balance = 20000m,
                    BankName = "Central City Bank",
                    SwiftCode = "FLASH420"
                },

                new BankAccount
                {
                    Balance = 50000m,
                    BankName = "Big Bank",
                    SwiftCode = "FK5973AS"
                }
            };

            db.BankAccounts.AddRange(bankAccounts);
            db.SaveChanges();

        }

        private static void CreatePaymentMethods(BillsPaymentSystemContext db)
        {

            var users = db.Users.ToList();
            var bankAccounts = db.BankAccounts.ToList();
            var creditCards = db.CreditCards.ToList();

            var paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod
                {
                    User = users[0],
                    CreditCard = creditCards[2],
                    Type = PaymentMethodType.CreditCard
                },

                new PaymentMethod
                {
                    User = users[0],
                    BankAccount = bankAccounts[2],
                    Type = PaymentMethodType.BankAccount
                },

                new PaymentMethod
                {
                    User = users[1],
                    CreditCard = creditCards[3],
                    Type = PaymentMethodType.CreditCard
                },

                new PaymentMethod
                {
                    User = users[1],
                    CreditCard = creditCards[4],
                    Type = PaymentMethodType.CreditCard
                },

                new PaymentMethod
                {
                    User = users[1],
                    BankAccount = bankAccounts[0],
                    Type = PaymentMethodType.BankAccount
                },

                new PaymentMethod
                {
                    User = users[2],
                    CreditCard = creditCards[1],
                    Type = PaymentMethodType.CreditCard
                },

                new PaymentMethod
                {
                    User = users[0],
                    BankAccount = bankAccounts[1],
                    Type = PaymentMethodType.BankAccount
                },

                new PaymentMethod
                {
                    User = users[3],
                    CreditCard = creditCards[0],
                    Type = PaymentMethodType.CreditCard
                },

                new PaymentMethod
                {
                    User = users[0],
                    BankAccount = bankAccounts[3],
                    Type = PaymentMethodType.BankAccount
                }
            };

            db.PaymentMethods.AddRange(paymentMethods);
            db.SaveChanges();
        }
    }
}