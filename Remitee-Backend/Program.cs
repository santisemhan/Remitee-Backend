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

services.AddWebSecurityConfiguration();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Remitee Backend", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
var environment = app.Environment;

app.UseMiddleware<ExceptionHandler>();

if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = "api/docs";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Remitee Backend");
    });
    app.UseCors("_AllowOriginDev");
}
else if (environment.IsProduction())
{
    app.UseCors("_AllowOrigin");
}
else
{
    throw new Exception("Invalid environment");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
