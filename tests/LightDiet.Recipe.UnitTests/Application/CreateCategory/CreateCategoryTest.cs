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

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
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
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidCategories),
        parameters: 24,
        MemberType = typeof(CreateCategoryTestDataGenerator)
    )]
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
