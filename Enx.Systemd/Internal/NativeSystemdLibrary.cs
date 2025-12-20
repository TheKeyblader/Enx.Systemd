using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

/// <summary>
/// Provides OS-specific library name candidates for systemd native bindings.
/// </summary>
public static class NativeSystemdLibrary
{
    /// <summary>
    /// Gets the list of candidate library names for the current platform.
    /// </summary>
    /// <returns>Library name candidates.</returns>
    public static IEnumerable<string> GetNames()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxNames();

        throw new PlatformNotSupportedException("Unsupported operating system");
    }

    /// <summary>
    /// Returns Linux library name candidates.
    /// </summary>
    private static IEnumerable<string> GetLinuxNames()
    {
        yield return "libsystemd.so";
    }
}
