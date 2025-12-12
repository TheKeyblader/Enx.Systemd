using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Enx.Systemd;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Errno
{
    OK = 0,

    /// <summary>
    /// Operation not permitted
    /// </summary>
    [Description("Operation not permitted")]
    EPERM = 1,

    /// <summary>
    /// No such file or directory
    /// </summary>
    [Description("No such file or directory")]
    ENOENT = 2,

    /// <summary>
    /// No such process
    /// </summary>
    [Description("No such process")] ESRCH = 3,

    /// <summary>
    /// Interrupted system call
    /// </summary>
    [Description("Interrupted system call")]
    EINTR = 4,

    /// <summary>
    /// I/O error
    /// </summary>
    [Description("I/O error")] EIO = 5,

    /// <summary>
    /// No such device or address
    /// </summary>
    [Description("No such device or address")]
    ENXIO = 6,

    /// <summary>
    /// Argument list too long
    /// </summary>
    [Description("Argument list too long")]
    E2BIG = 7,

    /// <summary>
    /// Exec format error
    /// </summary>
    [Description("Exec format error")] ENOEXEC = 8,

    /// <summary>
    /// Bad file number
    /// </summary>
    [Description("Bad file number")] EBADF = 9,

    /// <summary>
    /// No child processes
    /// </summary>
    [Description("No child processes")] ECHILD = 10,

    /// <summary>
    /// Try again
    /// </summary>
    [Description("Try again")] EAGAIN = 11,

    /// <summary>
    /// Out of memory
    /// </summary>
    [Description("Out of memory")] ENOMEM = 12,

    /// <summary>
    /// Permission denied
    /// </summary>
    [Description("Permission denied")] EACCES = 13,

    /// <summary>
    /// Bad address
    /// </summary>
    [Description("Bad address")] EFAULT = 14,

    /// <summary>
    /// Block device required
    /// </summary>
    [Description("Block device required")] ENOTBLK = 15,

    /// <summary>
    /// Device or resource busy
    /// </summary>
    [Description("Device or resource busy")]
    EBUSY = 16,

    /// <summary>
    /// File exists
    /// </summary>
    [Description("File exists")] EEXIST = 17,

    /// <summary>
    /// Cross-device link
    /// </summary>
    [Description("Cross-device link")] EXDEV = 18,

    /// <summary>
    /// No such device
    /// </summary>
    [Description("No such device")] ENODEV = 19,

    /// <summary>
    /// Not a directory
    /// </summary>
    [Description("Not a directory")] ENOTDIR = 20,

    /// <summary>
    /// Is a directory
    /// </summary>
    [Description("Is a directory")] EISDIR = 21,

    /// <summary>
    /// Invalid argument
    /// </summary>
    [Description("Invalid argument")] EINVAL = 22,

    /// <summary>
    /// File table overflow
    /// </summary>
    [Description("File table overflow")] ENFILE = 23,

    /// <summary>
    /// Too many open files
    /// </summary>
    [Description("Too many open files")] EMFILE = 24,

    /// <summary>
    /// Not a typewriter
    /// </summary>
    [Description("Not a typewriter")] ENOTTY = 25,

    /// <summary>
    /// Text file busy
    /// </summary>
    [Description("Text file busy")] ETXTBSY = 26,

    /// <summary>
    /// File too large
    /// </summary>
    [Description("File too large")] EFBIG = 27,

    /// <summary>
    /// No space left on device
    /// </summary>
    [Description("No space left on device")]
    ENOSPC = 28,

    /// <summary>
    /// Illegal seek
    /// </summary>
    [Description("Illegal seek")] ESPIPE = 29,

    /// <summary>
    /// Read-only file system
    /// </summary>
    [Description("Read-only file system")] EROFS = 30,

    /// <summary>
    /// Too many links
    /// </summary>
    [Description("Too many links")] EMLINK = 31,

    /// <summary>
    /// Broken pipe
    /// </summary>
    [Description("Broken pipe")] EPIPE = 32,

    /// <summary>
    /// Math argument out of domain of func
    /// </summary>
    [Description("Math argument out of domain of func")]
    EDOM = 33,

    /// <summary>
    /// Math result not representable
    /// </summary>
    [Description("Math result not representable")]
    ERANGE = 34,
}