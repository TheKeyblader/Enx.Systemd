using System.Collections;
using Enx.Systemd.Internal;
using static Enx.Systemd.Internal.NativeMethods.SdDevice;
using static Enx.Systemd.SystemdUtils;

namespace Enx.Systemd.Device;

/// <summary>
/// Provides enumeration and filtering over systemd devices and subsystems.
/// Prefer <see cref="Create"/>; the primary constructor is intended for advanced scenarios.
/// </summary>
/// <param name="handle">The enumerator handle to wrap.</param>
/// <param name="shouldRef">Whether to increment the handle reference count.</param>
public class DeviceEnumerator(SdDeviceEnumeratorHandle handle, bool shouldRef)
    : BaseWrapper<SdDeviceEnumeratorHandle>(handle, shouldRef)
{
    /// <summary>
    /// Increments the native reference count for the underlying enumerator handle.
    /// </summary>
    public override void Ref() => DeviceEnumeratorRef(Handle.DangerousGetHandle());

    /// <summary>
    /// Creates a new device enumerator.
    /// This is the recommended creation path; use the constructor only if you know what you are doing.
    /// </summary>
    /// <returns>The created enumerator.</returns>
    public static DeviceEnumerator Create()
    {
        ThrowIfError(DeviceEnumeratorNew(out var handle));
        return new DeviceEnumerator(handle, false);
    }

    /// <summary>
    /// Adds a subsystem filter.
    /// </summary>
    /// <param name="subsystem">The subsystem name.</param>
    /// <param name="match">True to include, false to exclude.</param>
    public DeviceEnumerator MatchSubsystem(string subsystem, bool match)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSubsystem(Handle, subsystem, match));
        return this;
    }

    /// <summary>
    /// Adds a sysfs attribute filter.
    /// </summary>
    /// <param name="attr">The sysfs attribute name.</param>
    /// <param name="value">The attribute value to match.</param>
    /// <param name="match">True to include, false to exclude.</param>
    public DeviceEnumerator MatchSysAttr(string attr, string value, bool match)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSysattr(Handle, attr, value, match));
        return this;
    }

    /// <summary>
    /// Adds a property filter.
    /// </summary>
    /// <param name="property">The property key.</param>
    /// <param name="value">The property value to match.</param>
    public DeviceEnumerator MatchProperty(string property, string value)
    {
        ThrowIfError(DeviceEnumeratorAddMatchProperty(Handle, property, value));
        return this;
    }

    /// <summary>
    /// Adds a required property filter.
    /// </summary>
    /// <param name="property">The property key.</param>
    /// <param name="value">The property value to match.</param>
    public DeviceEnumerator MatchPropertyRequired(string property, string value)
    {
        ThrowIfError(DeviceEnumeratorAddMatchPropertyRequired(Handle, property, value));
        return this;
    }

    /// <summary>
    /// Adds a sysname include filter.
    /// </summary>
    /// <param name="sysname">The sysfs name to include.</param>
    public DeviceEnumerator MatchSysname(string sysname)
    {
        ThrowIfError(DeviceEnumeratorAddMatchSysname(Handle, sysname));
        return this;
    }

    /// <summary>
    /// Adds a sysname exclusion filter.
    /// </summary>
    /// <param name="sysname">The sysfs name to exclude.</param>
    public DeviceEnumerator NoMatchSysname(string sysname)
    {
        ThrowIfError(DeviceEnumeratorAddNoMatchSysname(Handle, sysname));
        return this;
    }

    /// <summary>
    /// Adds a tag filter.
    /// </summary>
    /// <param name="tag">The tag to match.</param>
    public DeviceEnumerator MatchTag(string tag)
    {
        ThrowIfError(DeviceEnumeratorAddMatchTag(Handle, tag));
        return this;
    }

    /// <summary>
    /// Adds a parent filter.
    /// </summary>
    /// <param name="parent">The parent device to match.</param>
    public DeviceEnumerator MatchParent(Device parent)
    {
        ThrowIfError(DeviceEnumeratorAddMatchParent(Handle, parent.Handle));
        return this;
    }

    /// <summary>
    /// Allows uninitialized devices to be returned.
    /// </summary>
    public DeviceEnumerator AllowUnitialized()
    {
        ThrowIfError(DeviceEnumeratorAllowUnitialized(Handle));
        return this;
    }

    /// <summary>
    /// Includes all parent devices in the enumeration results.
    /// </summary>
    public DeviceEnumerator AddAllParents()
    {
        ThrowIfError(DeviceEnumeratorAddAllParents(Handle));
        return this;
    }

    /// <summary>
    /// Enumerates devices that match the current filters.
    /// </summary>
    public IEnumerable<Device> Devices
    {
        get
        {
            field ??= new DeviceEnumerable(Handle, false);
            return field;
        }
    }

    /// <summary>
    /// Enumerates matching subsystems.
    /// </summary>
    public IEnumerable<Device> SubSystems
    {
        get
        {
            field ??= new DeviceEnumerable(Handle, true);
            return field;
        }
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

        _current = deviceHandle.IsInvalid ? null : new Device(deviceHandle, true);

        return !deviceHandle.IsInvalid;
    }

    public void Reset() => _started = false;

    Device IEnumerator<Device>.Current => _current ?? throw new InvalidOperationException();

    object? IEnumerator.Current => _current;

    public void Dispose()
    {
    }
}
