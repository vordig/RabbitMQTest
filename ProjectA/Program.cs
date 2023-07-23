using MassTransit;
using MessagingContracts;
using MessagingContracts.Events;
using Microsoft.Extensions.Options;
using ProjectA;
using RabbitMQ.Client;
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
        busConfigurator.AddConsumer<ProjectContractConsumer>();
        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            var settings = context.GetRequiredService<MessageBrokerSettings>();
            configurator.Host(settings.Host, h =>
            {
                h.Username(settings.Username);
                h.Password(settings.Password);
            });

            configurator.Send<ProjectContract>(topologyConfigurator =>
            {
                topologyConfigurator.UseRoutingKeyFormatter(context => context.Message.Code);
            });
            configurator.Message<ProjectContract>(topologyConfigurator =>
            {
                topologyConfigurator.SetEntityName(Topics.ProjectA);
            });
            configurator.Publish<ProjectContract>(topologyConfigurator =>
            {
                topologyConfigurator.ExchangeType = ExchangeType.Direct;
            });

            configurator.ReceiveEndpoint("project-a-queue", endpointConfigurator =>
            {
                endpointConfigurator.ConfigureConsumeTopology = false;
                endpointConfigurator.Consumer<ProjectContractConsumer>(context);
                endpointConfigurator.Bind(Topics.ProjectB, bindingConfigurator =>
                {
                    bindingConfigurator.RoutingKey = ProjectBEvents.Codes.Event1;
                    bindingConfigurator.ExchangeType = ExchangeType.Direct;
                });
            });
        });
    });

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

    var app = builder.Build();

    app.MapEndpoints();

    app.Run();
}