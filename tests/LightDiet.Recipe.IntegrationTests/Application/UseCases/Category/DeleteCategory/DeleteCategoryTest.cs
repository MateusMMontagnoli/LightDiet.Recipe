using FluentAssertions;
using LightDiet.Recipe.Application.Exceptions;
using LightDiet.Recipe.Application.UseCases.Category.DeleteCategory.Dto;
using LightDiet.Recipe.Infra.Data.EF.Repositories;
using LightDiet.Recipe.Infra.Data.EF.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.DeleteCategory;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteOk))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task DeleteOk()
    {
        var dbContext = _fixture.CreateDbContext();
        var categoryToDelete = _fixture.GetValidCategory();
        var finalNumberOfCategories = 10;
        var listOfCategoriesExample = _fixture.GetValidCategoriesList(finalNumberOfCategories);

        await dbContext.AddRangeAsync(listOfCategoriesExample);
        var trackingCategory = await dbContext.AddAsync(categoryToDelete);
        
        await dbContext.SaveChangesAsync();

        trackingCategory.State = EntityState.Detached;
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);
        
        var input = new DeleteCategoryInput(categoryToDelete.Id);
        var useCase = new UseCase.DeleteCategory(
            repository,
            unitOfWork
        );

        await useCase.Handle(input, CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true);
        var totalOfCategories = await newDbContext.Categories.CountAsync();
        var deletedCategory = await newDbContext.Categories.FindAsync(categoryToDelete.Id);

        deletedCategory.Should().BeNull();
        totalOfCategories.Should().Be(finalNumberOfCategories);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Integration/Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var dbContext = _fixture.CreateDbContext();

        var randomGuid = Guid.NewGuid();
        var listOfCategoriesExample = _fixture.GetValidCategoriesList();

        await dbContext.AddRangeAsync(listOfCategoriesExample);

        await dbContext.SaveChangesAsync();

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var input = new DeleteCategoryInput(randomGuid);
        var useCase = new UseCase.DeleteCategory(
            repository,
            unitOfWork
        );

        var task = async() 
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{randomGuid}' not found");
    }
}
