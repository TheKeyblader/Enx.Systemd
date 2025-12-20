using System.Runtime.InteropServices.Marshalling;

namespace Enx.Systemd.Internal;

[CustomMarshaller(typeof(bool), MarshalMode.Default, typeof(BoolMarshaller))]
public static class BoolMarshaller
{
    public static int ConvertToUnmanaged(bool managed)
    {
        return managed ? 1 : 0;
    }

    public static bool ConvertToManaged(int unmanaged)
        => unmanaged < 0 ? throw new SystemdException(unmanaged) : unmanaged != 0;
}
