using System.Runtime.InteropServices;

namespace Enx.Systemd.Internal;

public abstract class BaseWrapper<THandle> : IDisposable
    where THandle : SafeHandle
{
    /// <summary>
    /// Gets the underlying native handle.
    /// </summary>
    public THandle Handle { get; }

    protected BaseWrapper(THandle handle, bool shouldRef)
    {
        Handle = handle;
        if (shouldRef) Ref();
    }

    public abstract void Ref();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _isDisposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        _isDisposed = true;
        if (disposing)
        {
            Handle.Dispose();
        }
    }
}
