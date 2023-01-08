using MySqlConnector;
using OpenTracing;

namespace Chapter04.Exercise_2.Repositories;

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


    public async Task<string> GetHelloSentence(string name, ISpan span)
    {
        using var command = new MySqlCommand("select title, description from people where name =@name", _mySqlConnection);
        command.Parameters.AddWithValue("@name", name);
        using var reader = await command.ExecuteReaderAsync();
        if (reader.Read())
        {
            span.Log(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", name),
                new KeyValuePair<string, object>("title", reader.GetValue(0)?.ToString()),
                new KeyValuePair<string, object>("description", reader.GetValue(1)?.ToString()),
            });
            return $"Hello, {reader.GetValue(0)?.ToString()} {name}! {reader.GetValue(1)?.ToString()}";
        }
        else
        {
            return $"Hello, {name}!";
        }
    }

}