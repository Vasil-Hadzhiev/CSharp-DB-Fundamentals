namespace BusTicketSystem.Data.Configurations
{
    using BusTicketSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasOne(ba => ba.Customer)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(ba => ba.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}