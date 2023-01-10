using Chapter04.Exercise_4.SayHello.ExternalServices;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using System.Net.Http.Headers;

namespace Chapter04.Exercise_4.SayHello.Controllers;
[ApiController]
[Route("")]
public class HelloController : ControllerBase
{
    
    private readonly ILogger<HelloController> _logger;
    private readonly BigBrotherService _bigBrotherService;
    private readonly FormatterService _formatterService;
    private readonly ITracer _tracer;

    public HelloController(ILogger<HelloController> logger, BigBrotherService bigBrotherService,
        FormatterService formatterService, ITracer tracer)
    {
        _logger = logger;
        _bigBrotherService = bigBrotherService;
        _formatterService = formatterService;
        _tracer = tracer;
    }

    [HttpGet("sayHello/{name}")]
    public async Task<string> SayHello([FromRoute] string name)
    {
        using var scope = _tracer.BuildSpan("say-hello").StartActive(true);

        var greetingString = string.Empty;
        try
        {
            var person = await _bigBrotherService.GetPerson(name);
            greetingString = await _formatterService.GetFormattedGretting(person);
            scope.Span.SetTag("response", greetingString);
        }
        catch (Exception ex)
        {
            scope.Span.SetTag("error", true);
            scope.Span.Log(ex.Message);
        }
        scope.Span.Finish();
        return greetingString;
    }
}
