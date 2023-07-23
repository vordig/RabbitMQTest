using MassTransit;
using MessagingContracts.Events;
using MessagingContracts;
using System.Text.Json;

namespace ProjectB;

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
        _logger.LogInformation("ProjectB recieve a contract {Id}", contract.Id);
        var eventDocument = contract.EventDocument.RootElement;
        var code = eventDocument.GetProperty("code").GetString();
        switch (code)
        {
            case ProjectAEvents.Codes.Event1:
                HandleProjectAEvent1(contract.EventDocument);
                break;
            case ProjectAEvents.Codes.Event2:
                HandleProjectAEvent2(contract.EventDocument);
                break;
            default:
                _logger.LogInformation("ProjectB is not interested in {EventCode} event from contract {Id}", code, contract.Id);
                break;
        }
        return Task.CompletedTask;
    }

    private void HandleProjectAEvent1(JsonDocument eventDocument)
    {
        var eventContract = ProjectAEvents.Event1.FromDocument(eventDocument);
        _logger.LogInformation("ProjectB handle a ProjectAEvent1 event {Id}", eventContract.Id);
    }

    private void HandleProjectAEvent2(JsonDocument eventDocument)
    {
        var eventContract = ProjectAEvents.Event2.FromDocument(eventDocument);
        _logger.LogInformation("ProjectB handle a ProjectAEvent2 event {Id}", eventContract.Id);
    }
}

