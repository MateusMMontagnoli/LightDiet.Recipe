
using FluentValidation;
using LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;

namespace LightDiet.Recipe.Application.UseCases.Category.DeleteCategory.Dto;

public class DeleteCategoryInputValidator 
    : AbstractValidator<DeleteCategoryInput>
{
    public DeleteCategoryInputValidator()
        => RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
}

