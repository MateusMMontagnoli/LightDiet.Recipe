using FluentAssertions;
using LightDiet.Recipe.Application.Exceptions;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.SeedWork.SearchableRepository;
using LightDiet.Recipe.Infra.Data.EF;
using Repository = LightDiet.Recipe.Infra.Data.EF.Repositories;

namespace LightDiet.Recipe.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext(true);

        var dbCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Description.Should().NotBeNull();
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetValidCategory();
        var exampleCategoriesList = _fixture.GetValidCategoriesList(15);
        exampleCategoriesList.Add(exampleCategory);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        _ = await dbContext.SaveChangesAsync(CancellationToken.None);

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext(true);

        var categoryRepository = new Repository.CategoryRepository(newDbContext);

        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        dbCategory.Should().NotBeNull();
        dbCategory!.Id.Should().Be(exampleCategory.Id);
        dbCategory.Name.Should().Be(exampleCategory.Name);
        dbCategory.Description.Should().NotBeNull();
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(GetThrowExceptionWhenNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowExceptionWhenNotFound()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var exampleId = Guid.NewGuid();

        var exampleCategoriesList = _fixture.GetValidCategoriesList(15);

        await dbContext.AddRangeAsync(exampleCategoriesList);
        _ = await dbContext.SaveChangesAsync(CancellationToken.None);

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext(true);

        var categoryRepository = new Repository.CategoryRepository(newDbContext);

        var action = async () => await categoryRepository.Get(
            exampleId,
            CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleId}' not found");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var newCategoryValues = _fixture.GetValidCategory();

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);


        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        await categoryRepository.Update(exampleCategory, CancellationToken.None);

        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext(true);

        var dbCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Description.Should().NotBeNull();
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var exampleCategory = _fixture.GetValidCategory();
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext(true);

        var dbCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().BeNull();
    }

    [Fact(DisplayName = nameof(SearchPaginatedList))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchPaginatedList()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture.GetValidCategoriesList(15);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var paginatedResult = await categoryRepository.Search(searchInput, CancellationToken.None);

        paginatedResult.Should().NotBeNull();
        paginatedResult.Items.Should().NotBeNull();
        paginatedResult.Items.Should().HaveCount(exampleCategoriesList.Count);
        paginatedResult.CurrentPage.Should().Be(searchInput.Page);
        paginatedResult.PerPage.Should().Be(searchInput.PerPage);
        paginatedResult.Total.Should().Be(exampleCategoriesList.Count);
        foreach (Category category in paginatedResult.Items)
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
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchEmptyPaginatedListWhenPersistenceIsEmpty()
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        var paginatedResult = await categoryRepository.Search(searchInput, CancellationToken.None);

        paginatedResult.Should().NotBeNull();
        paginatedResult.Items.Should().NotBeNull();
        paginatedResult.Items.Should().HaveCount(0);
        paginatedResult.CurrentPage.Should().Be(searchInput.Page);
        paginatedResult.PerPage.Should().Be(searchInput.PerPage);
        paginatedResult.Total.Should().Be(0);
    }

    [Theory(DisplayName = nameof(SearchPaginatedListBasedOnInput))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture.GetValidCategoriesList(quantityOfCategoriesToGenerate);
        var searchInput = new SearchInput(page, quantityPerPage, "", "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var paginatedResult = await categoryRepository.Search(searchInput, CancellationToken.None);

        paginatedResult.Should().NotBeNull();
        paginatedResult.Items.Should().NotBeNull();
        paginatedResult.Items.Should().HaveCount(expectedQuantityItems);
        paginatedResult.CurrentPage.Should().Be(searchInput.Page);
        paginatedResult.PerPage.Should().Be(searchInput.PerPage);
        paginatedResult.Total.Should().Be(quantityOfCategoriesToGenerate);
        foreach (Category category in paginatedResult.Items)
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
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
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

        var categoryRepository = new Repository.CategoryRepository(dbContext);

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
        var searchInput = new SearchInput(page, quantityPerPage, search, "", SearchOrder.Asc);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var paginatedResult = await categoryRepository.Search(searchInput, CancellationToken.None);

        paginatedResult.Should().NotBeNull();
        paginatedResult.Items.Should().NotBeNull();
        paginatedResult.Items.Should().HaveCount(expectedQuantityItemsReturned);
        paginatedResult.CurrentPage.Should().Be(searchInput.Page);
        paginatedResult.PerPage.Should().Be(searchInput.PerPage);
        paginatedResult.Total.Should().Be(expectedQuantityTotalItems);
        foreach (Category category in paginatedResult.Items)
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
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("name", SearchOrder.Asc)]
    [InlineData("name", SearchOrder.Desc)]
    public async Task SearchOrderedPaginatedList(
        string orderBy,
        SearchOrder order
    )
    {
        LightDietRecipeDbContext dbContext = _fixture.CreateDbContext();

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var exampleCategoriesList = _fixture
            .GetValidCategoriesList();

        var searchInput = new SearchInput(1, 20, "", orderBy, order);

        await dbContext.AddRangeAsync(exampleCategoriesList, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        var paginatedResult = await categoryRepository.Search(searchInput, CancellationToken.None);

        var expectedOrderedList = _fixture.OrderCategoryList(
            exampleCategoriesList, orderBy, order
        );

        paginatedResult.Should().NotBeNull();
        paginatedResult.Items.Should().NotBeNull();
        paginatedResult.Items.Should().HaveCount(exampleCategoriesList.Count);
        paginatedResult.CurrentPage.Should().Be(searchInput.Page);
        paginatedResult.PerPage.Should().Be(searchInput.PerPage);
        paginatedResult.Total.Should().Be(exampleCategoriesList.Count);
        for (int i = 0; i < expectedOrderedList.Count; i++)
        {
            var expectedItem = expectedOrderedList[i];
            var outputItem = paginatedResult.Items[i];

            outputItem.Id.Should().Be(expectedItem.Id);
            outputItem.Name.Should().Be(expectedItem!.Name);
            outputItem.Description.Should().Be(expectedItem.Description);
            outputItem.IsActive.Should().Be(expectedItem.IsActive);
            outputItem.CreatedAt.Should().Be(expectedItem.CreatedAt);
        }
    }


}
