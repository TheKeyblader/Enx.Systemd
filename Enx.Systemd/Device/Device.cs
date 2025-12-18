using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

public class Device : IDisposable
{
    private readonly SdDeviceHandle _refByHandle = new();
    private readonly SdDeviceEnumeratorHandle _enumerateByHandle = new();

    private Device(SdDeviceHandle handle, SdDeviceHandle? refByHandle, SdDeviceEnumeratorHandle? enumerateByHandle)
    {
        Handle = handle;
        if (refByHandle != null)
        {
            Marshal.InitHandle(_refByHandle, refByHandle.DangerousGetHandle());
            DeviceRef(_refByHandle.DangerousGetHandle());
        }

        if (enumerateByHandle != null)
        {
            Marshal.InitHandle(_enumerateByHandle, enumerateByHandle.DangerousGetHandle());
            DeviceEnumeratorRef(_enumerateByHandle.DangerousGetHandle());
        }
    }

    public Device(SdDeviceHandle handle) : this(handle, null, null)
    {
    }

    public Device(SdDeviceHandle handle, SdDeviceHandle refByHandle) : this(handle, refByHandle, null)
    {
    }

    public Device(SdDeviceHandle handle, SdDeviceEnumeratorHandle enumerateByHandle) : this(handle, null,
        enumerateByHandle)
    {
    }

    [JsonIgnore] public SdDeviceHandle Handle { get; }

    public static Device FromSyspath(string syspath)
    {
        ThrowIfError(DeviceNewFromSyspath(out var handle, syspath));
        return new Device(handle);
    }

    public static Device FromSubsystemSysname(string subsystem, string sysname)
    {
        ThrowIfError(DeviceNewFromSubsystemSysname(out var handle, subsystem, sysname));
        return new Device(handle);
    }

    public static Device FromDeviceId(string id)
    {
        ThrowIfError(DeviceNewFromDeviceId(out var handle, id));
        return new Device(handle);
    }

    public static Device FromDevname(string devname)
    {
        ThrowIfError(DeviceNewFromDevname(out var handle, devname));
        return new Device(handle);
    }

    public static Device FromPath(string path)
    {
        ThrowIfError(DeviceNewFromPath(out var handle, path));
        return new Device(handle);
    }

    public static Device FromIfname(string ifname)
    {
        ThrowIfError(DeviceNewFromIfname(out var handle, ifname));
        return new Device(handle);
    }

    public static Device FromIfindex(int ifindex)
    {
        ThrowIfError(DeviceNewFromIfindex(out var handle, ifindex));
        return new Device(handle);
    }

    public int NewChild(string suffix, [NotNullWhen(true)] out Device? child)
    {
        var result = DeviceNewChild(out var childhandle, Handle, suffix);
        child = result < 0 ? null : new Device(childhandle, Handle);
        return result;
    }

    [JsonIgnore]
    public Device? Parent
    {
        get
        {
            if (field == null)
            {
                DeviceGetParent(Handle, out var ret);
                field = new Device(ret, Handle);
            }

            return field.Handle.IsInvalid ? null : field;
        }
    }

    public Device? GetParentWithSubsystemDevType(string subsystem, string devtype)
    {
        ThrowIfError(DeviceGetParentWithSubsystemDevtype(Handle, subsystem, devtype, out var ret));
        return ret.IsInvalid ? null : new Device(ret);
    }

    public string Syspath
    {
        get
        {
            if (field == null && DeviceGetSysPath(Handle, out field) < 0)
                field = string.Empty;

            return field;
        }
    }

    public string Subsystem
    {
        get
        {
            if (field == null && DeviceGetSubsystem(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string DriverSubsystem
    {
        get
        {
            if (field == null && DeviceGetDriverSubsystem(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string Devtype
    {
        get
        {
            if (field == null && DeviceGetDevtype(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public int Ifindex
    {
        get
        {
            if (field == 0 && DeviceGetIfindex(Handle, out field) < 0)
                field = int.MaxValue;
            return field;
        }
    }

    public string Driver
    {
        get
        {
            if (field == null && DeviceGetDriver(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string Devpath
    {
        get
        {
            if (field == null && DeviceGetDevpath(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string Devname
    {
        get
        {
            if (field == null && DeviceGetDevname(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string Sysname
    {
        get
        {
            if (field == null && DeviceGetSysname(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string Sysnum
    {
        get
        {
            if (field == null && DeviceGetSysnum(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public IReadOnlyList<string> Tags => field ??= BuildTags();

    private ImmutableList<string> BuildTags()
    {
        var builder = ImmutableList.CreateBuilder<string>();
        for (var tag = DeviceGetTagFirst(Handle);
             tag != null;
             tag = DeviceGetTagNext(Handle))
        {
            builder.Add(tag);
        }

        return builder.ToImmutable();
    }

    public IReadOnlyList<string> Devlinks => field ??= BuildDevlinks();

    private ImmutableList<string> BuildDevlinks()
    {
        var builder = ImmutableList.CreateBuilder<string>();
        for (var devlink = DeviceGetDevlinkFirst(Handle);
             devlink != null;
             devlink = DeviceGetDevlinkNext(Handle))
        {
            builder.Add(devlink);
        }

        return builder.ToImmutable();
    }

    public IReadOnlyDictionary<string, string> Properties => field ??= BuildProperties();

    private ImmutableDictionary<string, string> BuildProperties()
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string>();
        for (var property = DeviceGetPropertyFirst(Handle, out var value);
             property != null;
             property = DeviceGetPropertyNext(Handle, out value))
        {
            builder.Add(property, value);
        }

        return builder.ToImmutable();
    }

    public IReadOnlyDictionary<string, string> SystemAttributes => field ??= BuildSystemAttributes();

    private ImmutableDictionary<string, string> BuildSystemAttributes()
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string>();
        for (var sysattr = DeviceGetSysattrFirst(Handle);
             sysattr != null;
             sysattr = DeviceGetSysattrNext(Handle))
        {
            if (DeviceGetSysattrValue(Handle, sysattr, out var value) < 0)
                value = string.Empty;
            builder.Add(sysattr, value);
        }

        return builder.ToImmutable();
    }

    [JsonIgnore] public IEnumerable<Device> Childrens => field ??= new DeviceChildEnumerable(Handle);

    public void Dispose()
    {
        Handle.Dispose();
        _refByHandle.Dispose();
        _enumerateByHandle.Dispose();
        GC.SuppressFinalize(this);
    }
}

internal class DeviceChildEnumerable(SdDeviceHandle handle) : IEnumerable<Device>, IEnumerator<Device>
{
    private Device? _current;
    private bool _started;

    public IEnumerator<Device> GetEnumerator() => this;

    IEnumerator IEnumerable.GetEnumerator() => this;

    public bool MoveNext()
    {
        SdDeviceHandle childhandle;
        if (!_started)
        {
            _started = true;
            childhandle = DeviceGetChildFirst(handle, null);
        }
        else childhandle = DeviceGetChildNext(handle, null);

        if (!childhandle.IsInvalid)
            _current = new Device(childhandle, handle);

        return !childhandle.IsInvalid;
    }

    public void Reset() => _started = false;

    Device IEnumerator<Device>.Current => _current ?? throw new InvalidOperationException();

    object? IEnumerator.Current => _current;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
