using Chapter04.Exercise_4.BigBrother.Repositories;
using MySqlConnector;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
