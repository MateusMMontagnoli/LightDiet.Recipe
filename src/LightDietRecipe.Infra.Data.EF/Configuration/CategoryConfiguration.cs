using LightDiet.Recipe.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LightDietRecipe.Infra.Data.EF.Configuration;

internal class CategoryConfiguration
    : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);
        builder.Property(category => category.Name)
            .HasMaxLength(50);
        builder.Property(category => category.Description)
            .HasMaxLength(150);
    }
}
