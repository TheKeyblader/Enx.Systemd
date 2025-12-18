using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public class SdEventSourceHandle() : SafeHandle(IntPtr.Zero, true)
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.SdEvent.EventSourceDisableUnref(handle);
        return true;
    }
}
