using MassTransit;
using MessagingContracts;

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
        _logger.LogInformation("ProjectA recieve an event {Id}", contract.Id);
        return Task.CompletedTask;
    }
}
