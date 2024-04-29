using Arduino.Library;
using Arduino.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

using Hangfire;
using Hangfire.Server;
using Hangfire.Storage.SQLite;

using Arduino.Controllers;
using Arduino.Services;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appSettings.json");
IConfiguration configuration = configurationBuilder.Build();

var connectionLite = configuration.GetSection("ConexaoSqlite").GetSection("SqliteConnectionString").Value;

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
.ReadFrom.Configuration(ctx.Configuration)));



//Log.Logger = new LoggerConfiguration()
//    .Enrich.FromLogContext()
//    .WriteTo.MSSqlServer(configuration.GetConnectionString("DefaultConnection"), sinkOptions: new MSSqlServerSinkOptions { TableName = "Log" }
//    , null, null, LogEventLevel.Information, null, null, null, null)
//    .CreateLogger();


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

Log.Logger.Information("This informational message will be written to SQLite database");

Log.CloseAndFlush();
var origins = configuration.GetSection("AllowedOrigins").Value
                           .Split(';');
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsApi",
//        builder => builder.WithOrigins(origins)
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader());
//});

GlobalConfiguration.Configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(connectionLite);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISerialPortConnector, SerialPortConnector>();
builder.Services.AddTransient<ILogActionsMethods, LogActionsMethods>();
builder.Services.AddTransient<IValvulaMethods, ValvulaMethods>();
builder.Services.AddTransient<IArduinoService, ArduinoService>();

//builder.Services.AddDbContext<CTRLArduinoContext>(options =>
//    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<CTRLArduinoContext>(options =>
    options.UseSqlite(connectionLite));


var app = builder.Build();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
