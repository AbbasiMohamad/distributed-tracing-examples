using MySqlConnector;
using OpenTracing;

namespace Chapter04.Exercise_3.Repositories;

public class MySqlRepository
{
    private readonly MySqlConnection _mySqlConnection;
    private readonly ILogger<MySqlRepository> _logger;
    private readonly ITracer _tracer;

    public MySqlRepository(MySqlConnection mySqlConnection, ILogger<MySqlRepository> logger
                            ,ITracer tracer)
    {
        _mySqlConnection = mySqlConnection;
        _logger = logger;
        _tracer = tracer;
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
        var scope = _tracer.BuildSpan("get-person").StartActive(true);
        using var command = new MySqlCommand("select title, description from people where name =@name", _mySqlConnection);
        command.Parameters.AddWithValue("@name", name);
        scope.Span.SetTag("db.statement", command.CommandText);
        using var reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            span.Log(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", name),
                new KeyValuePair<string, object>("title", reader.GetValue(0)?.ToString()),
                new KeyValuePair<string, object>("description", reader.GetValue(1)?.ToString()),
            });
            var response = FormatGreeting(reader.GetValue(0)?.ToString(), name, reader.GetValue(1)?.ToString());
            scope.Span.Finish();
            return response;
        }
        else
        {
            var response = FormatGreeting(null, name, null);
            scope.Span.Finish();
            return response;
        }
    }


    private string FormatGreeting(string title, string name, string description)
    {
        var scope = _tracer.BuildSpan("format-greeting").StartActive(true);

        var response = string.IsNullOrEmpty(description) ?
             $"Hello, {name}!" : $"Hello, {title} {name}! {description}";
        scope.Span.Finish();
        return response;
    }

}