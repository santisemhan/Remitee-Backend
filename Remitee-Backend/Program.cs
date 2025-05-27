using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Remitee_Backend.Configuration;
using Remitee_Backend.Middleware.GlobalException;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
    loggingBuilder.AddDebug();
});

services.AddControllers();

services.AddDatabaseConfiguration(configuration);
services.AddRepositoryConfiguration();

services.AddServiceConfiguration();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Remitee Backend", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandler>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = "api/docs";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Remitee Backend");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
