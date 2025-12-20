using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

/// <summary>
/// Safe handle wrapper for <c>sd_event_source</c>.
/// </summary>
public class SdEventSourceHandle() : SafeHandle(IntPtr.Zero, true)
{
    /// <summary>
    /// Gets a value indicating whether the handle is invalid.
    /// </summary>
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// Releases the native handle.
    /// </summary>
    protected override bool ReleaseHandle()
    {
        NativeMethods.SdEvent.EventSourceDisableUnref(handle);
        return true;
    }
}
