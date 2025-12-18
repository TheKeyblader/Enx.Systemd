using System.Runtime.InteropServices;
using Enx.Systemd.Events;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

public class DeviceMonitor
{
    private readonly SdDeviceMonitorHandle _handle;
    public SdDeviceMonitorHandle Handle => _handle;

    public DeviceMonitor()
    {
        ThrowIfError(DeviceMonitorNew(out _handle));
    }

    public Event? Event
    {
        get
        {
            var r = DeviceMonitorGetEvent(_handle);
            if (r.IsInvalid) field = null;
            else if (field is null)
                field = new Event(r);
            else if (!field.Handle.Equals(r))
            {
                field.Dispose();
                field = new Event(r);
            }

            return field;
        }
        set
        {
            field = value;
            if (value is null) ThrowIfError(DeviceMonitorDetachEvent(Handle));
            else ThrowIfError(DeviceMonitorAttachEvent(Handle, value.Handle));
        }
    }

    public void SetDefaultEvent()
    {
        ThrowIfError(DeviceMonitorAttachEvent(Handle, null!));
    }

    private int Handler(nint m, nint device, nint userdata)
    {
        if (m != this._handle.DangerousGetHandle()) throw new InvalidOperationException();
        var handle = new SdDeviceHandle();
        Marshal.InitHandle(handle, device);
        var d = new Device(handle);


        return 0;
    }

    public void Start()
    {
        ThrowIfError(DeviceMonitorStart(_handle, Handler, IntPtr.Zero));
    }
}
