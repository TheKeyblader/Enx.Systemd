using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Enx.Systemd;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Errno
{
    public const int OK = 0;

    /// <summary>
    /// Operation not permitted
    /// </summary>
    public const int EPERM = 1;

    /// <summary>
    /// No such file or directory
    /// </summary>
    public const int ENOENT = 2;

    /// <summary>
    /// No such process
    /// </summary>
    public const int ESRCH = 3;

    /// <summary>
    /// Interrupted system call
    /// </summary>
    public const int EINTR = 4;

    /// <summary>
    /// I/O error
    /// </summary>
    public const int EIO = 5;

    /// <summary>
    /// No such device or address
    /// </summary>
    public const int ENXIO = 6;

    /// <summary>
    /// Argument list too long
    /// </summary>
    public const int E2BIG = 7;

    /// <summary>
    /// Exec format error
    /// </summary>
    public const int ENOEXEC = 8;

    /// <summary>
    /// Bad file number
    /// </summary>
    public const int EBADF = 9;

    /// <summary>
    /// No child processes
    /// </summary>
    public const int ECHILD = 10;

    /// <summary>
    /// Try again
    /// </summary>
    public const int EAGAIN = 11;

    /// <summary>
    /// Out of memory
    /// </summary>
    public const int ENOMEM = 12;

    /// <summary>
    /// Permission denied
    /// </summary>
    public const int EACCES = 13;

    /// <summary>
    /// Bad address
    /// </summary>
    public const int EFAULT = 14;

    /// <summary>
    /// Block device required
    /// </summary>
    public const int ENOTBLK = 15;

    /// <summary>
    /// Device or resource busy
    /// </summary>
    public const int EBUSY = 16;

    /// <summary>
    /// File exists
    /// </summary>
    public const int EEXIST = 17;

    /// <summary>
    /// Cross-device link
    /// </summary>
    public const int EXDEV = 18;

    /// <summary>
    /// No such device
    /// </summary>
    public const int ENODEV = 19;

    /// <summary>
    /// Not a directory
    /// </summary>
    public const int ENOTDIR = 20;

    /// <summary>
    /// Is a directory
    /// </summary>
    public const int EISDIR = 21;

    /// <summary>
    /// Invalid argument
    /// </summary>
    public const int EINVAL = 22;

    /// <summary>
    /// File table overflow
    /// </summary>
    public const int ENFILE = 23;

    /// <summary>
    /// Too many open files
    /// </summary>
    public const int EMFILE = 24;

    /// <summary>
    /// Not a typewriter
    /// </summary>
    public const int ENOTTY = 25;

    /// <summary>
    /// Text file busy
    /// </summary>
    public const int ETXTBSY = 26;

    /// <summary>
    /// File too large
    /// </summary>
    public const int EFBIG = 27;

    /// <summary>
    /// No space left on device
    /// </summary>
    public const int ENOSPC = 28;

    /// <summary>
    /// Illegal seek
    /// </summary>
    public const int ESPIPE = 29;

    /// <summary>
    /// Read-only file system
    /// </summary>
    public const int EROFS = 30;

    /// <summary>
    /// Too many links
    /// </summary>
    public const int EMLINK = 31;

    /// <summary>
    /// Broken pipe
    /// </summary>
    public const int EPIPE = 32;

    /// <summary>
    /// Math argument out of domain of func
    /// </summary>
    public const int EDOM = 33;

    /// <summary>
    /// Math result not representable
    /// </summary>
    public const int ERANGE = 34;

    /// <summary>
    /// No data available
    /// </summary>
    public const int ENODATA = 61;
}
