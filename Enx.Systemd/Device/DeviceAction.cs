namespace Enx.Systemd.Device;

public enum DeviceAction : int
{
    Add,
    Remove,
    Change,
    Move,
    Online,
    Offline,
    Bind,
    Unbind,
    Max,
    Invalid = -22,
}
