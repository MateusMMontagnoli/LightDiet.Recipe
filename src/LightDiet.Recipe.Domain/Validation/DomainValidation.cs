using LightDiet.Recipe.Domain.Exceptions;

namespace LightDiet.Recipe.Domain.Validation;

public class DomainValidation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
        {
            throw new EntityValidationException(
                    $"{fieldName} should not be null"
                );
        }
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            throw new EntityValidationException(
                    $"{fieldName} should not be null or empty"
                );
        }
    }

    public static void MinLength(string target, string fieldName, int minLength)
    {
        if (target.Length < minLength) 
        {
            throw new EntityValidationException(
                    $"{fieldName} should not be less than {minLength} characters long"
                );
        }
    }
}
