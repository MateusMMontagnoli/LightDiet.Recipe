using FluentAssertions;
using LightDiet.Recipe.Application.Interfaces;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Exceptions;
using LightDiet.Recipe.Domain.Repository.Interfaces;
using Moq;
using System.Security.Cryptography;
using UseCases = LightDiet.Recipe.Application.UseCases.Category.CreateCategory;

namespace LightDiet.Recipe.UnitTests.Application.CreateCategory;

[Collection(nameof(CreateCategoryTesteFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTesteFixture _fixture;

    public CreateCategoryTest(CreateCategoryTesteFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = _fixture.GetValidInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository => 
            repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ), 
            Times.Once
        );

        unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(GetInvalidCategories))]
    public async void ThrowWhenCantInstantiateCategory(
        CreateCategoryInput input,
        string exceptionMessage
        )
    {
        var useCase = new UseCases.CreateCategory(
                _fixture.GetRepositoryMock().Object,
                _fixture.GetUnitOfWorkMock().Object
            );

        Func<Task> task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);

    }

    public static IEnumerable<object[]> GetInvalidCategories()
    {
        var fixture = new CreateCategoryTesteFixture();

        var invalidInputList = new List<object[]>();

        var nameMinLengthException = "Name should be at least 3 characters long";

        var nameMaxLengthException = "Name should be less or equal than 50 characters long";

        var nameEmptyException = "Name should not be empty or null";

        var nameNullException = "Name should not be empty or null";

        var descriptionMaxLengthException = "Description should be less or equal than 150 characters long";

        var descriptionNullException = "Description should not be null";

        var invalidInputByNameMinLength = fixture.GetValidInput();

        invalidInputByNameMinLength.Name = invalidInputByNameMinLength.Name.Substring(0, 2);

        invalidInputList.Add(new object[]
        {
                invalidInputByNameMinLength,
                nameMinLengthException
        });

        var invalidInputByNameMaxLength = fixture.GetValidInput();

        var invalidName = string.Empty;

        while (invalidName.Length <= 50)
        {
            var name = fixture.Faker.Commerce.ProductName();

            invalidName += $" {name}";
        }

        invalidInputByNameMaxLength.Name = invalidName;

        invalidInputList.Add(new object[]
        {
                invalidInputByNameMaxLength,
                nameMaxLengthException
        });

        var invalidInputByNameEmpty = fixture.GetValidInput();

        invalidInputByNameEmpty.Name = string.Empty;

        invalidInputList.Add(new object[]
        {
                invalidInputByNameEmpty,
                nameEmptyException
        });

        var invalidInputByNameNullity = fixture.GetValidInput();

        invalidInputByNameNullity.Name = null!;

        invalidInputList.Add(new object[]
        {
                invalidInputByNameNullity,
                nameNullException
        });

        var invalidInputByDescriptionMaxLength = fixture.GetValidInput();

        var invalidDescription = string.Empty;

        while (invalidDescription.Length <= 150)
        {
            var description = fixture.Faker.Commerce.ProductDescription();

            invalidDescription += $" {description}";
        }

        invalidInputByDescriptionMaxLength.Description = invalidDescription;

        invalidInputList.Add(new object[]
        {
            invalidInputByDescriptionMaxLength,
            descriptionMaxLengthException
        });

        var invalidInputByDescriptionNullity = fixture.GetValidInput();

        invalidInputByDescriptionNullity.Description = null!;

        invalidInputList.Add(new object[]
        {
            invalidInputByDescriptionNullity,
            descriptionNullException
        });


        return invalidInputList;
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyName()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = new CreateCategoryInput(_fixture.GetValidCategoryName());

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(string.Empty);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Fact(DisplayName = nameof(CreateCategoryOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryOnlyNameAndDescription()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var input = new CreateCategoryInput(
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription()
            );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(repository =>
            repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(true);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }
}
