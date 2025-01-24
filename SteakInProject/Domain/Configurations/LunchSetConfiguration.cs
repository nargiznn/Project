using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
	public class LunchSetConfiguration: IEntityTypeConfiguration<LunchSet>
    {
        public void Configure(EntityTypeBuilder<LunchSet> builder)
        {
            builder.Property(m => m.Title)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(m => m.Desc)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}

