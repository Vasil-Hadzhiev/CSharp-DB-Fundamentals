namespace P01_BillsPaymentSystem.Data.DatabaseInitializer.Generators
{
    using P01_BillsPaymentSystem.Data.Models;
    using System;

    public class CreditCardGenerator
    {
        private static Random rnd = new Random();

        private static decimal Limit()
        {
            var balance = rnd.NextDouble() * 1000;

            return Convert.ToDecimal(balance);
        }

        private static decimal MoneyOwed()
        {
            double balance = rnd.NextDouble() * 5;

            return Convert.ToDecimal(balance);
        }

        private static CreditCard NewCreditCard()
        {
            CreditCard creditCard = new CreditCard()
            {
                Limit = Limit(),
                MoneyOwed = MoneyOwed(),
                ExpirationDate = DateGenerator.GenerateDate()
            };

            return creditCard;
        }

        public static void InitialCreditCardSeed(BillsPaymentSystemContext db, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var creditCard = NewCreditCard();

                db.CreditCards.Add(creditCard);
                db.SaveChanges();
            }
        }
    }
}
