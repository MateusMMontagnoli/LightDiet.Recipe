using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.DeleteCategory;

public class DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
{
    private readonly DeleteCategoryApiTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategoryOk))]
    [Trait("EndToEnd/Api", "Category/Delete - Endpoints")]
    public async Task DeleteCategoryOk()
    {
        var categories = _fixture.GetExampleCategories(20);

        await _fixture.Persistence.InsertList(categories);

        var category = categories[10];

        var (response, output) = await _fixture.ApiClient.Delete<object>(
            $"/categories/{category.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);

        output.Should().BeNull();

        var persistenceCategory = await _fixture
            .Persistence
            .GetById(category.Id);

        persistenceCategory.Should().BeNull();

    }
}
