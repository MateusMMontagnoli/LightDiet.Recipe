using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using MediatR;
using System.Net;

namespace LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;

public record CreateCategoryInput : IRequest<CategoryModelOutput>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public CreateCategoryInput(
        string name,
        string? description = null,
        bool isActive = true)
    {
        Name = name;
        Description = description ?? string.Empty;
        IsActive = isActive;
    }
}
