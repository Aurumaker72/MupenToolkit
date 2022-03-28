# MupenToolkit <img src="https://github.com/Aurumaker72/MupenToolkit/blob/main/logo.png" align="right" /> 
Continuation of Mupen Utilities

[![Release](https://img.shields.io/github/v/release/Aurumaker72/MupenToolkit?label=Release)](https://github.com/Aurumaker72/MupenToolkit/releases)

## Comparison
|                                                         | Mupen Utilities | Mupen Toolkit | Mupen Toolkit Preview |                                         |
|---------------------------------------------------------|-----------------|---------------|-----------------------|-----------------------------------------|
| Median load time (median of 1hr movie across 1000 runs) | 1554            | 7             | -                     | not applicable due to lack of TASStudio |
| Full header editing                                     | Yes             | Yes           | Yes                   |                                         |
| M64                                                     | Yes             | Yes           | Yes                   |                                         |
| CMB                                                     | Yes             | Yes           | No                    | low priority                            |
| REC                                                     | Yes             | No            | No                    |                                         |
| Statistics                                              | Yes             | Yes           | Yes                   |                                         |
| Seeker                                                  | Yes             | No            | Yes                   |                                         |
| Diagnostic                                              | Yes             | Yes           | Yes (implicit)        |                                         |
| Replacement                                             | Yes             | No            | No                    | high priority                           |

## Why?
Mupen Utilities is very big and feature-packed but extremely error-prone, inaccurate and bloated. It has become realistically unmaintainable due to the displayed code quality.
This project aims to be the official continuation of MupenUtilities with more user friendliness, less bloat and high maintainability and speed.
*Irrelevant rant:* With some coding paradigms, research and lackluster optimization work I have cut down time loading of 120 Star TAS from 1554ms to 7ms. Yeah, I did 1000 runs on MupenUtilities, each taking a median of 1,5 seconds. What a waste of processing power.

## Technical Notes
If I did something the way I did, there is a good reason.

  - When reading, ~~aliasing~~ marshalling (weird linguistic intrinsics which i just now realized are wrong) first `0x400` bytes of movie (header section) is more concise than reading every field manually, *but* due to a sequential managed structure required on C# side and this app interacting with WPF (WPF bindings do not work on structs) this is not possible. We have to create a parent class with managed types (`string` instead of `char*`) which also provides the `INotifyPropertyChanged` interface.
  - When saving, writing every primitive property manually to stream with `BinaryWriter` and manually resizing and "reinterpreting" dynamic `string` types is required, as this guarantees `0x400` size header. The input saving is inefficient but after some tinkering I ran into same dynamic managed sizing barriers, so this way will remain. :trollface:   
  - Bit operations and other hacks are endian dependant
  - The reason for the speed increase is hardware acceleration, smarter code and TASStudio improvements. Some obscure WPF knowledge and hacks are used to speed up the TASStudio rendering.
  - WPF hack reduces TASStudio layout time complexity from O(n^r), with r being the rows to O(1)

## Known Issues
- Loading extremely long movies is slow. Possible naive solution is buffering/segmenting input and virtualization
