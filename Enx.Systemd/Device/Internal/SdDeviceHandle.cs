using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public class SdDeviceHandle() : SafeHandle(IntPtr.Zero, true)
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.SdDevice.DeviceUnref(handle);
        return true;
    }
}