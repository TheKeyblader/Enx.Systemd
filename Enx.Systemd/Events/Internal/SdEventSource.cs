using System.Runtime.InteropServices;
using Enx.Systemd.Events;

namespace Enx.Systemd.Internal;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct SdEventSource
{
    public int wakeup;
    public uint n_ref;
    public nint @event;
    public void* userdata;
    public nint prepare;
    public char* description;
    public EventSourceType type;

    public static unsafe EventSourceType GetType(SdEventSourceHandle handle)
    {
        var s = (SdEventSource*)handle.DangerousGetHandle();
        return s->type;
    }
}
