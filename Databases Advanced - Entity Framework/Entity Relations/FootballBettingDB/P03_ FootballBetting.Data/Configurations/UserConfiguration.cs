namespace P03_FootballBetting.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P03_FootballBetting.Data.Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);

            builder.Property(u => u.Email)
                .IsUnicode(false);

            builder.Property(u => u.Username)
                .IsRequired()
                .IsUnicode();

            builder.Property(u => u.Password)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}