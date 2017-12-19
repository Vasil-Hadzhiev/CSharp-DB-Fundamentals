namespace P01_BillsPaymentSystem.Data.Models
{
    using System;
    using System.Text;

    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public decimal Balance { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal amount)
        {
            if (amount > this.Balance)
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Value can't be zero or negative!");
            }

            this.Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException("Value can't be zero or negative!");
            }

            this.Balance += amount;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"-- ID: {this.BankAccountId}");
            sb.AppendLine($"--- Balance: {this.Balance}");
            sb.AppendLine($"--- Bank: {this.BankName}");
            sb.AppendLine($"--- SWIFT: {this.SwiftCode}");

            return sb.ToString().Trim();
        }
    }
}