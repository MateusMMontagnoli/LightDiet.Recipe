using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.DeleteCategory.Dto;

public class DeleteCategoryInput : IRequest<Unit>
{
    public Guid Id { get; set; }

    public DeleteCategoryInput(Guid id)
    {
        Id = id;
    }

}
