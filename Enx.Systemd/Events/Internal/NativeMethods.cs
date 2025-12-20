using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public static partial class NativeMethods
{
    public static partial class SdEvent
    {
        public const int SignalProcmask = (1 << 30);

        #region Event

        [LibraryImport(Library, EntryPoint = "sd_event_default")]
        public static partial int EventDefault(out SdEventHandle ret);


        [LibraryImport(Library, EntryPoint = "sd_event_new")]
        public static partial int EventNew(out SdEventHandle ret);

        [LibraryImport(Library, EntryPoint = "sd_event_ref")]
        public static partial nint EventRef(nint e);

        [LibraryImport(Library, EntryPoint = "sd_event_unref")]
        public static partial nint EventUnref(nint e);

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Handler(nint s, nint userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int IoHandler(nint s, int fd, uint revents, nint userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int TimeHandler(nint s, ulong usec, nint userdata);

        #endregion

        #region EventHandlers

        [LibraryImport(Library, EntryPoint = "sd_event_add_io")]
        public static partial int EventAddIo(SdEventHandle e, out SdEventSourceHandle s, int fd, uint events,
            IoHandler callback, nint userdata);

        [LibraryImport(Library, EntryPoint = "sd_event_add_time")]
        public static partial int EventAddTime(SdEventHandle e, out SdEventSourceHandle s, int clock, ulong usec,
            ulong accurary, TimeHandler callback, nint userdata);

        [LibraryImport(Library, EntryPoint = "sd_event_add_time_relative")]
        public static partial int EventAddTimeRelative(SdEventHandle e, out SdEventSourceHandle s, int clock,
            ulong usec,
            ulong accurary, TimeHandler callback, nint userdata);

        #endregion

        [LibraryImport(Library, EntryPoint = "sd_event_prepare")]
        public static partial int EventPrepare(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_wait")]
        public static partial int EventWait(SdEventHandle e, ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_dispatch")]
        public static partial int EventDispatch(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_run")]
        public static partial int EventRun(SdEventHandle e, ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_loop")]
        public static partial int EventLoop(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_exit")]
        public static partial int EventExit(SdEventHandle e, int code);


        [LibraryImport(Library, EntryPoint = "sd_event_now")]
        public static partial int EventNow(SdEventHandle e, int clock, ulong usec);


        #region Getters/Setters

        [LibraryImport(Library, EntryPoint = "sd_event_get_fd")]
        public static partial int EventGetFd(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_get_state")]
        public static partial int EventGetState(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_get_tid")]
        public static partial int EventGetTid(SdEventHandle e, out int tid);

        [LibraryImport(Library, EntryPoint = "sd_event_get_exit_code")]
        public static partial int EventGetExitCode(SdEventHandle e, out int code);

        [LibraryImport(Library, EntryPoint = "sd_event_set_watchdog")]
        public static partial int EventSetWatchdog(SdEventHandle e, int b);

        [LibraryImport(Library, EntryPoint = "sd_event_get_watchdog")]
        public static partial int EventGetWatchdog(SdEventHandle e);

        [LibraryImport(Library, EntryPoint = "sd_event_get_iteration")]
        public static partial int EventGetIteration(SdEventHandle e, out ulong ret);

        [LibraryImport(Library, EntryPoint = "sd_event_set_signal_exit")]
        public static partial int EventSetSignalExit(SdEventHandle e, int b);

        #endregion

        #endregion

        #region EventSource

        [LibraryImport(Library, EntryPoint = "sd_event_source_ref")]
        public static partial SdEventSourceHandle EventSourceRef(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_unref")]
        public static partial nint EventSourceUnref(nint s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_disable_unref")]
        public static partial nint EventSourceDisableUnref(nint s);


        [LibraryImport(Library, EntryPoint = "sd_event_source_get_event")]
        public static partial SdEventHandle EventSourceGetEvent(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_userdata")]
        public static partial nint EventSourceGetUserdata(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_userdata")]
        public static partial nint EventSourceSetUserdata(SdEventSourceHandle s, nint userdata);


        [LibraryImport(Library, EntryPoint = "sd_event_source_set_description")]
        public static partial int EventSourceSetDescription(SdEventSourceHandle s,
            [MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
            string description);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_description")]
        public static partial int EventSourceGetDescription(SdEventSourceHandle s,
            [MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
            out string description);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_prepare")]
        public static partial int EventSourceSetPrepare(SdEventSourceHandle s, Handler? callback);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_pending")]
        [return: MarshalUsing(typeof(BoolMarshaller))]
        public static partial bool EventSourceGetPending(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_priority")]
        public static partial int EventSourceGetPriority(SdEventSourceHandle s, out long priority);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_priority")]
        public static partial int EventSourceSetPriority(SdEventSourceHandle s, long priority);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_enabled")]
        public static partial int EventSourceGetEnabled(SdEventSourceHandle s,
            [MarshalUsing(typeof(BoolMarshaller))] out bool enabled);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_enabled")]
        public static partial int EventSourceSetEnabled(SdEventSourceHandle s,
            [MarshalUsing(typeof(BoolMarshaller))] bool enabled);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_io_fd")]
        public static partial int EventSourceGetIoFd(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_io_fd")]
        public static partial int EventSourceSetIoFd(SdEventSourceHandle s, int fd);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_io_fd_own")]
        [return: MarshalUsing(typeof(BoolMarshaller))]
        public static partial bool EventSourceGetIoFdOwn(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_io_fd_own")]
        public static partial int EventSourceSetIoFdOwn(SdEventSourceHandle s,
            [MarshalUsing(typeof(BoolMarshaller))] bool own);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_io_events")]
        public static partial int EventSourceGetIoEvents(SdEventSourceHandle s, out uint events);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_io_events")]
        public static partial int EventSourceSetIoEvents(SdEventSourceHandle s, uint events);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_io_revents")]
        public static partial int EventSourceGetIoRevents(SdEventSourceHandle s, out uint revents);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_time")]
        public static partial int EventSourceGetTime(SdEventSourceHandle s, out ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_time")]
        public static partial int EventSourceSetTime(SdEventSourceHandle s, ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_time_relative")]
        public static partial int EventSourceSetTimeRelative(SdEventSourceHandle s, ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_time_accuracy")]
        public static partial int EventSourceGetTimeAccuracy(SdEventSourceHandle s, out ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_time_accuracy")]
        public static partial int EventSourceSetTimeAccuracy(SdEventSourceHandle s, ulong usec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_time_clock")]
        public static partial int EventSourceGetTimeClock(SdEventSourceHandle s, out int clock);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_signal")]
        public static partial int EventSourceGetSignal(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_child_pid")]
        public static partial int EventSourceGetChildPid(SdEventSourceHandle s, out int pid);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_child_pidfd")]
        public static partial int EventSourceGetChildPidfd(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_child_pidfd_own")]
        public static partial int EventSourceGetChildPidfdOwn(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_child_pidfd_own")]
        public static partial int EventSourceSetChildPidfdOwn(SdEventSourceHandle s, int own);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_child_process_own")]
        public static partial int EventSourceGetChildProcessOwn(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_child_process_own")]
        public static partial int EventSourceSetChildProcessOwn(SdEventSourceHandle s, int own);

        [LibraryImport(Library, EntryPoint = "sd_event_source_send_child_signal")]
        public static partial int EventSourceSendChildSignal(SdEventSourceHandle s, int sig, nint si, uint flags);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_inotify_mask")]
        public static partial int EventSourceGetInotifyMask(SdEventSourceHandle s, out uint ret);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_memory_pressure_type")]
        public static partial int EventSourceSetMemoryPressureType(SdEventSourceHandle s,
            [MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
            string ty);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_memory_pressure_period")]
        public static partial int EventSourceSetMemoryPressurePeriod(SdEventSourceHandle s, ulong thresholdUsec,
            ulong windowUsec);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_destroy_callback")]
        public static partial int EventSourceSetDestroyCallback(SdEventSourceHandle s, nint callback);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_destroy_callback")]
        public static partial int EventSourceGetDestroyCallback(SdEventSourceHandle s, out nint callback);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_floating")]
        [return: MarshalUsing(typeof(BoolMarshaller))]
        public static partial bool EventSourceGetFloating(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_floating")]
        public static partial int EventSourceSetFloating(SdEventSourceHandle s,
            [MarshalUsing(typeof(BoolMarshaller))] bool b);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_exit_on_failure")]
        [return: MarshalUsing(typeof(BoolMarshaller))]
        public static partial bool EventSourceGetExitOnFailure(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_exit_on_failure")]
        public static partial int EventSourceSetExitOnFailure(SdEventSourceHandle s,
            [MarshalUsing(typeof(BoolMarshaller))] bool b);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_ratelimit")]
        public static partial int EventSourceSetRateLimit(SdEventSourceHandle s, ulong intervalUsec, uint burst);

        [LibraryImport(Library, EntryPoint = "sd_event_source_get_ratelimit")]
        public static partial int EventSourceGetRateLimit(SdEventSourceHandle s, out ulong retIntervalUsec,
            out uint retBurst);

        [LibraryImport(Library, EntryPoint = "sd_event_source_is_ratelimited")]
        public static partial int EventSourceIsRateLimited(SdEventSourceHandle s);

        [LibraryImport(Library, EntryPoint = "sd_event_source_set_ratelimit_expire_callback")]
        public static partial int EventSourceSetRateLimitExpireCallback(SdEventSourceHandle s, Handler callback);

        [LibraryImport(Library, EntryPoint = "sd_event_source_leave_ratelimit")]
        public static partial int EventSourceLeaveRateLimit(SdEventSourceHandle s);

        #endregion

        [LibraryImport(Library, EntryPoint = "sd_event_trim_memory")]
        public static partial int EventTrimMemory();
    }
}
