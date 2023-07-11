using MassTransit;
using Microsoft.Extensions.Options;
using ProjectA;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Default";
Log.Information("Application starting up in {Environment} mode", env);

try
{
    RunApplication(args);

    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static void RunApplication(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

    builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

    builder.Services.AddMassTransit(busConfigurator =>
    {
        busConfigurator.SetKebabCaseEndpointNameFormatter();
        busConfigurator.AddConsumer<ProjectAContractConsumer>();
        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            var settings = context.GetRequiredService<MessageBrokerSettings>();
            configurator.Host(settings.Host, h =>
            {
                h.Username(settings.Username);
                h.Password(settings.Password);
            });
            configurator.ConfigureEndpoints(context);
        });
    });

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    var app = builder.Build();

    app.MapEndpoints();

    app.Run();
}