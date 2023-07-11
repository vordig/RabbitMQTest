using System.Text.Json;

namespace MessagingContracts;

public record ProjectContract(Guid Id, DateTime CreatedAt, JsonDocument Event)
{
    public static ProjectContract NewContract(object eventContract)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var contract = new ProjectContract(Guid.NewGuid(), DateTime.Now, JsonSerializer.SerializeToDocument(eventContract, options));
        return contract;
    }
}