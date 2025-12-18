// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Enx.Systemd.Device;

var monitor = new DeviceMonitor();
monitor.Start();
monitor.Event!.Loop();
