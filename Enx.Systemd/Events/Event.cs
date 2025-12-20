using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

/// <summary>
/// Wraps a systemd <c>sd_event</c> loop instance.
/// Prefer <see cref="CreateDefault"/>; the primary constructor is intended for advanced scenarios.
/// </summary>
/// <param name="handle">The event handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public class Event(SdEventHandle handle, bool shouldRef) : BaseWrapper<SdEventHandle>(handle, shouldRef)
{
    /// <summary>
    /// Increments the native reference count for the underlying event handle.
    /// </summary>
    public override void Ref() => EventRef(Handle.DangerousGetHandle());

    /// <summary>
    /// Creates the default event loop for the current process.
    /// </summary>
    /// <returns>The created event loop.</returns>
    public static Event CreateDefault()
    {
        ThrowIfError(EventDefault(out var ret));
        return new Event(ret, false);
    }

    /// <summary>
    /// Gets the file descriptor associated with the event loop.
    /// </summary>
    public int Fd => EventGetFd(Handle);

    /// <summary>
    /// Gets the current state of the event loop.
    /// </summary>
    public EventState State
    {
        get
        {
            int r = EventGetState(Handle);
            ThrowIfError(r);
            return (EventState)r;
        }
    }

    /// <summary>
    /// Gets the thread ID of the event loop owner.
    /// </summary>
    public int Tid
    {
        get
        {
            ThrowIfError(EventGetTid(Handle, out int tid));
            return tid;
        }
    }

    /// <summary>
    /// Gets the exit code if the loop is exiting, or null otherwise.
    /// </summary>
    public int? ExitCode
    {
        get
        {
            int r = EventGetExitCode(Handle, out int code);
            if (r == Errno.ENODATA) return null;
            return code;
        }
    }

    /// <summary>
    /// Gets or sets the watchdog state.
    /// </summary>
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

    /// <summary>
    /// Gets the loop iteration counter.
    /// </summary>
    public ulong Iteration
    {
        get
        {
            ThrowIfError(EventGetIteration(Handle, out ulong ret));
            return ret;
        }
    }

    /// <summary>
    /// Enables or disables automatic exit on termination signals.
    /// </summary>
    public bool SignalExit
    {
        set => ThrowIfError(EventSetSignalExit(Handle, value ? 1 : 0));
    }

    /// <summary>
    /// Prepares the event loop for dispatch.
    /// </summary>
    public void Prepare()
    {
        ThrowIfError(EventPrepare(Handle));
    }

    /// <summary>
    /// Waits for events, with a timeout in microseconds.
    /// </summary>
    public void Wait(ulong usec)
    {
        ThrowIfError(EventWait(Handle, usec));
    }

    /// <summary>
    /// Dispatches pending events.
    /// </summary>
    public void Dispatch()
    {
        ThrowIfError(EventDispatch(Handle));
    }

    /// <summary>
    /// Runs the event loop for one iteration with a timeout in microseconds.
    /// </summary>
    public void Run(ulong usec)
    {
        ThrowIfError(EventRun(Handle, usec));
    }

    /// <summary>
    /// Runs the event loop until it exits.
    /// </summary>
    public void Loop()
    {
        ThrowIfError(EventLoop(Handle));
    }

    /// <summary>
    /// Requests the event loop to exit with the specified code.
    /// </summary>
    public int Exit(int code)
    {
        return EventExit(Handle, code);
    }
}
