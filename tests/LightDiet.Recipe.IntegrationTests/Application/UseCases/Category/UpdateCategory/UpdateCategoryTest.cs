using FluentAssertions;
using LightDiet.Recipe.Application.Exceptions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;
using LightDiet.Recipe.Domain.Exceptions;
using LightDiet.Recipe.Infra.Data.EF.Repositories;
using LightDiet.Recipe.Infra.Data.EF.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Entities = LightDiet.Recipe.Domain.Entity;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.UpdateCategory;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategoryOk))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryOk(Entities.Category exampleCategory, UpdateCategoryInput input)
    {
        var dbContext = _fixture.CreateDbContext();
        var listOfCategories = _fixture.GetValidCategoriesList();
        await dbContext.AddRangeAsync(listOfCategories);
        var categoryTrackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();

        categoryTrackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);

        var newDbContext = _fixture.CreateDbContext(true);

        var updatedCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be(output.Name);
        updatedCategory.IsActive.Should().Be((bool)output.IsActive!);
        updatedCategory.Description.Should().Be(output.Description);
        updatedCategory.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryWithoutIsActive(Entities.Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);

        var dbContext = _fixture.CreateDbContext();
        var listOfCategories = _fixture.GetValidCategoriesList();
        await dbContext.AddRangeAsync(listOfCategories);
        var categoryTrackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();

        categoryTrackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(exampleCategory.IsActive!);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);

        var newDbContext = _fixture.CreateDbContext(true);

        var updatedCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be(output.Name);
        updatedCategory.IsActive.Should().Be(exampleCategory.IsActive!);
        updatedCategory.Description.Should().Be(output.Description);
        updatedCategory.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 5,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryOnlyName(Entities.Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);

        var dbContext = _fixture.CreateDbContext();
        var listOfCategories = _fixture.GetValidCategoriesList();
        await dbContext.AddRangeAsync(listOfCategories);
        var categoryTrackingInfo = await dbContext.AddAsync(exampleCategory);
        await dbContext.SaveChangesAsync();

        categoryTrackingInfo.State = EntityState.Detached;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(exampleCategory.IsActive!);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);

        var newDbContext = _fixture.CreateDbContext(true);

        var updatedCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        updatedCategory.Should().NotBeNull();
        updatedCategory!.Name.Should().Be(output.Name);
        updatedCategory.IsActive.Should().Be(exampleCategory.IsActive!);
        updatedCategory.Description.Should().Be(exampleCategory.Description);
        updatedCategory.CreatedAt.Should().Be(output.CreatedAt);
    }

    [Fact(DisplayName = nameof(ThrowErrorWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowErrorWhenCategoryNotFound()
    {
        var input = _fixture.GetValidInput();

        var dbContext = _fixture.CreateDbContext();
        var listOfCategories = _fixture.GetValidCategoriesList();
        await dbContext.AddRangeAsync(listOfCategories);
        await dbContext.SaveChangesAsync();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{input.Id}' not found");
    }

    [Theory(DisplayName = nameof(ThrowErrorWhenCantInstantiateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidCategories),
        parameters: 6,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task ThrowErrorWhenCantInstantiateCategory(
        UpdateCategoryInput input,
        string expectedExceptionMessage    
    )
    {
        var dbContext = _fixture.CreateDbContext();
        
        var listOfCategories = _fixture.GetValidCategoriesList();
        await dbContext.AddRangeAsync(listOfCategories);
        await dbContext.SaveChangesAsync();

        input.Id = listOfCategories[0].Id;

        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.UpdateCategory(repository, unitOfWork);

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);

    }
}
