using MassTransit;
using MessagingContracts;
using System.Text.Json;

namespace ProjectA;

public class ProjectContractConsumer : IConsumer<ProjectContract>
{
    private readonly ILogger<ProjectContractConsumer> _logger;

    public ProjectContractConsumer(ILogger<ProjectContractConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProjectContract> context)
    {
        var contract = context.Message;
        _logger.LogInformation("ProjectA recieve a contract {Id}", contract.Id);
        var eventDocument = contract.Event.RootElement;
        var code = eventDocument.GetProperty("code").GetString();
        switch (code)
        {
            case EventCodes.HomeEvent:
                HandleHomeEvent(contract.Event);
                break;
            case EventCodes.SomeEvent:
                HandleSomeEvent(contract.Event);
                break;
        }
        return Task.CompletedTask;
    }

    private void HandleHomeEvent(JsonDocument eventDocument)
    {
        var eventContract = HomeEvent.FromDocument(eventDocument);
        _logger.LogInformation("ProjectA handle a home event {Id}", eventContract.Id);
    }

    private void HandleSomeEvent(JsonDocument eventDocument)
    {
        var eventContract = SomeEvent.FromDocument(eventDocument);
        _logger.LogInformation("ProjectA handle a some event {Id}", eventContract.Id);
    }
}
