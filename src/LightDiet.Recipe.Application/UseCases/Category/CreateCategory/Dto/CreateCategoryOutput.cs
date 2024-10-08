﻿using DomainEntity = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;

public record CreateCategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateCategoryOutput(
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

    public static CreateCategoryOutput FromCategory(DomainEntity.Category category) 
        => new CreateCategoryOutput(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt
        );
}
