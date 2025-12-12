using System.Collections;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

public class DeviceEnumerator : IDisposable
{
    private readonly SdDeviceEnumeratorHandle _handle;
    public SdDeviceEnumeratorHandle Handle => _handle;

    public DeviceEnumerator()
    {
        ThrowIfError(DeviceEnumeratorNew(out _handle));
    }

    public DeviceEnumerator MatchSubsystem(string subsystem, bool match)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSubsystem(_handle, subsystem, match));
        return this;
    }

    public DeviceEnumerator MatchSysAttr(string attr, string value, bool match)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSysattr(_handle, attr, value, match));
        return this;
    }

    public DeviceEnumerator MatchProperty(string property, string value)
    {
        ThrowIfError(DeviceEnumeratorAddMatchProperty(_handle, property, value));
        return this;
    }

    public DeviceEnumerator MatchPropertyRequired(string property, string value)
    {
        ThrowIfError(DeviceEnumeratorAddMatchPropertyRequired(_handle, property, value));
        return this;
    }

    public DeviceEnumerator MatchSysname(string sysname)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSysname(_handle, sysname));
        return this;
    }

    public DeviceEnumerator NoMatchSysname(string sysname)
    {
        ThrowIfError(DeviceEnumeratorAddNoMatchSysname(_handle, sysname));
        return this;
    }

    public DeviceEnumerator MatchTag(string tag)
    {
        ThrowIfError(DeviceEnumeratorAddMatchTag(_handle, tag));
        return this;
    }

    public DeviceEnumerator MatchParent(Device parent)
    {
        ThrowIfError(DeviceEnumeratorAddMatchParent(_handle, parent.Handle));
        return this;
    }

    public DeviceEnumerator AllowUnitialized()
    {
        ThrowIfError(DeviceEnumeratorAllowUnitialized(_handle));
        return this;
    }

    public DeviceEnumerator AddAllParents()
    {
        ThrowIfError(DeviceEnumeratorAddAllParents(_handle));
        return this;
    }

    public IEnumerable<Device> Devices
    {
        get
        {
            field ??= new DeviceEnumerable(_handle, false);
            return field;
        }
    }

    public IEnumerable<Device> SubSystems
    {
        get
        {
            field ??= new DeviceEnumerable(_handle, true);
            return field;
        }
    }

    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal class DeviceEnumerable(SdDeviceEnumeratorHandle handle, bool subsystem)
    : IEnumerable<Device>, IEnumerator<Device>
{
    private Device? _current;
    private bool _started;

    public IEnumerator<Device> GetEnumerator() => this;
    IEnumerator IEnumerable.GetEnumerator() => this;

    public bool MoveNext()
    {
        SdDeviceHandle deviceHandle;
        if (!_started)
        {
            deviceHandle = subsystem
                ? DeviceEnumeratorGetSubsystemFirst(handle)
                : DeviceEnumeratorGetDeviceFirst(handle);
            _started = true;
        }
        else
        {
            deviceHandle = subsystem
                ? DeviceEnumeratorGetSubsystemNext(handle)
                : DeviceEnumeratorGetDeviceNext(handle);
        }

        _current = deviceHandle.IsInvalid ? null : new Device(deviceHandle, handle);

        return !deviceHandle.IsInvalid;
    }

    public void Reset() => _started = false;

    Device IEnumerator<Device>.Current => _current ?? throw new InvalidOperationException();

    object? IEnumerator.Current => _current;

    public void Dispose()
    {
    }
}