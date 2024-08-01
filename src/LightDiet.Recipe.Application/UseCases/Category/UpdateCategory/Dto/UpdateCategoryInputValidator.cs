using FluentValidation;

namespace LightDiet.Recipe.Application.UseCases.Category.UpdateCategory.Dto;

public class UpdateCategoryInputValidator : AbstractValidator<UpdateCategoryInput>
{
    public UpdateCategoryInputValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("'Id' must not be empty.");
    }
}
