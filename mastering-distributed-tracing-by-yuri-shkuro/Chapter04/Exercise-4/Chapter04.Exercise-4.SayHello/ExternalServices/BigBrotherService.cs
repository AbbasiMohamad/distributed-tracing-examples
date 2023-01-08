using Chapter04.Exercise_4.SayHello.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Chapter04.Exercise_4.SayHello.ExternalServices;

public class BigBrotherService
{
    private readonly IHttpClientFactory _httpClient;

    public BigBrotherService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<Person> GetPerson(string name)
    {
        var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"http://localhost:8081/getPerson/{name}");
        var json = response.Content.ReadAsStringAsync().Result;
        var person = JsonConvert.DeserializeObject<Person>(json);
        return person;
    }
}
