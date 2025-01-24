using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
	public class SpecialCategoryConfiguration: IEntityTypeConfiguration<SpecialCategory>
    {
        public void Configure(EntityTypeBuilder<SpecialCategory> builder)
        {
            builder.Property(m => m.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}

