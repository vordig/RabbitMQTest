using MassTransit;
using MessagingContracts;
using MessagingContracts.Events;

namespace ProjectB;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/event-1", CreateEvent1);
        endpoints.MapGet("/event-2", CreateEvent2);
    }

    private static async Task<IResult> CreateEvent1(IPublishEndpoint bus, ILogger<Program> logger)
    {
        var eventContract = new ProjectBEvents.Event1(Guid.NewGuid());
        var contract = ProjectContract.NewContract(eventContract);
        logger.LogInformation("ProjectB create a contract {ContractId} with an event {EventId}", contract.Id, eventContract.Id);
        await bus.Publish(contract);
        return Results.Ok(contract);
    }

    private static async Task<IResult> CreateEvent2(IPublishEndpoint bus, ILogger<Program> logger)
    {
        var eventContract = new ProjectBEvents.Event2(Guid.NewGuid());
        var contract = ProjectContract.NewContract(eventContract);
        logger.LogInformation("ProjectB create a contract {ContractId} with an event {EventId}", contract.Id, eventContract.Id);
        await bus.Publish(contract);
        return Results.Ok(contract);
    }
}
