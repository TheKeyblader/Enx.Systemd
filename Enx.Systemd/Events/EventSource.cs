using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

public class EventSource : IDisposable
{
    public SdEventSourceHandle Handle { get; }

    public EventSource(SdEventSourceHandle handle)
    {
        Handle = handle;
    }

    public string Description
    {
        get
        {
            ThrowIfError(EventSourceGetDescription(Handle, out string description));
            return description;
        }
        set { ThrowIfError(EventSourceSetDescription(Handle, value)); }
    }

    public bool Pending
    {
        get
        {
            int r = EventSourceGetPending(Handle);
            ThrowIfError(r);
            return r != 0;
        }
    }

    public void Dispose()
    {
        Handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
