using System.Text.Json.Serialization;
using DotNetEnv;
using Infrastructure.Data;
using Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

Env.Load("C:/Asp.net/Shop/Server/.env");
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.Converters.Add(new StringEnumConverter
        {
            AllowIntegerValues = false
        });
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseInfrastructure();
app.MapControllers();
app.Run();
