namespace MessagingContracts;

public record SomeEvent(Guid Id) : EventBase<SomeEvent>(EventCodes.SomeEvent);
