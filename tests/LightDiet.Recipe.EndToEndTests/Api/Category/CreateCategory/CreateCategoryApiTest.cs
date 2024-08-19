using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using Entities = LightDiet.Recipe.Domain.Entity;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.CreateCategory;

[Collection(nameof(CreateCategoryApiTestFixture))]
public class CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
{
    private readonly CreateCategoryApiTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategoryOk))]
    [Trait("EndToEnd/Api", "Category - Endpoints")]
    public async Task CreateCategoryOk()
    {
        var input = _fixture.GetValidInput();

        CategoryModelOutput output = await _fixture
            .ApiClient
            .Post<CategoryModelOutput>(
                "/categories",
                input
            );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);

        Entities.Category dbCategory = await _fixture.Persistence
            .GetById(output.Id);

        dbCategory.Should().NotBeNull();
        dbCategory.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(input.IsActive);
        dbCategory.Id.Should().NotBeEmpty();
        dbCategory.CreatedAt.Should().NotBeSameDateAs(default);

    }
}
