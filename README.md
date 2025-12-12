# Enx.Systemd

> **Status: ALPHA – API not stable yet**

Enx.Systemd is a .NET library that provides **modern, type-safe access to systemd APIs** from C#.

The goal of this project is to offer **clean, idiomatic .NET bindings** over systemd, built on top of:
- `LibraryImport` (source-generated P/Invoke)
- explicit ownership and memory-safe patterns
- minimal abstractions over the native APIs

At the moment, the library focuses on **`sd-device`**, allowing inspection and traversal of Linux devices through systemd’s device model.

---

## ✨ Features

- Access systemd **sd-device** API from .NET
- Safe handling of native resources (`SafeHandle`)
- Explicit and predictable memory ownership
- UTF-8–correct string marshalling
- Designed for **Linux-first** tooling and system-level applications

---

## 🚧 Current State

This project is in **early development (ALPHA)**.

- Only a subset of **sd-device** is currently implemented
- APIs may change without notice
- Documentation is minimal and evolving

That said, the core interop layer is already functional and used in real-world experiments.

---

## 🎯 Motivation

Systemd exposes powerful low-level APIs, but consuming them safely from .NET is non-trivial.

Enx.Systemd aims to:
- remove the pain of P/Invoke boilerplate
- avoid hidden runtime marshalling
- make systemd APIs usable in modern .NET applications

Typical use cases include:
- hardware inspection tools
- device management utilities
- Linux desktop tooling
- system-level diagnostics

---

## 🐧 Platform Support

- **Linux only**
- Requires **systemd** and **libsystemd**

Other platforms are intentionally not supported.

---

## 🔮 Roadmap (high-level)

- Extend `sd-device` coverage
- Add bindings for other systemd components
- Improve documentation and examples
- Stabilize the public API

---

## ⚠️ Disclaimer

This project is **not affiliated with systemd**.

Use at your own risk — especially in production environments.

---

## 🤝 Contributing

Feedback, issues, and experiments are welcome.
Expect breaking changes while the project is in alpha.
