using LightDiet.Recipe.Domain.Exceptions;
using System.Net.Http.Headers;

namespace LightDiet.Recipe.Domain.Entity;

public class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        }

        if (Name.Length < 3)
        {
            throw new EntityValidationException($"{nameof(Name)} should be at least 3 characters long");
        }

        if (Name.Length > 50)
        {
            throw new EntityValidationException($"{nameof(Name)} should be less or equal than 50 characters long");
        }

        if (Description == null)
        {
            throw new EntityValidationException($"{nameof(Description)} should not be null");
        }

        if (Description.Length > 150)
        {
            throw new EntityValidationException($"{nameof(Description)} should be less or equal than 150 characters long");
        }
    }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
}
