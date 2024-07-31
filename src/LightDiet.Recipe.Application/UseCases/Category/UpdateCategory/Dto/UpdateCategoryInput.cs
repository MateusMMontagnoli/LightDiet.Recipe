using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;

public class UpdateCategoryInput : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsActive { get; set; }

    public UpdateCategoryInput(Guid id, string name, string description, bool isActive)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }
}
