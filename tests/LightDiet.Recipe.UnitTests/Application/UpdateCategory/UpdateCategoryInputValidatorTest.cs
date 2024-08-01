using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;

namespace LightDiet.Recipe.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryInputValidatorTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ErrorWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryValidator - Use Cases")]
    public void ErrorWhenEmptyGuid()
    {
        var input = _fixture.GetValidInput(Guid.Empty);

        var validador = new UpdateCategoryInputValidator();

        var validateResult = validador.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }

    [Fact(DisplayName = nameof(UpdateCategoryInputOk))]
    [Trait("Application", "UpdateCategoryValidator - Use Cases")]
    public void UpdateCategoryInputOk()
    {
        var input = _fixture.GetValidInput();

        var validador = new UpdateCategoryInputValidator();

        var validateResult = validador.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}

