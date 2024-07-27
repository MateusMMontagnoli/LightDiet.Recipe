using LightDiet.Recipe.Domain.Exceptions;
using LightDiet.Recipe.Domain.SeedWork;
using LightDiet.Recipe.Domain.Validation;

namespace LightDiet.Recipe.Domain.Entity;

public class Category : AggregateRoot
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true) : base ()
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;

        Validate();
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

    public void Update(string name, string? description = null)
    {
        this.Name = name;
        this.Description = description ?? Description;
        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, nameof(Name), 3);
        DomainValidation.MaxLength(Name, nameof(Name), 50);
        DomainValidation.NotNull(Description, nameof(Description));
        DomainValidation.MaxLength(Description, nameof(Description), 150);
    }
}
