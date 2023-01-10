using Chapter04.Exercise_4.BigBrother.Repositories;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Jaeger;
using MySqlConnector;
using OpenTracing;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(_ =>
{
    try
    {
        var con = new MySqlConnection(builder.Configuration["ConnectionStrings:Default"]);
        con.Open();
        con.Ping();
        return con;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Database in not availible with {e.Message}");
        Environment.Exit(1);
        return null;
    }
});

builder.Services.AddTransient<MySqlRepository>();

builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = "dotnet-2-hello";
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory)
           .WithSender(new UdpSender()).Build();

    var tracer = new Tracer.Builder(serviceName)
           .WithSampler(new ConstSampler(true))
           .WithReporter(reporter)
           .Build();
    return tracer;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
