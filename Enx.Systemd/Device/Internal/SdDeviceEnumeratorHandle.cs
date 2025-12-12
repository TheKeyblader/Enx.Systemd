using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public class SdDeviceEnumeratorHandle() : SafeHandle(IntPtr.Zero, true)
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.SdDevice.DeviceEnumeratorUnref(handle);
        return true;
    }
}