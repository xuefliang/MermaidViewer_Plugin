# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MermaidViewer** is a Notepad++ plugin written in C# that provides real-time preview and export of Mermaid diagrams. It uses the mermaid-rs-renderer (mmdr) CLI tool for fast rendering.

## Build & Development Commands

### Building
```bash
# In Visual Studio: Build > Build Solution (or Ctrl+Shift+B)
# Or from command line:
msbuild src/MermaidViewer.csproj /p:Configuration=Release /p:Platform="AnyCPU"
```

The compiled DLL outputs to:
- Debug: `src/bin/Debug/MermaidViewer.dll`
- Release: `src/bin/Release/MermaidViewer.dll`

The post-build event in the .csproj automatically copies the DLL and mmdr.exe to the Notepad++ plugins directory.

### Plugin Installation (Development)
The Release build automatically installs to `Notepad++/plugins/MermaidViewer/` via post-build event, which queries the Windows registry for Notepad++ installation path.

### Optional: Rebuilding mmdr.exe
```bash
cd tools/mmdr
cargo build --release
copy target/release/mmdr.exe ../mmdr/
```

## Architecture Overview

### Core Components

**MermaidPlugin.cs** - Main plugin class inheriting from PluginBase. Responsibilities:
- Plugin initialization and menu setup
- Settings management and loading
- Preview form lifecycle and event handling
- File watching and debounced refresh logic
- Dark mode detection

**Forms/**
- `MermaidPreviewForm.cs` - WinForms user control displaying SVG rendered diagrams with zoom/pan capabilities
- `SettingsForm.cs` - Settings dialog for configuration options

**Rendering/**
- `MermaidRenderer.cs` - Wrapper around mmdr.exe CLI that executes the renderer process

**PluginInfrastructure/** - Framework code from NotepadPlusPlusPluginPack.Net:
- `PluginBase.cs` - Base class for plugins with Notepad++ integration
- `NotepadPlusPlusGateway.cs` - Wraps Notepad++ native API (via P/Invoke)
- `ScintillaGateway.cs` - Wraps Scintilla editor control API
- `UnManagedExports.cs` - DLL exports required by Notepad++ plugin interface
- `Messages.cs` - Notepad++ message constants
- `Win32.cs` - Win32 API definitions and helpers

### Key Design Patterns

**mmdr CLI Integration**: Rather than using P/Invoke to call a Rust DLL, the plugin spawns mmdr.exe as a subprocess and passes Mermaid source code via stdin, receiving SVG output via stdout. This trades ~3ms process overhead for simplicity and stability.

**Settings Storage**: Plugin settings are serialized to XML in the Notepad++ config directory (`%AppData%\Notepad++\plugins\config\`).

**Debounced Refresh**: A timer-based debouncing mechanism prevents excessive rendering while typing. The `_debounceTimer` delays render requests by ~500ms after the last edit.

**Dark Mode Detection**: The plugin queries Notepad++ to determine if dark mode is active and passes this to the preview form for appropriate styling.

## Key Files Reference

| File | Purpose |
|------|---------|
| `MermaidPlugin.cs` | Plugin entry point and main event loop |
| `Forms/MermaidPreviewForm.cs` | Preview panel UI and rendering |
| `Rendering/MermaidRenderer.cs` | mmdr process wrapper |
| `MermaidViewer.csproj` | Build configuration with post-build install script |

## Project Structure

```
.
├── src/
│   ├── MermaidPlugin.cs               # Main plugin class
│   ├── PluginInfrastructure/          # Framework from PluginPack.Net
│   ├── Forms/                         # UI components (WinForms)
│   ├── Rendering/                     # Rendering backends
│   ├── Properties/AssemblyInfo.cs     # Assembly version
│   └── MermaidViewer.csproj           # Project file
├── tools/mmdr/                        # Pre-compiled mmdr.exe
├── docs/                              # Documentation
├── examples/                          # Sample .mmd files
├── MermaidViewer.sln                  # Visual Studio solution
└── README.md
```

## Technical Decisions

**C# over C++**: Chosen for superior UI development with WinForms/WPF and easier maintenance. Reference: PlantUmlViewer plugin demonstrates this approach works well for Notepad++.

**mmdr CLI over Rust DLL**: Simplicity and reliability outweigh the ~3ms process overhead. Direct P/Invoke to a Rust DLL introduces complex FFI and ABI compatibility issues across Windows versions.

**Settings in XML**: Standard .NET approach using `XDocument` for serialization in Notepad++ config directory.

## Important Context

- Target Framework: **.NET Framework 4.6.2** (required by Notepad++ plugin ecosystem for compatibility)
- Platform: Windows only (plugin integrates with Notepad++ via Win32/COM)
- No external NuGet dependencies (uses only .NET Framework standard libraries)
- The plugin must be placed in `plugins/MermaidViewer/` subdirectory (not directly in plugins/)

## Common Tasks

**Adding a Menu Item**: In `MermaidPlugin.cs`, add entry to `SetupMenu()` method using `MenuItemBase.Create()`. The menu item automatically handles click routing to a handler method.

**Implementing a New Renderer Backend**: Create new class in `Rendering/`, implement the rendering interface, and update `MermaidRenderer.cs` to use it.

**Modifying Preview UI**: Edit `MermaidPreviewForm.cs` (the form layout code) and `MermaidPreviewForm.Designer.cs` (auto-generated). Use the designer or direct code modification for WinForms controls.

**Plugin Settings**: Modify `MermaidSettings` class (should be defined in plugin codebase) and ensure XML serialization in `MermaidPlugin.LoadSettings()`.
