using MassTransit;
using MessagingContracts.Events;
using MessagingContracts;
using System.Text.Json;
using MessagingContracts.Events.ProjectB;

namespace ProjectB;

public class ProjectBContractConsumer : IConsumer<ProjectBContract>
{
    private readonly ILogger<ProjectBContractConsumer> _logger;

    public ProjectBContractConsumer(ILogger<ProjectBContractConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProjectBContract> context)
    {
        var contract = context.Message;
        _logger.LogInformation("ProjectB recieve a contract {Id}", contract.Id);
        var eventDocument = contract.EventDocument.RootElement;
        var code = eventDocument.GetProperty("code").GetString();
        switch (code)
        {
            case EventCodes.ProjectAEvent1:
                HandleProjectAEvent1(contract.EventDocument);
                break;
            case EventCodes.ProjectAEvent2:
                HandleProjectAEvent2(contract.EventDocument);
                break;
            default:
                _logger.LogInformation("ProjectA is not interested in {EventCode} event from contract {Id}", code, contract.Id);
                break;
        }
        return Task.CompletedTask;
    }

    private void HandleProjectAEvent1(JsonDocument eventDocument)
    {
        var eventContract = ProjectBEvent1.FromDocument(eventDocument);
        _logger.LogInformation("ProjectB handle a ProjectAEvent1 event {Id}", eventContract.Id);
    }

    private void HandleProjectAEvent2(JsonDocument eventDocument)
    {
        var eventContract = ProjectBEvent2.FromDocument(eventDocument);
        _logger.LogInformation("ProjectB handle a ProjectAEvent2 event {Id}", eventContract.Id);
    }
}

