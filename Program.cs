using Arduino.Library;
using Arduino.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

using Hangfire;
using Hangfire.Heartbeat;
using Hangfire.Storage.SQLite;

using Arduino.Controllers;
using Arduino.Services;
using WebSample;
using Hangfire.JobsLogger;
using Hangfire.SqlServer;



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

#region Hangfire

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage("Hangfire.db")
    .UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(30))
    .UseJobsLogger());


JobStorage.Current = new SQLiteStorage("Hangfire.db");

builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "Arduino", "default" };
});




RecurringJob.AddOrUpdate("TaskMethod()", (TaskSample t) => t.TaskMethod(), "42 17 * * *", TimeZoneInfo.Local);
RecurringJob.AddOrUpdate("TaskMethodEnd()", (TaskSample t) => t.TaskMethodEnd(), "43 17 * * *",TimeZoneInfo.Local);

#endregion

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
app.UseHangfireDashboard("/hangfire");

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
