using Chapter04.Exercise_4.BigBrother.Models;
using MySqlConnector;
using OpenTracing;

namespace Chapter04.Exercise_4.BigBrother.Repositories;

public class MySqlRepository
{
    private readonly MySqlConnection _mySqlConnection;
    private readonly ILogger<MySqlRepository> _logger;

    public MySqlRepository(MySqlConnection mySqlConnection, ILogger<MySqlRepository> logger)
    {
        _mySqlConnection = mySqlConnection;
        _logger = logger;
        try
        {
            _mySqlConnection.Open();
            _mySqlConnection.Ping();

        }
        catch (Exception ex)
        {
            _logger.LogCritical("cannot connect to database", ex);
        }
    }



    public async Task<Person> GetPersonAsync(string name)
    {
        using var command = new MySqlCommand("select title, description from people where name =@name", _mySqlConnection);
        command.Parameters.AddWithValue("@name", name);
        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var person = MakePerson(reader.GetValue(0)?.ToString(), name, reader.GetValue(1)?.ToString());
            return person;
        }
        else
        {
            var person = MakePerson(name, null, null);
            return person;
        }
    }

    private Person MakePerson(string name, string title, string description)
    {
        return new Person
        {
            Name = name,
            Title = title,
            Description = description
        };
    }

}