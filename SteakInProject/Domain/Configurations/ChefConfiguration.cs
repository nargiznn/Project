using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
	public class ChefConfiguration: IEntityTypeConfiguration<Chef>
	{
        public void Configure(EntityTypeBuilder<Chef> builder)
        {
            builder.Property(m => m.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(m => m.Surname)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}

