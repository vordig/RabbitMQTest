using MassTransit;
using MessagingContracts;

namespace ProjectA;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", CreateEvent);
        endpoints.MapGet("/some-event", CreateSomeEvent);
    }

    private static async Task<IResult> CreateEvent(IPublishEndpoint bus, ILogger<Program> logger)
    {
        var eventContract = new HomeEvent(Guid.NewGuid());
        var contract = ProjectContract.NewContract(eventContract);
        logger.LogInformation("ProjectA create a contract {ContractId} with a home event {EventId}", contract.Id, eventContract.Id);
        await bus.Publish(contract);
        return Results.Ok(contract);
    }

    private static async Task<IResult> CreateSomeEvent(IPublishEndpoint bus, ILogger<Program> logger)
    {
        var eventContract = new SomeEvent(Guid.NewGuid());
        var contract = ProjectContract.NewContract(eventContract);
        logger.LogInformation("ProjectA create a contract {ContractId} with a some event {EventId}", contract.Id, eventContract.Id);
        await bus.Publish(contract);
        return Results.Ok(contract);
    }
}
