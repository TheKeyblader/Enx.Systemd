// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Enx.Systemd.Device;
using Enx.Systemd.Internal;

int version = NativeMethods.Version;

var monitor = DeviceMonitor.Create();
monitor.Start(new Test());
monitor.OnEvent += (sender, args) =>
{
    Console.WriteLine(args.Device.DeviceId);
};
monitor.Event!.Loop();

class Test
{
}
