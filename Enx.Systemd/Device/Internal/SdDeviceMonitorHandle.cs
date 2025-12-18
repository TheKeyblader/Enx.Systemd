using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public class SdDeviceMonitorHandle() : SafeHandle(IntPtr.Zero, true)
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.SdDevice.DeviceMonitorUnref(handle);
        return true;
    }
}
