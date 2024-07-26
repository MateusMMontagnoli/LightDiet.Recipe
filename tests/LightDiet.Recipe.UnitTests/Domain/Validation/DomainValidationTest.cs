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

        Action action =
            () => DomainValidation.NotNull(value!, "FieldName");

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validators")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? value)
    {
        Action action = 
            () => DomainValidation.NotNullOrEmpty(value, "FieldName");

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null or empty");
    }


    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    public void NotNullOrEmptyOk()
    {
        var value = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNullOrEmpty(value, "FieldName");

        action
            .Should()
            .NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validators")]
    [MemberData(nameof(GetValuesGreaterThanTheMin), parameters: 10)]
    public void MinLengthOk(string value, int minLength)
    {
        

        Action action =
            () => DomainValidation.MinLength(value, "FieldName", minLength);

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
        Action action =
            () => DomainValidation.MinLength(value, "FieldName", minLength);

        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"FieldName should not be less than {minLength} characters long");
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
}
