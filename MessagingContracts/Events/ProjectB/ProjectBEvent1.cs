namespace MessagingContracts.Events.ProjectB;

public record ProjectBEvent1(Guid Id) : EventBase<ProjectBEvent1>(EventCodes.ProjectBEvent1);
