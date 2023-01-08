using Microsoft.AspNetCore.Mvc;

namespace Chapter04.Exercise_4.Formatter.Controllers;

[ApiController]
[Route("")]
public class FormatterController : ControllerBase
{

    private readonly ILogger<FormatterController> _logger;

    public FormatterController(ILogger<FormatterController> logger)
    {
        _logger = logger;
    }

    [HttpGet("formatGreeting")]
    public ActionResult<string> Get(string name, string? title, string? description)
    {
        return Ok(FormatGreeting(title, name, description));
    }


    private string FormatGreeting(string? title, string name, string? description)
    {
        title = string.IsNullOrEmpty(title) ? string.Empty : title + " ";
        var response = $"Hello, {title}{name}! {description}";
        return response;
    }
}