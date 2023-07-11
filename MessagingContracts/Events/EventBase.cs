using System.Text.Json;

namespace MessagingContracts.Events;

public abstract record EventBase<TEvent>(string Code)
{
    public static TEvent FromDocument(JsonDocument eventDocument)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var eventContract = eventDocument.Deserialize<TEvent>(options);
        return eventContract is null ? throw new Exception() : eventContract;
    }
}
