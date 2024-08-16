using FluentAssertions;
using LightDiet.Recipe.Infra.Data.EF;
using LightDiet.Recipe.Infra.Data.EF.Repositories;
using LightDiet.Recipe.Application.UseCases.Category.ListCategories.Dto;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.ListCategories;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using LightDiet.Recipe.Domain.SeedWork;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact(DisplayName = nameof(SearchPaginatedList))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchPaginatedList()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture.GetValidCategoriesList(15);
        var input = new ListCategoriesInput(1, 20);
        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var useCase = new UseCase.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);
        
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        foreach (CategoryModelOutput category in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                inputedCategory => inputedCategory.Id == category.Id
            );

            exampleItem.Should().NotBeNull();
            category.Name.Should().Be(exampleItem!.Name);
            category.Description.Should().NotBeNull();
            category.IsActive.Should().Be(exampleItem.IsActive);
            category.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Fact(DisplayName = nameof(SearchEmptyPaginatedListWhenPersistenceIsEmpty))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    public async Task SearchEmptyPaginatedListWhenPersistenceIsEmpty()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);
        var input = new ListCategoriesInput(1, 20);

        var useCase = new UseCase.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(0);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
    }

    [Theory(DisplayName = nameof(SearchPaginatedListBasedOnInput))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchPaginatedListBasedOnInput(
        int quantityOfCategoriesToGenerate,
        int page,
        int quantityPerPage,
        int expectedQuantityItems
        )
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture.GetValidCategoriesList(quantityOfCategoriesToGenerate);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var useCase = new UseCase.ListCategories(categoryRepository);

        var input = new ListCategoriesInput(page, quantityPerPage, "", "", SearchOrder.Asc);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(expectedQuantityItems);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityOfCategoriesToGenerate);
        foreach (CategoryModelOutput category in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                inputedCategory => inputedCategory.Id == category.Id
            );

            exampleItem.Should().NotBeNull();
            category.Name.Should().Be(exampleItem!.Name);
            category.Description.Should().NotBeNull();
            category.IsActive.Should().Be(exampleItem.IsActive);
            category.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchPaginatedListBasedOnInputWithText))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData("Vegan", 1, 5, 1, 1)]
    [InlineData("Gluten-Free", 1, 5, 2, 2)]
    [InlineData("Gluten-Free", 2, 5, 0, 2)]
    [InlineData("Homemade", 1, 5, 4, 4)]
    [InlineData("Homemade", 1, 2, 2, 4)]
    [InlineData("Homemade", 2, 3, 1, 4)]
    [InlineData("Appetizers", 1, 5, 0, 0)]
    [InlineData("Dishes", 1, 5, 2, 2)]
    public async Task SearchPaginatedListBasedOnInputWithText(
        string search,
        int page,
        int quantityPerPage,
        int expectedQuantityItemsReturned,
        int expectedQuantityTotalItems
        )
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);

        var listOfNames = new List<string>()
        {
            "Vegan",
            "Healthy",
            "Vegetarian",
            "High Protein",
            "Gluten-Free",
            "Homemade",
            "Healthy Homemade",
            "Gluten-Free Homemade",
            "Homemade with high protein",
            "Main Dishes",
            "Side Dishes"
        };

        var exampleCategoriesList = _fixture
            .GetValidCategoriesListWithNames(listOfNames);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var input = new ListCategoriesInput(page, quantityPerPage, search, "", SearchOrder.Asc);

        var useCase = new UseCase.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        foreach (CategoryModelOutput category in output.Items)
        {
            var exampleItem = exampleCategoriesList.Find(
                inputedCategory => inputedCategory.Id == category.Id
            );

            exampleItem.Should().NotBeNull();
            category.Name.Should().Be(exampleItem!.Name);
            category.Description.Should().NotBeNull();
            category.IsActive.Should().Be(exampleItem.IsActive);
            category.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }

    [Theory(DisplayName = nameof(SearchPaginatedListBasedOnInputWithText))]
    [Trait("Integration/Application", "ListCategories - Use Cases")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    [InlineData("id", SearchOrder.Asc)]
    [InlineData("id", SearchOrder.Desc)]
    [InlineData("createdAt", SearchOrder.Asc)]
    [InlineData("createdAt", SearchOrder.Desc)]
    [InlineData("", SearchOrder.Asc)]
    public async Task SearchOrderedPaginatedList(
       string orderBy,
       SearchOrder order
   )
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture
            .GetValidCategoriesList();

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var input = new ListCategoriesInput(1, 20, "", orderBy, order);

        var useCase = new UseCase.ListCategories(categoryRepository);

        var output = await useCase.Handle(input, CancellationToken.None);

        var expectedOrderedList = _fixture.OrderCategoryList(
            exampleCategoriesList, 
            input.Sort,
            input.Dir
        );

        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var outputItem = output.Items[i];

            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }

}
