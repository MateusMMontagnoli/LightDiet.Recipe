using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.DeleteCategory.Dto;

namespace LightDiet.Recipe.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryInputValidatorTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryInputValidatorTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ValidateOk))]
    [Trait("Application", "DeleteValidation - Use Cases")]
    public void ValidateOk()
    {
        var validInput = new DeleteCategoryInput(Guid.NewGuid());
        var validator = new DeleteCategoryInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ValidateOk))]
    [Trait("Application", "DeleteValidation - Use Cases")]
    public void InvalidWhenEmptyGuid()
    {
        var validInput = new DeleteCategoryInput(Guid.Empty);
        var validator = new DeleteCategoryInputValidator();

        var validationResult = validator.Validate(validInput);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
