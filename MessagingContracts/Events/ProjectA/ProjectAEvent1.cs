namespace MessagingContracts.Events.ProjectA;

public record ProjectAEvent1(Guid Id) : EventBase<ProjectAEvent1>(EventCodes.ProjectAEvent1);
