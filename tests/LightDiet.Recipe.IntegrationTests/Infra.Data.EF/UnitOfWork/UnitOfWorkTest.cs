using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkInfra = LightDiet.Recipe.Infra.Data.EF.UnitOfWork;

namespace LightDiet.Recipe.IntegrationTests.Infra.Data.EF.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Commit()
    {
        var dbContext = _fixture.CreateDbContext();

        var exampleCategoriesList = _fixture.GetValidCategoriesList();

        await dbContext.AddRangeAsync(exampleCategoriesList);

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        await unitOfWork.Commit(CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true);

        var quantityOfCategories = await newDbContext.Categories.CountAsync();

        quantityOfCategories.Should().Be(exampleCategoriesList.Count);
    }

    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
    public async Task Rollback()
    {
        var dbContext = _fixture.CreateDbContext();

        var exampleCategoriesList = _fixture.GetValidCategoriesList();

        await dbContext.AddRangeAsync(exampleCategoriesList);

        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        var task = async () => await unitOfWork.Rollback(CancellationToken.None);

        await task.Should().NotThrowAsync();

        var newDbContext = _fixture.CreateDbContext(true);

        var quantityOfCategories = await newDbContext.Categories.CountAsync();

        quantityOfCategories.Should().Be(0);

    }
}
