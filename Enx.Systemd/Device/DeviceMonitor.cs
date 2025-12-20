using System.Runtime.InteropServices;
using Enx.Systemd.Events;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

/// <summary>
/// Monitors udev device events through systemd's <c>sd-device</c> monitor API.
/// Prefer <see cref="Create"/>; the primary constructor is intended for advanced scenarios.
/// </summary>
/// <param name="handle">The monitor handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public class DeviceMonitor(SdDeviceMonitorHandle handle, bool shouldRef = true)
    : BaseWrapper<SdDeviceMonitorHandle>(handle, shouldRef)
{
    private PinnedGCHandle<object?> _userdataPin;

    /// <summary>
    /// Increments the native reference count for the underlying monitor handle.
    /// </summary>
    public override void Ref() => DeviceMonitorRef(Handle.DangerousGetHandle());

    /// <summary>
    /// Creates a new device monitor instance.
    /// This is the recommended creation path.
    /// </summary>
    /// <returns>The created monitor.</returns>
    public static DeviceMonitor Create()
    {
        ThrowIfError(DeviceMonitorNew(out var handle));
        return new DeviceMonitor(handle, false);
    }

    private readonly PropertyUpdater<SdEventHandle, Event> _event =
        new(h => new Event(h, true));

    /// <summary>
    /// Gets or sets the event loop used by the monitor.
    /// Setting null detaches the current event loop.
    /// </summary>
    public Event? Event
    {
        get
        {
            var r = DeviceMonitorGetEvent(Handle);
            return _event.GetValue(r);
        }
        set
        {
            ThrowIfError(DeviceMonitorDetachEvent(Handle));
            if (value is not null)
                ThrowIfError(DeviceMonitorAttachEvent(Handle, value.Handle));
            _event.SetValue(value);
        }
    }

    private readonly PropertyUpdater<SdEventSourceHandle, EventSourceIO> _eventSource =
        new(h => new EventSourceIO(h, true));

    /// <summary>
    /// Gets the event source created by the monitor when attached to an event loop.
    /// </summary>
    public EventSourceIO? EventSource
    {
        get
        {
            SdEventSourceHandle r = DeviceMonitorGetEventSource(Handle);
            return _eventSource.GetValue(r);
        }
    }

    /// <summary>
    /// Gets or sets the monitor description string.
    /// </summary>
    public string Description
    {
        get
        {
            ThrowIfError(DeviceMonitorGetDescription(Handle, out string description));
            return description;
        }
        set { ThrowIfError(DeviceMonitorSetDescription(Handle, value)); }
    }

    /// <summary>
    /// Gets a value indicating whether the monitor is running.
    /// </summary>
    public bool IsRunning => DeviceMonitorIsRunning(Handle);

    /// <summary>
    /// Attaches the monitor to the default event loop.
    /// </summary>
    public void SetDefaultEvent()
    {
        ThrowIfError(DeviceMonitorDetachEvent(Handle));
        ThrowIfError(DeviceMonitorAttachEvent(Handle, null!));
    }

    /// <summary>
    /// Adds a subsystem and devtype filter.
    /// </summary>
    /// <param name="subsystem">The subsystem name.</param>
    /// <param name="devtype">The device type to match.</param>
    public void MatchSubsystem(string subsystem, string devtype)
    {
        ThrowIfError(DeviceMonitorFilterAddMatchSubsystemDevType(Handle, subsystem, devtype));
    }

    /// <summary>
    /// Adds a tag filter.
    /// </summary>
    /// <param name="tag">The tag to match.</param>
    public void MatchTag(string tag)
    {
        ThrowIfError(DeviceMonitorAddMatchTag(Handle, tag));
    }

    /// <summary>
    /// Adds a sysfs attribute filter.
    /// </summary>
    /// <param name="sysattr">The sysfs attribute name.</param>
    /// <param name="value">The attribute value to match.</param>
    /// <param name="match">True to include, false to exclude.</param>
    public void MatchSysattr(string sysattr, string value, bool match)
    {
        ThrowIfError(DeviceMonitorAddMatchSysattr(Handle, sysattr, value, match));
    }

    /// <summary>
    /// Adds a parent device filter.
    /// </summary>
    /// <param name="device">The parent device to match.</param>
    /// <param name="match">True to include, false to exclude.</param>
    public void MatchParent(Device device, bool match)
    {
        ThrowIfError(DeviceMonitorAddMatchParent(Handle, device.Handle, match));
    }

    /// <summary>
    /// Applies the current set of filters.
    /// </summary>
    public void UpdateFilters()
    {
        ThrowIfError(DeviceMonitorFilterUpdate(Handle));
    }

    /// <summary>
    /// Removes all active filters.
    /// </summary>
    public void RemoveFilters()
    {
        ThrowIfError(DeviceMonitorFilterRemove(Handle));
    }

    /// <summary>
    /// Raised when a device event is received.
    /// </summary>
    public event EventHandler<DeviceEventArgs>? OnEvent;

    private int Handler(nint monitorPtr, nint devicePtr, nint userdataPtr)
    {
        if (monitorPtr != Handle.DangerousGetHandle()) throw new InvalidOperationException("Invalid expected handle");

        var deviceHandle = new SdDeviceHandle();
        Marshal.InitHandle(deviceHandle, devicePtr);
        using var device = new Device(deviceHandle, true);

        object? userdata = null;
        if (_userdataPin.IsAllocated && PinnedGCHandle<object?>.ToIntPtr(_userdataPin) == userdataPtr)
        {
            userdata = _userdataPin.Target;
        }

        var eventArgs = new DeviceEventArgs(device, userdata);
        OnEvent?.Invoke(this, eventArgs);
        return 0;
    }

    /// <summary>
    /// Starts monitoring for device events.
    /// </summary>
    /// <param name="userdata">User data associated with the monitor callback.</param>
    public void Start(object? userdata = null)
    {
        IntPtr userdataPtr = IntPtr.Zero;
        _userdataPin = default;
        if (userdata != null)
        {
            _userdataPin = new PinnedGCHandle<object?>(userdata);
            userdataPtr = PinnedGCHandle<object?>.ToIntPtr(_userdataPin);
        }

        ThrowIfError(DeviceMonitorStart(Handle, Handler, userdataPtr));
    }

    /// <summary>
    /// Stops monitoring for device events.
    /// </summary>
    public void Stop()
    {
        ThrowIfError(DeviceMonitorStop(Handle));
    }
}

/// <summary>
/// Event arguments for device monitor callbacks.
/// </summary>
public class DeviceEventArgs(Device device, object? userdata) : EventArgs
{
    /// <summary>
    /// <remarks>Disposed at end of event</remarks>
    /// </summary>
    public Device Device { get; } = device;

    public object? Userdata { get; } = userdata;
    public int ExitCode { get; set; }
}
