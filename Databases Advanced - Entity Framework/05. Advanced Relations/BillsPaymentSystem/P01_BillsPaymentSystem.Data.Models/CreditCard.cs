namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.Text;

    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Limit { get; set; }
        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal amount)
        {
            if (amount > this.LimitLeft)
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Value can't be zero or negative!");
            }

            this.MoneyOwed += amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Value can't be zero or negative!");
            }

            this.MoneyOwed -= amount;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"-- ID: {this.CreditCardId}");
            sb.AppendLine($"--- Limit: {this.Limit:f2}");
            sb.AppendLine($"--- Money Owed: {this.MoneyOwed:f2}");
            sb.AppendLine($"--- Expiration Date: {this.ExpirationDate}");

            return sb.ToString().Trim();
        }
    }
}