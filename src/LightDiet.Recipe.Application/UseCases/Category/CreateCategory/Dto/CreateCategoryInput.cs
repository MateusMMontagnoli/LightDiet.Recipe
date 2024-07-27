namespace LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;

public record CreateCategoryInput
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
