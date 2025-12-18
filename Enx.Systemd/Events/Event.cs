using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

public class Event : IDisposable
{
    public SdEventHandle Handle { get; }

    public static Event Default
    {
        get
        {
            ThrowIfError(EventDefault(out var ret));
            return new Event(ret);
        }
    }

    public Event(SdEventHandle handle)
    {
        Handle = handle;
    }

    public int Fd => EventGetFd(Handle);

    public EventState State
    {
        get
        {
            int r = EventGetState(Handle);
            ThrowIfError(r);
            return (EventState)r;
        }
    }

    public int Tid
    {
        get
        {
            ThrowIfError(EventGetTid(Handle, out int tid));
            return tid;
        }
    }

    public int? ExitCode
    {
        get
        {
            int r = EventGetExitCode(Handle, out int code);
            if (r == Errno.ENODATA) return null;
            return code;
        }
    }

    public bool Watchdog
    {
        get
        {
            int r = EventGetWatchdog(Handle);
            ThrowIfError(r);
            return r != 0;
        }
        set => ThrowIfError(EventSetWatchdog(Handle, value ? 1 : 0));
    }

    public ulong Iteration
    {
        get
        {
            ThrowIfError(EventGetIteration(Handle, out ulong ret));
            return ret;
        }
    }

    public bool SignalExit
    {
        set => ThrowIfError(EventSetSignalExit(Handle, value ? 1 : 0));
    }

    public void Prepare()
    {
        ThrowIfError(EventPrepare(Handle));
    }

    public void Wait(ulong usec)
    {
        ThrowIfError(EventWait(Handle, usec));
    }

    public void Dispatch()
    {
        ThrowIfError(EventDispatch(Handle));
    }

    public void Run(ulong usec)
    {
        ThrowIfError(EventRun(Handle, usec));
    }

    public void Loop()
    {
        ThrowIfError(EventLoop(Handle));
    }

    public int Exit(int code)
    {
        return EventExit(Handle, code);
    }

    public void Dispose()
    {
        Handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
