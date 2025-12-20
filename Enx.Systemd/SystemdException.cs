using System.Runtime.InteropServices;
namespace Enx.Systemd;

/// <summary>
/// Exception type for native systemd errors.
/// </summary>
public class SystemdException : Exception
{
    /// <summary>
    /// Creates a new exception using a systemd errno value.
    /// </summary>
    /// <param name="errno">The native errno value.</param>
    /// <param name="innerException">An optional inner exception.</param>
    public SystemdException(int errno, Exception? innerException = null) : base(GetMessage(-errno), innerException)
    {
        NativeErrorCode = -errno;
    }

    /// <summary>
    /// Gets the native error code (negative errno).
    /// </summary>
    public int NativeErrorCode { get; }

    /// <summary>
    /// Formats a native error message from errno.
    /// </summary>
    private static string? GetMessage(int errno) =>
        Marshal.GetPInvokeErrorMessage(errno);
}
