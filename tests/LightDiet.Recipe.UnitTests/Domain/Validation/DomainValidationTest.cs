using Bogus;
using FluentAssertions;
using LightDiet.Recipe.Domain.Exceptions;
using LightDiet.Recipe.Domain.Validation;

namespace LightDiet.Recipe.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; set; } = new Faker();

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action = 
            () => DomainValidation.NotNull(value, "Value");

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void NotNullThrowWhenNull()
    {
        object? value = null;
        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.NotNull(value!, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validators")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? value)
    {
        string? fieldName = Faker.Database.Column();

        Action action = 
            () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }


    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void NotNullOrEmptyOk()
    {
        var value = Faker.Commerce.ProductName();

        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.NotNullOrEmpty(value, $"{fieldName}");

        action
            .Should()
            .NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesGreaterThanTheMin), parameters: 10)]
    public void MinLengthOk(string value, int minLength)
    {
        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.MinLength(value, $"{fieldName}", minLength);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanTheMin(int numberOfValues)
    {
        var faker = new Faker();

        for (int i = 0; i < (numberOfValues - 1); i++)
        {
            var example = faker.Commerce.ProductName();

            var minLength = example.Length - (new Random().Next(1, 5));

            yield return new object[] { example, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLessLimit))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesSmallerThanTheMin), parameters: 10)]
    public void MinLengthThrowWhenLessLimit(string value, int minLength)
    {
        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.MinLength(value, $"{fieldName}", minLength);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numberOfValues)
    {
        var faker = new Faker();

        for (int i = 0; i < (numberOfValues - 1); i++)
        {
            var example = faker.Commerce.ProductName();

            var minLength = example.Length + (new Random().Next(1, 20));

            yield return new object[] { example, minLength };
        }
    }


    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreaterThanLimit))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesGreaterThanTheMax), parameters: 10)]
    public void MaxLengthThrowWhenGreaterThanLimit(string value, int maxLength)
    {
        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.MaxLength(value, $"{fieldName}", maxLength);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal than {maxLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesGreaterThanTheMax(int numberOfValues)
    {
        var faker = new Faker();

        for (int i = 0; i < (numberOfValues - 1); i++)
        {
            var example = faker.Commerce.ProductName();

            var maxLength = example.Length - (new Random().Next(1, 5));

            yield return new object[] { example, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesLessThanTheMax), parameters: 10)]
    public void MaxLengthOk(string value, int maxLength)
    {
        string? fieldName = Faker.Database.Column();

        Action action =
            () => DomainValidation.MaxLength(value, $"{fieldName}", maxLength);

        action.Should().NotThrow();
    }

    public static IEnumerable<object[]> GetValuesLessThanTheMax(int numberOfValues)
    {
        var faker = new Faker();

        for (int i = 0; i < (numberOfValues - 1); i++)
        {
            var example = faker.Commerce.ProductName();

            var maxLength = example.Length + (new Random().Next(0, 5));

            yield return new object[] { example, maxLength };
        }
    }
}
