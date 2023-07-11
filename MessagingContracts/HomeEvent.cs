using System.Text.Json;

namespace MessagingContracts;

public record HomeEvent(Guid Id) : EventBase<HomeEvent>(EventCodes.HomeEvent);
