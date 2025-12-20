using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdEvent;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Events;

/// <summary>
/// Base type for event sources that are not exit sources.
/// </summary>
/// <param name="handle">The event source handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public abstract class EventSourceNotExit(SdEventSourceHandle handle, bool shouldRef) : EventSource(handle, shouldRef)
{
    #region Prepare

    private Func<EventSourceNotExit, object?, int>? _prepareFunc;

    /// <summary>
    /// Gets or sets the prepare callback executed before dispatch.
    /// Set to null to unregister the callback.
    /// </summary>
    public Func<EventSourceNotExit, object?, int>? Prepare
    {
        get => _prepareFunc;
        set
        {
            if (value == null)
            {
                _prepareFunc = null;
                ThrowIfError(EventSourceSetPrepare(Handle, null));
            }

            if (_prepareFunc == null)
                ThrowIfError(EventSourceSetPrepare(Handle, PrepareHandle));

            _prepareFunc = value;
        }
    }

    private int PrepareHandle(nint eventSourcePtr, nint userdata)
    {
        if (eventSourcePtr != Handle.DangerousGetHandle())
            throw new InvalidOperationException("Invalid expected handle");
        if (userdata != EventSourceGetUserdata(Handle))
            throw new InvalidOperationException("Invalid userdata");
        if (_prepareFunc is null)
            throw new InvalidOperationException("Invalid pepare func to call");

        return _prepareFunc(this, UserData);
    }

    #endregion

    /// <summary>
    /// Gets a value indicating whether the source is pending.
    /// </summary>
    public bool Pending => EventSourceGetPending(Handle);

    /// <summary>
    /// Gets or sets whether the loop should exit on callback failure.
    /// </summary>
    public bool ExitOnFailure
    {
        get => EventSourceGetExitOnFailure(Handle);
        set => ThrowIfError(EventSourceSetExitOnFailure(Handle, value));
    }
}
