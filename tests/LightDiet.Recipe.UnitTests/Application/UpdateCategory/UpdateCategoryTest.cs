using FluentAssertions;
using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
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

    [Fact(DisplayName = nameof(UpdateCategoryOk))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task UpdateCategoryOk()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var exampleCategory = _fixture.GetValidCategory();

        repositoryMock.Setup(x =>
            x.Get(exampleCategory.Id, 
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleCategory);

        var input = new UseCase.Dto.UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription(),
            !exampleCategory.IsActive
        );

        var useCase = new UseCase.UpdateCategory(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.IsActive.Should().Be(input.IsActive);
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
}
