using FluentValidation;

namespace LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;

public class GetCategoryInputValidator 
    : AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidator()
        => RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty."); 

}
