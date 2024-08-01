using LightDiet.Recipe.Domain.Entity;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.ListCategories;
using Moq;


namespace LightDiet.Recipe.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCategoriesOk))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListCategoriesOk()
    {
        var categoriesExampleList = _fixture.GetListOfCategories(15);
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = new ListCategoriesInput(
            page: 2,
            perPage: 15,
            search: "search-example",
            sort: "name",
            direction: SearchOrder.Asc
        );

        var outputRepositorySearch = new OutputSearch<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            Items: (IReadOnlyList<Category>)categoriesExampleList,
            Total: 70
        );

        repositoryMock.Setup(x =>
            x.Search(
                It.Is<SearchInput>(
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.Page
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
                It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCase.ListCategories(repositoryMock.Object);

        ListCategoriesOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Items.Foreach(outputItem =>
        {
            var repositoryCategory = outputRepositorySearch.Items
            .Find(x => x.Id == outputItem.Id);
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(repositoryCategory.Name);
            outputItem.Description.Should().Be(repositoryCategory.Description);
            outputItem.IsActive.Should().Be(repositoryCategory.IsActive);
            outputItem.Id.Should().Be(repositoryCategory.Id);
            outputItem.CreatedAt.Should().Be(repositoryCategory.CreatedAt);
        });

        repositoryMock.Verify(x => x.Search(
             It.Is<SearchInput>(
                    searchInput.Page == input.Page
                    && searchInput.PerPage == input.Page
                    && searchInput.Search == input.Search
                    && searchInput.OrderBy == input.Sort
                    && searchInput.Order == input.Dir
                ),
             It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
