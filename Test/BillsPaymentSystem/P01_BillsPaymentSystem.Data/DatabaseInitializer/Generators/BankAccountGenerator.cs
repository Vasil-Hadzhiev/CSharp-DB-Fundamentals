namespace P01_BillsPaymentSystem.Data.DatabaseInitializer.Generators
{
    using P01_BillsPaymentSystem.Data.Models;
    using System;

    public class BankAccountGenerator
    {
        private static Random rnd = new Random();

        private static string[] bankNames =
        {
            "DSK Bank",
            "Fibank",
            "UniCredit Bulbank",
            "Unionbank",
            "BNB",
            "Raiffeisenbank",
            "Corporate Commercial Bank"
        };

        private static string[] swiftCodes =
        {
            "QWERTY123",
            "ASDFG5678",
            "BATMAN123",
            "SUPERMAN3",
            "IRONMAN55",
            "THEFLASH1",
            "ARROW5555",
            "BARRYALEN",
            "WUTFACE69",
            "WUBADUB77"
        };

        private static decimal NewBalance()
        {
            var balance = rnd.NextDouble() * 1600;

            return Convert.ToDecimal(balance);
        }

        public static void InitialBankAccountSeed(BillsPaymentSystemContext db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var account = NewAccount();

                db.BankAccounts.Add(account);
                db.SaveChanges();
            }
        }

        private static BankAccount NewAccount()
        {
            BankAccount account = new BankAccount()
            {
                Balance = NewBalance(),
                BankName = GetRandomBankName(),
                SwiftCode = GetRandomSwiftCode()
            };

            return account;
        }

        private static string GetRandomSwiftCode()
        {
            return swiftCodes[rnd.Next(0, swiftCodes.Length)];
        }

        private static string GetRandomBankName()
        {
            return bankNames[rnd.Next(0, bankNames.Length)];
        }
    }
}