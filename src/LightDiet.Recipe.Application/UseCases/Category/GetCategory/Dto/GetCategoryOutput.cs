using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using DomainEntity = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;

public record GetCategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public GetCategoryOutput(
        Guid id,
        string name,
        string description,
        bool isActive,
        DateTime createdAt
    )
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        Id = id;
        CreatedAt = createdAt;
    }

    public static GetCategoryOutput FromCategory(DomainEntity.Category category)
        => new GetCategoryOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
}
