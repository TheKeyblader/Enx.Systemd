using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

internal class PropertyUpdater<THandle, TImplementation>(Func<THandle, TImplementation> createFunc)
    where THandle : SafeHandle, new()
    where TImplementation : BaseWrapper<THandle>
{
    private TImplementation? _value;

    public TImplementation? GetValue(THandle actualHandle)
    {
        if (actualHandle.IsInvalid)
        {
            _value?.Dispose();
            _value = null;
            return null;
        }

        if (_value == null)
            return _value = createFunc(actualHandle);

        if (_value.Handle.DangerousGetHandle() == actualHandle.DangerousGetHandle())
            return _value;

        _value.Dispose();
        return _value = createFunc(actualHandle);
    }

    public void SetValue(TImplementation? value)
    {
        _value?.Dispose();
        _value = value;
    }
}

internal class CustomDataPropertyUpdater
{
    private PinnedGCHandle<object?> _customPin;

    public object? Get(nint actualPtr)
    {
        if (!_customPin.IsAllocated)
            return actualPtr != IntPtr.Zero ? new UnknownData(actualPtr) : null;

        nint pinPtr = PinnedGCHandle<object?>.ToIntPtr(_customPin);

        return pinPtr != actualPtr
            ? throw new InvalidOperationException("data change externaly")
            : _customPin.Target;
    }

    public nint Set(object? value, nint oldPtr)
    {
        if (!_customPin.IsAllocated)
        {
            if (oldPtr != IntPtr.Zero)
                throw new InvalidOperationException("data is set externaly");
            if (value == null) return nint.Zero;
            _customPin = new PinnedGCHandle<object?>(value);
            return PinnedGCHandle<object?>.ToIntPtr(_customPin);
        }

        nint pinPtr = PinnedGCHandle<object?>.ToIntPtr(_customPin);
        if (pinPtr != oldPtr) throw new InvalidOperationException("data has been change externaly");
        _customPin.Target = value;
        return pinPtr;
    }
}

public record struct UnknownData(IntPtr Pointer);
