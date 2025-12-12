using System.Runtime.CompilerServices;

namespace Enx.Systemd;

public static class SystemdUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfError(int errorCode)
    {
        if (errorCode < 0)
            throw new SystemdException(errorCode);
    }
}