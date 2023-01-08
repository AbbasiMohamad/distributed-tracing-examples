using Chapter04.Exercise_1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chapter04.Exercise_1.Controllers;
[ApiController]
[Route("")]
public class HelloController : ControllerBase
{

    private readonly MySqlRepository _mySqlRepository;

    public HelloController(MySqlRepository mySqlRepository)
    {
        _mySqlRepository = mySqlRepository;
    }

    [HttpGet("sayHello/{name}")]
    public async Task<string> SayHello([FromRoute] string name) =>
        await _mySqlRepository.GetHelloSentence(name);
    
}
