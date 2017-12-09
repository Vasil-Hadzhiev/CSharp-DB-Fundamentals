namespace TeamBuilder.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TeamBuilder.Models;

    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasAlternateKey(t => t.Name);

            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(t => t.Acronym)
                .IsRequired();

            builder.HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTeams)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}