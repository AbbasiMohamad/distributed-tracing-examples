using Chapter04.Exercise_2.Repositories;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace Chapter04.Exercise_2.Controllers;
[ApiController]
[Route("")]
public class HelloController : ControllerBase
{

    private readonly MySqlRepository _mySqlRepository;
    private readonly ITracer _tracer;

    public HelloController(MySqlRepository mySqlRepository, ITracer tracer)
    {
        _mySqlRepository = mySqlRepository;
        _tracer = tracer;
    }


    [HttpGet("sayHello/{name}")]
    public async Task<string> SayHello([FromRoute] string name) 
    {
        using var scope = _tracer.BuildSpan("say-hello").StartActive(true);
        string greeting = string.Empty;
        try
        {
            greeting = await _mySqlRepository.GetHelloSentence(name, scope.Span);
            scope.Span.SetTag("response", greeting);
        } catch (Exception ex)
        {
            scope.Span.SetTag("error", true);
            scope.Span.Log(ex.Message);
        }
        return greeting;
    }

}
