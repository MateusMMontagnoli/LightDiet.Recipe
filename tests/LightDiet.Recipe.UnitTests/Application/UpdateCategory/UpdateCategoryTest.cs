using FluentAssertions;
using LightDiet.Recipe.Application.Exceptions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;
using LightDiet.Recipe.Domain.Entity;
using LightDiet.Recipe.Domain.Exceptions;
using Moq;
using UseCase = LightDiet.Recipe.Application.UseCases.Category.UpdateCategory;


namespace LightDiet.Recipe.UnitTests.Application.UpdateCategory;

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
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task UpdateCategoryOk(Category exampleCategory, UpdateCategoryInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x =>
            x.Get(exampleCategory.Id, 
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be((bool)input.IsActive!);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);

        repositoryMock.Verify(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        repositoryMock.Verify(x =>
            x.Update(
            exampleCategory,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()    
        ), Times.Once);
    }

    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();

        repositoryMock.Setup(x =>x.Get(
            input.Id,
            It.IsAny<CancellationToken>()
        )).ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );


        var task = async ()
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x =>
            x.Get(input.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

    }

    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
      nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
      parameters: 10,
      MemberType = typeof(UpdateCategoryTestDataGenerator)
  )]
    public async Task UpdateCategoryWithoutProvidingIsActive(Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var input = new UpdateCategoryInput(
            exampleInput.Id, 
            exampleInput.Name, 
            exampleInput.Description
        );

        repositoryMock.Setup(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);

        repositoryMock.Verify(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        repositoryMock.Verify(x =>
            x.Update(
            exampleCategory,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
      nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
      parameters: 10,
      MemberType = typeof(UpdateCategoryTestDataGenerator)
  )]
    public async Task UpdateCategoryOnlyName(Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var input = new UpdateCategoryInput(
            exampleInput.Id,
            exampleInput.Name
        );

        repositoryMock.Setup(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);

        repositoryMock.Verify(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        repositoryMock.Verify(x =>
            x.Update(
            exampleCategory,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Theory(DisplayName = nameof(ThrowErrorCantUpdateCategoryFromInvalidData))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
     nameof(UpdateCategoryTestDataGenerator.GetInvalidCategories),
     parameters: 10,
     MemberType = typeof(UpdateCategoryTestDataGenerator)
    )]
    public async Task ThrowErrorCantUpdateCategoryFromInvalidData(UpdateCategoryInput invalidInput, string expectedExceptionMessage)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleCategory = _fixture.GetValidCategory();
        invalidInput.Id = exampleCategory.Id;
        repositoryMock.Setup(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        var task = async ()
            => await useCase.Handle(invalidInput, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);

        repositoryMock.Verify(x =>
            x.Get(exampleCategory.Id,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

}
