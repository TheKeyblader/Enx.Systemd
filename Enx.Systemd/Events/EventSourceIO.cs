using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

// ReSharper disable once InconsistentNaming
/// <summary>
/// IO-specific event source wrapper.
/// </summary>
/// <param name="handle">The event source handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public class EventSourceIO(SdEventSourceHandle handle, bool shouldRef) : EventSourceNotExit(handle, shouldRef)
{
    /// <summary>
    /// Gets or sets the file descriptor associated with the source.
    /// </summary>
    public int Fd
    {
        get
        {
            int r = EventSourceGetIoFd(Handle);
            ThrowIfError(r);
            return r;
        }
        set => ThrowIfError(EventSourceSetIoFd(Handle, value));
    }

    /// <summary>
    /// Gets or sets ownership of the file descriptor.
    /// </summary>
    public bool Own
    {
        get => EventSourceGetIoFdOwn(Handle);
        set => ThrowIfError(EventSourceSetIoFdOwn(Handle, value));
    }

    /// <summary>
    /// Gets or sets the events mask for this source.
    /// </summary>
    public uint Events
    {
        get
        {
            ThrowIfError(EventSourceGetIoEvents(Handle, out uint events));
            return events;
        }
        set => ThrowIfError(EventSourceSetIoEvents(Handle, value));
    }

    /// <summary>
    /// Gets the received events mask, or null if not available.
    /// </summary>
    public uint? REvents
    {
        get
        {
            int r = EventSourceGetIoRevents(Handle, out uint events);
            if (r == -Errno.ENODATA) return null;
            ThrowIfError(r);
            return events;
        }
    }
}
