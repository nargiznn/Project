using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
	public class FaqConfiguration : IEntityTypeConfiguration<Faq>
    {
        public void Configure(EntityTypeBuilder<Faq> builder)
        {
            builder.Property(m => m.Question)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(m => m.Answer)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}

