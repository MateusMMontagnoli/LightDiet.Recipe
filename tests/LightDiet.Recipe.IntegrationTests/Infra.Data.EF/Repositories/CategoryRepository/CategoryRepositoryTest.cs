﻿using FluentAssertions;
using LightDiet.Recipe.Application.Exceptions;
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

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext();

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

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext();

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

        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext();

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
        
        LightDietRecipeDbContext newDbContext = _fixture.CreateDbContext();

        var dbCategory = await newDbContext.Categories.FindAsync(exampleCategory.Id);

        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(exampleCategory.Name);
        dbCategory.Id.Should().Be(exampleCategory.Id);
        dbCategory.Description.Should().NotBeNull();
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
        dbCategory.CreatedAt.Should().Be(exampleCategory.CreatedAt);
    }
}
