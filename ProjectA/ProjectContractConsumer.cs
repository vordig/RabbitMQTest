using MassTransit;
using MessagingContracts;
using MessagingContracts.Events;
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
        var eventDocument = contract.EventDocument.RootElement;
        var code = eventDocument.GetProperty("code").GetString();
        switch (code)
        {
            case ProjectBEvents.Codes.Event1:
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
        var eventContract = ProjectBEvents.Event1.FromDocument(eventDocument);
        _logger.LogInformation("ProjectA handle a ProjectBEvent1 event {Id}", eventContract.Id);
    }
}
