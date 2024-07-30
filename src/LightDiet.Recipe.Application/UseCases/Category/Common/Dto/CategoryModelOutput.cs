using DomainEntity = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.Application.UseCases.Category.Common.Dto;

public record CategoryModelOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CategoryModelOutput(
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

    public static CategoryModelOutput FromCategory(DomainEntity.Category category)
        => new CategoryModelOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
}
