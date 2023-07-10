using MassTransit;
using MessagingContracts;

namespace ProjectA;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/", CreateEvent);
    }

    private static async Task<IResult> CreateEvent(IPublishEndpoint bus, ILogger<Program> logger)
    {
        var contract = new ProjectContract(Guid.NewGuid());
        logger.LogInformation("ProjectA create an event {Id}", contract.Id);
        await bus.Publish(contract);
        return Results.Ok(contract);
    }
}
