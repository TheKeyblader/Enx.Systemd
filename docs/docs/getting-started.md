# Getting Started

Enx.Systemd is a Linux-only .NET library that provides modern, type-safe bindings to systemd APIs. The current focus is `sd-device`.

## Prerequisites

- Linux with `systemd` and `libsystemd` installed
- .NET 10 SDK

## Install

NuGet package:

[![NuGet](https://img.shields.io/nuget/v/Enx.Systemd.svg)](https://www.nuget.org/packages/Enx.Systemd/)

From NuGet:

```bash
dotnet add package Enx.Systemd
```

Or reference the project locally (for development):

```bash
dotnet build Enx.Systemd.slnx
dotnet add <your-project>.csproj reference ./Enx.Systemd/Enx.Systemd.csproj
```

Note: Enx.Systemd targets Linux only and requires `systemd` + `libsystemd` at runtime.

## Basic Usage

### Create a device from sysfs or devnode

```csharp
using Enx.Systemd.Device;

var device = Device.FromDevname("/dev/sda");
Console.WriteLine(device.Syspath);
Console.WriteLine(device.Subsystem);
Console.WriteLine(device.Devtype);
```

### Enumerate devices

```csharp
using Enx.Systemd.Device;

var enumerator = DeviceEnumerator.Create()
    .MatchSubsystem("block", match: true)
    .AllowUnitialized();

foreach (var dev in enumerator.Devices)
{
    Console.WriteLine($"{dev.Sysname} ({dev.Devname})");
}
```

### Monitor device events

```csharp
using Enx.Systemd.Device;
using Enx.Systemd.Events;

var monitor = DeviceMonitor.Create();
monitor.SetDefaultEvent();
monitor.OnEvent += (_, e) =>
{
    Console.WriteLine($"{e.Device.Action}: {e.Device.Devname}");
};
monitor.Start();

monitor.Event?.Loop();
```

## Notes on Lifetime and Ownership

- Prefer the `From*` / `Create()` factory methods. The public constructors are for advanced scenarios where you already own a native handle.
- Properties are lazily fetched and cached. If a native value is missing, string properties return `""`.

## Next Steps

- Read the introduction: `introduction.md`
- Browse the API docs: `../api/`
