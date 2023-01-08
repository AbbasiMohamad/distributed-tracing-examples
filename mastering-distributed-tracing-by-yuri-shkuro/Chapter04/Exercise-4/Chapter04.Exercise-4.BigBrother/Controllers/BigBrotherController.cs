using Chapter04.Exercise_4.BigBrother.Models;
using Chapter04.Exercise_4.BigBrother.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chapter04.Exercise_4.BigBrother.Controllers;

[ApiController]
[Route("")]
public class BigBrotherController : ControllerBase
{

    private readonly ILogger<BigBrotherController> _logger;
    private readonly MySqlRepository _mySqlRepository;

    public BigBrotherController(ILogger<BigBrotherController> logger, MySqlRepository mySqlRepository)
    {
        _logger = logger;
        _mySqlRepository = mySqlRepository;
    }


    [HttpGet("getPerson/{name}")]
    public async Task<Person> GetPerson([FromRoute] string name)
    {
        Person greeting;
        try
        {
            greeting = await _mySqlRepository.GetPersonAsync(name);
        }
        catch (Exception e)
        {
            return null!;
        }
        return greeting;
    }
}
