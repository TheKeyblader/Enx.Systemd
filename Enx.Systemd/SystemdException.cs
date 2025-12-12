using EnumsNET;

namespace Enx.Systemd;

public class SystemdException : Exception
{
    public SystemdException(int errno, Exception? innerException = null) : base(GetMessage(errno), innerException)
    {
        NativeErrorCode = (Errno)Math.Abs(errno);
    }

    public Errno NativeErrorCode { get; }

    private static string? GetMessage(int errno) =>
        ((Errno)Math.Abs(errno)).AsString(EnumFormat.Description);
}