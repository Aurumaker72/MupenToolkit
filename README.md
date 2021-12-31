# MupenToolkit <img src="https://github.com/Aurumaker72/MupenToolkit/blob/main/logo.png" align="right" /> 
Continuation of Mupen Utilities

[![Release](https://img.shields.io/github/v/release/Aurumaker72/MupenToolkit?label=Release)](https://github.com/Aurumaker72/MupenToolkit/releases)


## Features
  - Header editing
  - TASStudio input editing
  - Saving modified m64
  - Multicontroller support

## Technical Notes
If I did something the way I did, there is a good reason.

  - When reading, ~~aliasing~~ marshalling (weird linguistic intrinsics which i just now realized are wrong) first `0x400` bytes of movie (header section) is more concise than reading every field manually, *but* due to a sequential managed structure required on C# side and this app interacting with WPF (WPF bindings do not work on structs) this is not possible. We have to create a parent class with managed types (`string` instead of `char*`) which also provides the `INotifyPropertyChanged` interface.
  - When saving, writing every primitive property manually to stream with `BinaryWriter` and manually resizing and "reinterpreting" dynamic `string` types is required, as this guarantees `0x400` size header. The input saving is inefficient but after some tinkering I ran into same dynamic managed sizing barriers, so this way will remain. :trollface:   
  - Bit operations and other hacks are endian dependant
