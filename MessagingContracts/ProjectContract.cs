using MessagingContracts.Events;
using System.Text.Json;

namespace MessagingContracts;

public record ProjectContract
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Code { get; init; } = default!;
    public JsonDocument EventDocument { get; init; } = default!;

    public static ProjectContract NewContract(object eventContract)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var contract = new ProjectContract
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Code = (eventContract as IEvent)?.Code ?? string.Empty,
            EventDocument = JsonSerializer.SerializeToDocument(eventContract, options)
        };

        return contract;
    }
}