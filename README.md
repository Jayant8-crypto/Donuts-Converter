# OrbitConvert

OrbitConvert is a Windows-first desktop file converter built around a compact radial menu. Pick up a file in
File Explorer, trigger the menu, and choose a compatible output format without opening a conventional workspace.
Conversions run locally using the project's conversion engines; files are not uploaded.

OrbitConvert is an independent rebrand and customization of the open-source [Filee project](https://github.com/KnifeLemon/Filee).
It retains Filee's MIT license and original copyright and third-party attributions. OrbitConvert is not an official
Filee release.

## Current status

- The existing modifier-key drag-and-drop menu remains available as a fallback.
- A Windows shake gesture has been implemented to reveal the existing menu during a drag.
- The shake interaction still needs validation with real Windows Explorer drags. Until that is tested, treat shake
  activation as experimental; use the modifier-key trigger if needed.
- The radial menu filters formats from the actual dragged files and hands selections to the existing conversion
  pipeline. Output is saved beside the source by default.

## Build from source

Requirements: Windows and the .NET 10 SDK.

```powershell
git clone https://github.com/Jayant8-crypto/Donuts-Converter.git
Set-Location Donuts-Converter
dotnet build src\Filee.App\Filee.App.csproj
dotnet test tests\Filee.App.Tests\Filee.App.Tests.csproj
```

The project retains its upstream internal project and assembly names. Windows installer and packaging scripts are in
[`build/`](build/); conversion engines and platform integrations remain part of the source tree.

## Project structure

- `src/Filee.App` — Avalonia desktop app, tray integration, settings, and radial menu.
- `src/Filee.Core` — shared conversion and application contracts.
- `src/Filee.Engines` — conversion engines and format support.
- `src/Filee.Platform.Windows` — Windows and Explorer integration.
- `tests` — automated tests for app, engines, platform services, and packaging.

## License and acknowledgements

This project remains under the [MIT License](LICENSE). The original Filee copyright and attribution notices are
preserved. Third-party components and their licenses are documented in
[`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md).
