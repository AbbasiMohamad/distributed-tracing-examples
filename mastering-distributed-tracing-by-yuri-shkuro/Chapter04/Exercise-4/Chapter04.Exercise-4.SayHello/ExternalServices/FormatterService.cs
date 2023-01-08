using Chapter04.Exercise_4.SayHello.Models;
using System.Xml.Linq;

namespace Chapter04.Exercise_4.SayHello.ExternalServices;

public class FormatterService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FormatterService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<string> GetFormattedGretting(Person person)
    {
        var client = _httpClientFactory.CreateClient();
        var response =
            await client.GetAsync($"http://localhost:8082/formatGreeting?name={person.Name}&title={person.Title}&description={person.Description}");
        var result = response.Content.ReadAsStringAsync().Result;
        return result;
    }
}
