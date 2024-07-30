using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using MediatR;

namespace LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;

public class GetCategoryInput 
    : IRequest<CategoryModelOutput>
{
    public Guid Id { get; set; }

    public GetCategoryInput(Guid id)
    {
        Id = id;
    }

}
