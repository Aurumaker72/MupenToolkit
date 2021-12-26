# MupenToolkit <img src="https://github.com/Aurumaker72/MupenToolkit/blob/main/logo.png" align="right" />
Continuation of Mupen Utilities


## Features
  - Basic header editing
  - TASStudio input editing
  - Saving modified m64
  - Multicontroller support

## Technical Notes
If I did something the way I did, there is a good reason.

  - When reading, aliasing first `0x400` bytes of movie (header section) is more concise than reading every field manually, *but* due to a sequential managed structure required on C# side and this app interacting with WPF (WPF bindings do not work on structs) this is not possible. We have to create a parent class with managed types (`string` instead of `char*`) which also provides the `INotifyPropertyChanged` interface.
