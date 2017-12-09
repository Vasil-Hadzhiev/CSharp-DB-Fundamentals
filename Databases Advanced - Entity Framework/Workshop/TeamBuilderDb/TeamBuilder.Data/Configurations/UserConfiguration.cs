namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasAlternateKey(u => u.Username);

            builder.Property(u => u.Username)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(u => u.FirstName)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(u => u.LastName)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(u => u.Password)
                .IsRequired(false)
                .IsUnicode(false);

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);         
        }
    }
}