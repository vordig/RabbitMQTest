using MassTransit;
using MessagingContracts;
using MessagingContracts.Events;
using MessagingContracts.Events.ProjectB;
using System.Text.Json;

namespace ProjectA;

public class ProjectAContractConsumer : IConsumer<ProjectAContract>
{
    private readonly ILogger<ProjectAContractConsumer> _logger;

    public ProjectAContractConsumer(ILogger<ProjectAContractConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProjectAContract> context)
    {
        var contract = context.Message;
        _logger.LogInformation("ProjectA recieve a contract {Id}", contract.Id);
        var eventDocument = contract.EventDocument.RootElement;
        var code = eventDocument.GetProperty("code").GetString();
        switch (code)
        {
            case EventCodes.ProjectBEvent1:
                HandleProjectBEvent1(contract.EventDocument);
                break;
            default:
                _logger.LogInformation("ProjectA is not interested in {EventCode} event from contract {Id}", code, contract.Id);
                break;
        }
        return Task.CompletedTask;
    }

    private void HandleProjectBEvent1(JsonDocument eventDocument)
    {
        var eventContract = ProjectBEvent1.FromDocument(eventDocument);
        _logger.LogInformation("ProjectA handle a ProjectBEvent1 event {Id}", eventContract.Id);
    }
}
