using System.Text.Json.Serialization;

namespace TypeChatSharp.Example.Calendar;

// The following types define the structure of an object of type CalendarActions that represents an array of requested calendar actions
public class CalendarActions
{
    public Action[] Actions { get; set; } = Array.Empty<Action>();
}

// metadata properties, that start with $ must always be first when represented in json
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
[JsonDerivedType(typeof(AddEventAction), typeDiscriminator: "add event")]
[JsonDerivedType(typeof(RemoveEventAction), typeDiscriminator: "remove event")]
[JsonDerivedType(typeof(AddParticipantsAction), typeDiscriminator: "add participants")]
[JsonDerivedType(typeof(ChangeTimeRangeAction), typeDiscriminator: "change time range")]
[JsonDerivedType(typeof(ChangeDescriptionAction), typeDiscriminator: "change description")]
[JsonDerivedType(typeof(FindEventsAction), typeDiscriminator: "find events")]
public class Action
{
    // text typed by the user that the system did not understand (if applicable)
    public string? Text { get; set; }
}

public class AddEventAction : Action
{
    public Event? EventItem { get; set; }
}

public class RemoveEventAction : Action
{

    public EventReference? EventReference { get; set; }
}

public class AddParticipantsAction : Action
{
    // event to be augmented; if not specified assume last event discussed
    public EventReference? EventReference { get; set; }

    // new participants (one or more)
    public string[] Participants { get; set; } = Array.Empty<string>();
}

public class ChangeTimeRangeAction : Action
{
    // event to be changed
    public EventReference? EventReference { get; set; }
    // new time range for the event
    public EventTimeRange? TimeRange { get; set; }
}

public class ChangeDescriptionAction : Action
{
    // event to be changed
    public EventReference? EventReference { get; set; }
    // new description for the event
    public string? Description { get; set; }
}

public class FindEventsAction : Action
{
    // one or more event properties to use to search for matching events
    public EventReference? EventReference { get; set; }
}

public class EventTimeRange
{
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Duration { get; set; }
}

public class Event
{
    // date (example: March 22, 2024) or relative date (example: after EventReference)
    public string? Day { get; set; }
    public EventTimeRange? TimeRange { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    // a list of people or named groups like 'team'
    public string[] Participants { get; set; } = Array.Empty<string>();
}

// properties used by the requester in referring to an event
// these properties are only specified if given directly by the requester
public class EventReference
{
    // date (example: March 22, 2024) or relative date (example: after EventReference)
    public string? Day { get; set; }
    public EventTimeRange? TimeRange { get; set; }
    // (examples: this month, this week, in the next two days)
    public string? DayRange { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string[] Participants { get; set; } = Array.Empty<string>();
}