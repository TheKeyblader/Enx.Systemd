using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

/// <summary>
/// Provides native systemd bindings and library resolution helpers.
/// </summary>
public static partial class NativeMethods
{
    private const string Library = "libsystemd";
    private static IntPtr s_libraryHandle = IntPtr.Zero;

    /// <summary>
    /// Installs the library resolver for systemd bindings.
    /// </summary>
    static NativeMethods()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, Resolve);
    }

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != Library)
            return IntPtr.Zero;

        if (s_libraryHandle != IntPtr.Zero)
            return s_libraryHandle;

        foreach (string library in NativeSystemdLibrary.GetNames())
            if (NativeLibrary.TryLoad(library, assembly, searchPath, out s_libraryHandle))
                return s_libraryHandle;

        throw new DllNotFoundException(
            $"Could not find systemd library tried: {string.Join(", ", NativeSystemdLibrary.GetNames())}");
    }

    private static readonly Lazy<int> s_version = new Lazy<int>(GetVersion);

    /// <summary>
    /// Gets the systemd version reported by <c>systemctl --version</c>.
    /// </summary>
    public static int Version => s_version.Value;

    private static int GetVersion()
    {
        string shell = Environment.GetEnvironmentVariable("SHELL")!;
        var process = new Process();
        process.StartInfo.FileName = shell;
        process.StartInfo.ArgumentList.Add("-c");
        process.StartInfo.ArgumentList.Add(
            "systemctl --version | awk '{if($1==\"systemd\" && $2~\"^[0-9]\"){print $2}}' | head -n 1");
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        string str = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return int.Parse(str);
    }
}
