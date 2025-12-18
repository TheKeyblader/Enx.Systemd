using System.Runtime.InteropServices;


// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public sealed class SdEventHandle() : SafeHandle(IntPtr.Zero, true), IEquatable<SdEventHandle>
{
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.SdEvent.EventUnref(handle);
        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as SdEventHandle);

    public bool Equals(SdEventHandle? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return handle == other.handle;
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => HashCode.Combine(handle);
}
