using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

public static class NativeSystemdLibrary
{
    public static IEnumerable<string> GetNames()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxNames();

        throw new PlatformNotSupportedException("Unsupported operating system");
    }

    private static IEnumerable<string> GetLinuxNames()
    {
        yield return "libsystemd.so";
    }
}