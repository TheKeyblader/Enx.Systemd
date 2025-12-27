using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

/// <summary>
/// Represents a systemd device backed by an <c>sd_device</c> handle.
/// Prefer the <c>From*</c> factory methods; the public constructors are intended for advanced scenarios.
/// Properties are fetched lazily and cached; missing values return empty strings.
/// </summary>
public class Device : BaseWrapper<SdDeviceHandle>
{
    /// <summary>
    /// Initializes a device wrapper around an existing handle, optionally retaining a reference
    /// from another device or enumerator handle.
    /// </summary>
    /// <param name="handle">The device handle to wrap.</param>
    /// <param name="shouldRef">Whether to increment the handle reference count.</param>
    public Device(SdDeviceHandle handle, bool shouldRef)
        : base(handle, shouldRef)
    {
    }

    /// <summary>
    /// Increments the native reference count for the underlying device handle.
    /// </summary>
    public override void Ref() => DeviceRef(Handle.DangerousGetHandle());

    /// <summary>
    /// Creates a device from its sysfs path.
    /// </summary>
    /// <param name="syspath">The sysfs path.</param>
    /// <returns>A device instance for the path.</returns>
    public static Device FromSyspath(string syspath)
    {
        ThrowIfError(DeviceNewFromSyspath(out var handle, syspath));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from a subsystem and sysname.
    /// </summary>
    /// <param name="subsystem">The subsystem name.</param>
    /// <param name="sysname">The sysfs name.</param>
    /// <returns>A device instance for the subsystem and sysname.</returns>
    public static Device FromSubsystemSysname(string subsystem, string sysname)
    {
        ThrowIfError(DeviceNewFromSubsystemSysname(out var handle, subsystem, sysname));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from a device ID (e.g. "b8:1").
    /// </summary>
    /// <param name="id">The device ID.</param>
    /// <returns>A device instance for the ID.</returns>
    public static Device FromDeviceId(string id)
    {
        ThrowIfError(DeviceNewFromDeviceId(out var handle, id));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from a device node path.
    /// </summary>
    /// <param name="devname">The device node path (e.g. "/dev/sda").</param>
    /// <returns>A device instance for the device node.</returns>
    public static Device FromDevname(string devname)
    {
        ThrowIfError(DeviceNewFromDevname(out var handle, devname));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from any supported path.
    /// </summary>
    /// <param name="path">The device path.</param>
    /// <returns>A device instance for the path.</returns>
    public static Device FromPath(string path)
    {
        ThrowIfError(DeviceNewFromPath(out var handle, path));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from a network interface name.
    /// </summary>
    /// <param name="ifname">The interface name.</param>
    /// <returns>A device instance for the interface.</returns>
    public static Device FromIfname(string ifname)
    {
        ThrowIfError(DeviceNewFromIfname(out var handle, ifname));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a device from a network interface index.
    /// </summary>
    /// <param name="ifindex">The interface index.</param>
    /// <returns>A device instance for the interface index.</returns>
    public static Device FromIfindex(int ifindex)
    {
        ThrowIfError(DeviceNewFromIfindex(out var handle, ifindex));
        return new Device(handle, false);
    }

    /// <summary>
    /// Creates a child device by appending a suffix to this device's sysfs path.
    /// </summary>
    /// <param name="suffix">The sysfs suffix to append.</param>
    /// <param name="child">The created child device, or null on error.</param>
    /// <returns>The native result code.</returns>
    public int NewChild(string suffix, [NotNullWhen(true)] out Device? child)
    {
        int result = DeviceNewChild(out var childhandle, Handle, suffix);
        child = result < 0 ? null : new Device(childhandle, false);
        return result;
    }

    /// <summary>
    /// Gets the parent device, if available.
    /// </summary>
    [JsonIgnore]
    public Device? Parent
    {
        get
        {
            if (field == null)
            {
                DeviceGetParent(Handle, out var ret);
                field = new Device(ret, true);
            }

            return field.Handle.IsInvalid ? null : field;
        }
    }

    /// <summary>
    /// Gets the first parent that matches the given subsystem and device type.
    /// </summary>
    /// <param name="subsystem">The subsystem to match.</param>
    /// <param name="devtype">The device type to match.</param>
    /// <returns>The matching parent device, or null if none is found.</returns>
    public Device? GetParentWithSubsystemDevType(string subsystem, string devtype)
    {
        ThrowIfError(DeviceGetParentWithSubsystemDevtype(Handle, subsystem, devtype, out var ret));
        return ret.IsInvalid ? null : new Device(ret, false);
    }

    /// <summary>
    /// Gets the sysfs path for this device, or an empty string if not available.
    /// </summary>
    public string Syspath
    {
        get
        {
            if (field == null && DeviceGetSysPath(Handle, out field) < 0)
                field = string.Empty;

            return field;
        }
    }

    /// <summary>
    /// Gets the device subsystem, or an empty string if not available.
    /// </summary>
    public string Subsystem
    {
        get
        {
            if (field == null && DeviceGetSubsystem(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the driver subsystem, or an empty string if not available.
    /// </summary>
    public string DriverSubsystem
    {
        get
        {
            if (field == null && DeviceGetDriverSubsystem(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the device type, or an empty string if not available.
    /// </summary>
    public string Devtype
    {
        get
        {
            if (field == null && DeviceGetDevtype(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the network interface index, or <see cref="int.MaxValue"/> if not available.
    /// </summary>
    public int Ifindex
    {
        get
        {
            if (field == 0 && DeviceGetIfindex(Handle, out field) < 0)
                field = int.MaxValue;
            return field;
        }
    }

    /// <summary>
    /// Gets the bound driver, or an empty string if not available.
    /// </summary>
    public string Driver
    {
        get
        {
            if (field == null && DeviceGetDriver(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the device path relative to sysfs, or an empty string if not available.
    /// </summary>
    public string Devpath
    {
        get
        {
            if (field == null && DeviceGetDevpath(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the device node path, or an empty string if not available.
    /// </summary>
    public string Devname
    {
        get
        {
            if (field == null && DeviceGetDevname(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the sysfs name, or an empty string if not available.
    /// </summary>
    public string Sysname
    {
        get
        {
            if (field == null && DeviceGetSysname(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the sysfs number, or an empty string if not available.
    /// </summary>
    public string Sysnum
    {
        get
        {
            if (field == null && DeviceGetSysnum(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    public string DeviceId
    {
        get
        {
            if (field == null && DeviceGetDeviceId(Handle, out field) < 0)
                field = string.Empty;
            return field;
        }
    }

    /// <summary>
    /// Gets the last reported action for the device, or null if not available.
    /// </summary>
    public DeviceAction? Action
    {
        get
        {
            int r = DeviceGetAction(Handle, out var ret);
            return r == Errno.ENOENT ? null : ret;
        }
    }

    /// <summary>
    /// Gets the tags associated with this device.
    /// </summary>
    public IReadOnlyList<string> Tags => field ??= BuildTags();

    private ImmutableList<string> BuildTags()
    {
        var builder = ImmutableList.CreateBuilder<string>();
        for (string? tag = DeviceGetTagFirst(Handle);
             tag != null;
             tag = DeviceGetTagNext(Handle))
        {
            builder.Add(tag);
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Gets the device links (symlinks) for this device.
    /// </summary>
    public IReadOnlyList<string> Devlinks => field ??= BuildDevlinks();

    private ImmutableList<string> BuildDevlinks()
    {
        var builder = ImmutableList.CreateBuilder<string>();
        for (string? devlink = DeviceGetDevlinkFirst(Handle);
             devlink != null;
             devlink = DeviceGetDevlinkNext(Handle))
        {
            builder.Add(devlink);
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Gets the device properties.
    /// </summary>
    public IReadOnlyDictionary<string, string> Properties => field ??= BuildProperties();

    private ImmutableDictionary<string, string> BuildProperties()
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string>();
        for (string? property = DeviceGetPropertyFirst(Handle, out string value);
             property != null;
             property = DeviceGetPropertyNext(Handle, out value))
        {
            builder.Add(property, value);
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Gets the sysfs attributes and their values.
    /// </summary>
    public IReadOnlyDictionary<string, string> SystemAttributes => field ??= BuildSystemAttributes();

    private ImmutableDictionary<string, string> BuildSystemAttributes()
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string>();
        for (string? sysattr = DeviceGetSysattrFirst(Handle);
             sysattr != null;
             sysattr = DeviceGetSysattrNext(Handle))
        {
            if (DeviceGetSysattrValue(Handle, sysattr, out string value) < 0)
                value = string.Empty;
            builder.Add(sysattr, value);
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Enumerates direct child devices.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<Device> Childrens => new DeviceChildEnumerable(Handle);
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
            _current = new Device(childhandle, false);

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
