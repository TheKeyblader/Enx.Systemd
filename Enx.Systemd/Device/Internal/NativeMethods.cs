using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

// ReSharper disable once CheckNamespace
namespace Enx.Systemd.Internal;

public static partial class NativeMethods
{
    public static partial class SdDevice
    {
        [LibraryImport(Library, EntryPoint = "sd_device_ref")]
        public static partial nint DeviceRef(nint ret);

        [LibraryImport(Library, EntryPoint = "sd_device_unref")]
        public static partial nint DeviceUnref(nint ret);

        #region NewFrom

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_syspath", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromSyspath(out SdDeviceHandle ret, string syspath);

        //[LibraryImport(Library, EntryPoint = "sd_device_new_from_devnum", StringMarshalling = StringMarshalling.Utf16)]
        //public static partial int DeviceNewFromDevnum(out SdDeviceHandle handle, char type,);
        [LibraryImport(Library, EntryPoint = "sd_device_new_from_subsystem_sysname",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromSubsystemSysname(out SdDeviceHandle ret, string subsystem,
            string sysname);

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_device_id",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromDeviceId(out SdDeviceHandle ret, string id);

        //[LibraryImport(Library, EntryPoint = "sd_device_new_from_stat_rdev", StringMarshalling = StringMarshalling.Utf16)]
        //public static partial int DeviceNewFromStatRdev(out SdDeviceHandle handle, string syspath);

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_devname", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromDevname(out SdDeviceHandle ret, string devname);

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_path", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromPath(out SdDeviceHandle ret, string path);

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_ifname", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewFromIfname(out SdDeviceHandle ret, string ifname);

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_ifindex")]
        public static partial int DeviceNewFromIfindex(out SdDeviceHandle ret, int ifindex);

        #endregion

        [LibraryImport(Library, EntryPoint = "sd_device_new_from_ifindex", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceNewChild(out SdDeviceHandle ret, SdDeviceHandle device, string suffix);

        [LibraryImport(Library, EntryPoint = "sd_device_get_parent")]
        public static partial int DeviceGetParent(SdDeviceHandle device, out SdDeviceHandle ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_parent_with_subsystem_devtype",
            StringMarshalling = StringMarshalling.Utf16)]
        public static partial int DeviceGetParentWithSubsystemDevtype(SdDeviceHandle deviceHandle, string subsystem,
            string devtype, out SdDeviceHandle ret);

        #region Getters

        [LibraryImport(Library, EntryPoint = "sd_device_get_syspath", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetSysPath(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_subsystem", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetSubsystem(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_driver_subsystem",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDriverSubsystem(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devtype", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDevtype(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devnum", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDevnum(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_ifindex", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetIfindex(SdDeviceHandle device, out int ifindex);

        [LibraryImport(Library, EntryPoint = "sd_device_get_driver", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDriver(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devpath", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDevpath(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devname", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDevname(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_sysname", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetSysname(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_sysnum", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetSysnum(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_action", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetAction(SdDeviceHandle device, out uint ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_seqnum", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetSeqnum(SdDeviceHandle device, out ulong ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_diskseq", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDiskseq(SdDeviceHandle device, out ulong ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_device_id", StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetDeviceId(SdDeviceHandle device, out string ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_is_initialized",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetIsInitialized(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_usec_initialized",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetUsecInitialized(SdDeviceHandle device, out ulong ret);

        [LibraryImport(Library, EntryPoint = "sd_device_get_usec_since_initialized",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceGetUsecSinceInitialized(SdDeviceHandle device, out ulong ret);

        #region DynamicProperties

        [LibraryImport(Library, EntryPoint = "sd_device_get_tag_first")]
        [return: MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetTagFirst(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_tag_next")]
        [return: MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetTagNext(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_child_first", StringMarshalling = StringMarshalling.Utf8)]
        public static unsafe partial SdDeviceHandle DeviceGetChildFirst(SdDeviceHandle device, string? suffix);

        [LibraryImport(Library, EntryPoint = "sd_device_get_child_next", StringMarshalling = StringMarshalling.Utf8)]
        public static unsafe partial SdDeviceHandle DeviceGetChildNext(SdDeviceHandle device, string? suffix);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devlink_first")]
        [return: MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetDevlinkFirst(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_devlink_next")]
        [return: MarshalUsing(typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetDevlinkNext(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_property_first",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetPropertyFirst(SdDeviceHandle device, out string value);

        [LibraryImport(Library, EntryPoint = "sd_device_get_property_next",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetPropertyNext(SdDeviceHandle device, out string value);

        [LibraryImport(Library, EntryPoint = "sd_device_get_sysattr_first",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetSysattrFirst(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_sysattr_next",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial string? DeviceGetSysattrNext(SdDeviceHandle device);

        [LibraryImport(Library, EntryPoint = "sd_device_get_sysattr_value",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static unsafe partial int
            DeviceGetSysattrValue(SdDeviceHandle device, string sysattr, out string value);

        #endregion

        #endregion

        #region DeviceEnumerator

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_new")]
        public static partial int DeviceEnumeratorNew(out SdDeviceEnumeratorHandle ret);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_ref")]
        public static partial nint DeviceEnumeratorRef(nint ret);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_unref")]
        public static partial nint DeviceEnumeratorUnref(nint ret);

        #region Iterator

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_get_device_first")]
        public static partial SdDeviceHandle DeviceEnumeratorGetDeviceFirst(SdDeviceEnumeratorHandle enumerator);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_get_device_next")]
        public static partial SdDeviceHandle DeviceEnumeratorGetDeviceNext(SdDeviceEnumeratorHandle enumerator);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_get_subsystem_first")]
        public static partial SdDeviceHandle DeviceEnumeratorGetSubsystemFirst(SdDeviceEnumeratorHandle enumerator);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_get_subsystem_next")]
        public static partial SdDeviceHandle DeviceEnumeratorGetSubsystemNext(SdDeviceEnumeratorHandle enumerator);

        #endregion

        #region Matches

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_subsystem",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchSubsystem(SdDeviceEnumeratorHandle handle, string subsystem,
            [MarshalUsing(typeof(BoolMarshaller))] bool match);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_sysattr",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchSysattr(SdDeviceEnumeratorHandle handle, string sysattr,
            string value, [MarshalUsing(typeof(BoolMarshaller))] bool match);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_property",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchProperty(SdDeviceEnumeratorHandle handle, string property,
            string value);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_property_required",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchPropertyRequired(SdDeviceEnumeratorHandle handle,
            string property,
            string value);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_sysname",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchSysname(SdDeviceEnumeratorHandle handle, string sysname);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_nomatch_sysname",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddNoMatchSysname(SdDeviceEnumeratorHandle handle, string sysname);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_tag",
            StringMarshalling = StringMarshalling.Custom,
            StringMarshallingCustomType = typeof(Utf8StringNoFreeMarshaller))]
        public static partial int DeviceEnumeratorAddMatchTag(SdDeviceEnumeratorHandle handle, string tag);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_match_parent")]
        public static partial int
            DeviceEnumeratorAddMatchParent(SdDeviceEnumeratorHandle handle, SdDeviceHandle parent);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_allow_uninitialized")]
        public static partial int DeviceEnumeratorAllowUnitialized(SdDeviceEnumeratorHandle handle);

        [LibraryImport(Library, EntryPoint = "sd_device_enumerator_add_all_parents")]
        public static partial int DeviceEnumeratorAddAllParents(SdDeviceEnumeratorHandle handle);

        #endregion

        #endregion
    }
}