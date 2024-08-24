using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LightDiet.Recipe.EndToEndTests.Api.Category.GetCategory;

[Collection(nameof(GetCategoryApiTestFixture))]
public class GetCategoryApiTest(GetCategoryApiTestFixture fixture)
{
    private readonly GetCategoryApiTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(GetCategoryOk))]
    [Trait("EndToEnd/Api", "Category/Get - Endpoints")]
    public async Task GetCategoryOk()
    {
        var categories = _fixture.GetExampleCategories(20);

        await _fixture.Persistence.InsertList(categories);

        var category = categories[10];

        var (response, output) = await _fixture.ApiClient.Get<CategoryModelOutput>(
            $"/categories/{category.Id}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

        output.Should().NotBeNull();
        output!.Id.Should().Be(category.Id);
        output.Name.Should().Be(category.Name);
        output.Description.Should().Be(category.Description);
        output.IsActive.Should().Be(category.IsActive);
        output.CreatedAt.Should().Be(category.CreatedAt);
    }

    [Fact(DisplayName = nameof(ThrowErrorWhenCategoryNotFound))]
    [Trait("EndToEnd/Api", "Category/Get - Endpoints")]
    public async Task ThrowErrorWhenCategoryNotFound()
    {
        var categories = _fixture.GetExampleCategories(20);

        await _fixture.Persistence.InsertList(categories);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Get<ProblemDetails>(
          $"/categories/{randomGuid}"
        );

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

        output.Should().NotBeNull();
        output!.Status.Should().Be((int)StatusCodes.Status404NotFound);
        output.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"Category '{randomGuid}' not found");
        output.Type.Should().Be("NotFound");
    }
}
