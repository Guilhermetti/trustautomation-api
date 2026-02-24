using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustAutomation.Domain.Models;

namespace TrustAutomation.Infrastructure.Configurations
{
    public sealed class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.CreatedAtUtc);
            builder.HasIndex(x => x.Email);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Company).HasMaxLength(100);
            builder.Property(x => x.Whatsapp).HasMaxLength(30);
            builder.Property(x => x.NeedType).HasMaxLength(40);
            builder.Property(x => x.Deadline).HasMaxLength(40);
            builder.Property(x => x.Idea).HasMaxLength(2000).IsRequired();
            builder.Property(x => x.SourceUrl).HasMaxLength(500);
            builder.Property(x => x.Ip).HasMaxLength(60);
        }
    }
}
