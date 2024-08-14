using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Domain.Exceptions;
using LightDiet.Recipe.Infra.Data.EF.Repositories;
using LightDiet.Recipe.Infra.Data.EF.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.CreateCategory;

namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest(CreateCategoryTestFixture fixture)
{
    private readonly CreateCategoryTestFixture _fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(
            repository,
            unitOfWork
        );

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true);

        var categoryInserted = await newDbContext.Categories.FindAsync(output.Id);

        categoryInserted.Should().NotBeNull();
        categoryInserted!.Name.Should().Be(input.Name);
        categoryInserted.Description.Should().Be(input.Description);
        categoryInserted.IsActive.Should().Be(input.IsActive);
        categoryInserted.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(
            repository,
            unitOfWork
        );

        var input = new CreateCategoryInput(_fixture.GetValidInput().Name);

        var output = await useCase.Handle(input, CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true);

        var categoryInserted = await newDbContext.Categories.FindAsync(output.Id);

        categoryInserted.Should().NotBeNull();
        categoryInserted!.Name.Should().Be(input.Name);
        categoryInserted.Description.Should().Be("");
        categoryInserted.IsActive.Should().Be(true);
        categoryInserted.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be("");
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(
            repository,
            unitOfWork
        );
        var exampleInput = _fixture.GetValidInput();

        var input = new CreateCategoryInput(exampleInput.Name, exampleInput.Description);

        var output = await useCase.Handle(input, CancellationToken.None);

        var newDbContext = _fixture.CreateDbContext(true);

        var categoryInserted = await newDbContext.Categories.FindAsync(output.Id);

        categoryInserted.Should().NotBeNull();
        categoryInserted!.Name.Should().Be(input.Name);
        categoryInserted.Description.Should().Be(input.Description);
        categoryInserted.IsActive.Should().Be(true);
        categoryInserted.CreatedAt.Should().Be(output.CreatedAt);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default);
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Integration/Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidCategories),
        parameters: 6,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
    public async void ThrowWhenCantInstantiateCategory(
        CreateCategoryInput input,
        string exceptionMessage
        )
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = new CategoryRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var useCase = new UseCase.CreateCategory(
            repository,
            unitOfWork
        );

        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);

        var newDbContext = _fixture.CreateDbContext(true);

        var quantityOfCategoriesInDatabase = await newDbContext.Categories.CountAsync();

        quantityOfCategoriesInDatabase.Should().Be(0);
    }
}
