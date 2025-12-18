namespace Enx.Systemd.Events;

public enum EventState
{
    Initial = 0,
    Armed = 1,
    Pending = 2,
    Running = 3,
    Exiting = 4,
    Finished = 5,
    Preparing = 6
}
