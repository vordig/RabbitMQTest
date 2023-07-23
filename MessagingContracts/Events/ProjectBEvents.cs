namespace MessagingContracts.Events;

public static class ProjectBEvents
{
    public static class Codes
    {
        public const string Event1 = "project-b-event-1";
        public const string Event2 = "project-b-event-2";
    }

    public record Event1(Guid Id) : EventBase<Event1>(Codes.Event1);
    public record Event2(Guid Id) : EventBase<Event2>(Codes.Event2);
}
