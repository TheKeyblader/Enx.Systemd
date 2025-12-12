using System.Reflection;
using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

public static partial class NativeMethods
{
    private const string Library = "libsystemd";
    private static IntPtr _libraryHandle = IntPtr.Zero;

    static NativeMethods()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, Resolve);
    }

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != Library)
            return IntPtr.Zero;

        if (_libraryHandle != IntPtr.Zero)
            return _libraryHandle;

        foreach (var library in NativeSystemdLibrary.GetNames())
            if (NativeLibrary.TryLoad(library, assembly, searchPath, out _libraryHandle))
                return _libraryHandle;

        throw new DllNotFoundException(
            $"Could not find systemd library tried: {string.Join(", ", NativeSystemdLibrary.GetNames())}");
    }
}