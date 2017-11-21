namespace P01_BillsPaymentSystem
{
    using Microsoft.EntityFrameworkCore;
    using P01_BillsPaymentSystem.Data;
    using P01_BillsPaymentSystem.Data.Models.Enums;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            var db = new BillsPaymentSystemContext();

           var userId = int.Parse(Console.ReadLine());

            using (db)
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();
                DatabaseInitializer.ResetDatabase();
                SelectUserWithId(db, userId);
            }
        }

        private static void SelectUserWithId(BillsPaymentSystemContext db, int userId)
        {
            var user = db.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    Name = $"{u.FirstName} {u.LastName}",
                    CreditCards = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentMethodType.CreditCard)
                        .Select(pm => pm.CreditCard)
                        .ToList(),
                    BankAccounts = u.PaymentMethods
                        .Where(pm => pm.Type == PaymentMethodType.BankAccount)
                        .Select(pm => pm.BankAccount)
                        .ToList()
                })
                .FirstOrDefault();

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                return;
            }

            var bankAccounts = user.BankAccounts
                .OrderBy(ba => ba.BankAccountId);

            var creditCards = user.CreditCards
                .OrderBy(cc => cc.CreditCardId);

            Console.WriteLine($"User: {user.Name}");

            if (bankAccounts.Any())
            {
                Console.WriteLine("Bank Accounts:");

                foreach (var bankAccount in bankAccounts)
                {
                    Console.WriteLine(bankAccount);
                }
            }

            if (creditCards.Any())
            {
                Console.WriteLine("Credit Cards:");

                foreach (var creditCard in creditCards)
                {
                    Console.WriteLine(creditCard);
                }
            }
        }
    }
}