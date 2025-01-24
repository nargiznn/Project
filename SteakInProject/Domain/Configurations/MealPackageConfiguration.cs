using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
	public class MealPackageConfiguration: IEntityTypeConfiguration<MealPackage>
    {
        public void Configure(EntityTypeBuilder<MealPackage> builder)
        {
            builder.Property(m => m.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Desc)
               .IsRequired();

            builder.Property(m => m.NumberOfPeople)
                .IsRequired();

            builder.Property(m => m.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasCheckConstraint("CHK_MealPackage_NumberOfPeople", "[NumberOfPeople] <= 20");
        }
    }
}

