using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

/// <summary>
/// Wraps a systemd <c>sd_event_source</c>.
/// </summary>
/// <param name="handle">The event source handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public class EventSource(SdEventSourceHandle handle, bool shouldRef)
    : BaseWrapper<SdEventSourceHandle>(handle, shouldRef)
{
    /// <summary>
    /// Increments the native reference count for the underlying event source handle.
    /// </summary>
    public override void Ref() => EventSourceRef(Handle);

    /// <summary>
    /// Gets the event source type.
    /// </summary>
    public EventSourceType Type => SdEventSource.GetType(Handle);

    private readonly PropertyUpdater<SdEventHandle, Event> _event
        = new(h => new Event(h, true));

    /// <summary>
    /// Gets the parent event loop for this source.
    /// </summary>
    public Event Event => _event.GetValue(EventSourceGetEvent(Handle))!;

    /// <summary>
    /// Gets or sets whether the source is enabled.
    /// </summary>
    public bool Enabled
    {
        get
        {
            ThrowIfError(EventSourceGetEnabled(Handle, out bool enabled));
            return enabled;
        }
        set => ThrowIfError(EventSourceSetEnabled(Handle, value));
    }

    /// <summary>
    /// Gets or sets the source description.
    /// </summary>
    public string Description
    {
        get
        {
            ThrowIfError(EventSourceGetDescription(Handle, out string description));
            return description;
        }
        set { ThrowIfError(EventSourceSetDescription(Handle, value)); }
    }

    /// <summary>
    /// Gets or sets the source priority.
    /// </summary>
    public long Priority
    {
        get
        {
            ThrowIfError(EventSourceGetPriority(Handle, out long priority));
            return priority;
        }
        set => ThrowIfError(EventSourceSetPriority(Handle, value));
    }

    /// <summary>
    /// Gets or sets whether the source is floating.
    /// </summary>
    public bool Floating
    {
        get => EventSourceGetFloating(Handle);
        set => ThrowIfError(EventSourceSetFloating(Handle, value));
    }

    private readonly CustomDataPropertyUpdater _userdata = new();

    /// <summary>
    /// Gets or sets user data associated with the source.
    /// </summary>
    public object? UserData
    {
        get => _userdata.Get(EventSourceGetUserdata(Handle));
        set => EventSourceSetUserdata(Handle, _userdata.Set(value, EventSourceGetUserdata(Handle)));
    }
}
