using System.Text.Json;

namespace MessagingContracts;

public abstract record ProjectContract<TContract>
    where TContract : ProjectContract<TContract>, new()
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public JsonDocument EventDocument { get; set; } = default!;

    public static TContract NewContract(object eventContract)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var contract = new TContract 
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            EventDocument = JsonSerializer.SerializeToDocument(eventContract, options)
        };
        return contract;
    }
}