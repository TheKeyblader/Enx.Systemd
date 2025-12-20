namespace Enx.Systemd.Events;

/// <summary>
/// Represents the native event source type.
/// </summary>
public enum EventSourceType
{
    IO,
    TimeRealtime,
    TimeBootTime,
    TimeMonotonic,
    TimeRealtimeAlarm,
    TimeBoottimeAlarm,
    Signal,
    Child,
    Defer,
    Post,
    Exit,
    Watch,
    INotify,
    MemoryPressure
}
