using LightDiet.Recipe.Application.UseCases.Category.Common.Dto;
using LightDiet.Recipe.Application.UseCases.Category.CreateCategory.Dto;
using LightDiet.Recipe.Application.UseCases.Category.GetCategory.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LightDiet.Recipe.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(
        IMediator mediator
    )
        => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryInput model, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(model, cancellationToken);

        return CreatedAtAction(
            nameof(Create), 
            new { output.Id }, 
            output
        );
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryModelOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var output = await _mediator.Send(new GetCategoryInput(id), cancellationToken);

        return Ok(output);
    }
}
