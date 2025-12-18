using System.Runtime.InteropServices;
using EnumsNET;

namespace Enx.Systemd;

public class SystemdException : Exception
{
    public SystemdException(int errno, Exception? innerException = null) : base(GetMessage(-errno), innerException)
    {
        NativeErrorCode = -errno;
    }

    public int NativeErrorCode { get; }

    private static string? GetMessage(int errno) =>
        Marshal.GetPInvokeErrorMessage(errno);
}
