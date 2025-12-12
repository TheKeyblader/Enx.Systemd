// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Enx.Systemd.Device;

using var enumerator = new DeviceEnumerator();
enumerator.MatchSubsystem("usb", true);
enumerator.MatchPropertyRequired("DEVTYPE", "usb_device");
enumerator.MatchSysAttr("bDeviceClass", "09", false);

var devices = enumerator.Devices.ToList();
var devicesWithHidraw = devices.Select(d =>
{
    using var hidEnumerator = new DeviceEnumerator();
    hidEnumerator.MatchSubsystem("hidraw", true);
    hidEnumerator.MatchParent(d);
    return new { Device = d, Hids = hidEnumerator.Devices.ToList() };
});

var stream = File.Open("../../../devices.json", FileMode.Create);
await JsonSerializer.SerializeAsync(stream, devicesWithHidraw, new JsonSerializerOptions(JsonSerializerDefaults.General)
{
    WriteIndented = true
});
await stream.FlushAsync();

Console.WriteLine("Hello, World!");