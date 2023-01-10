using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace Chapter04.Exercise_4.Formatter.Controllers;

[ApiController]
[Route("")]
public class FormatterController : ControllerBase
{

    private readonly ILogger<FormatterController> _logger;
    private readonly ITracer _tracer;

    public FormatterController(ILogger<FormatterController> logger, ITracer tracer)
    {
        _logger = logger;
        _tracer = tracer;
    }

    [HttpGet("formatGreeting")]
    public ActionResult<string> Get(string name, string? title, string? description)
    {
        using var scope = _tracer.BuildSpan("format-greeting").StartActive(true);
        var response = FormatGreeting(title, name, description);
        return Ok(response);
    }


    private string FormatGreeting(string? title, string name, string? description)
    {
        title = string.IsNullOrEmpty(title) ? string.Empty : title + " ";
        var response = $"Hello, {title}{name}! {description}";
        return response;
    }
}