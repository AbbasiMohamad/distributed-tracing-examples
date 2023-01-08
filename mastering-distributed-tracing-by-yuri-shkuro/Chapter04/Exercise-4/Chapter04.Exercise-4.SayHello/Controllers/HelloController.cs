using Chapter04.Exercise_4.SayHello.ExternalServices;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chapter04.Exercise_4.SayHello.Controllers;
[ApiController]
[Route("")]
public class HelloController : ControllerBase
{
    
    private readonly ILogger<HelloController> _logger;
    private readonly BigBrotherService _bigBrotherService;
    private readonly FormatterService _formatterService;

    public HelloController(ILogger<HelloController> logger, BigBrotherService bigBrotherService,
        FormatterService formatterService)
    {
        _logger = logger;
        _bigBrotherService = bigBrotherService;
        _formatterService = formatterService;
    }

    [HttpGet("sayHello/{name}")]
    public async Task<string> SayHello([FromRoute] string name)
    {
        var person = await _bigBrotherService.GetPerson(name);
        var greetingString = await _formatterService.GetFormattedGretting(person);
        return greetingString;
    }
}
