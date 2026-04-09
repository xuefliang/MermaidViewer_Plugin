# Mermaid Viewer Plugin Development Guide

## Project Overview

MermaidViewer is a Notepad++ plugin that provides real-time Mermaid diagram preview and export functionality. It uses the mermaid-rs-renderer (mmdr) for ultra-fast rendering.

### Key Features
- **Real-time Preview**: Automatically renders Mermaid diagrams as you type
- **Fast Rendering**: Uses Rust-based mmdr renderer (500-1000x faster than mermaid-cli)
- **Multiple Formats**: Export diagrams as SVG or PNG
- **Multiple Diagrams**: Support for documents containing multiple Mermaid diagrams
- **Dark Mode**: Automatic dark mode support

## Architecture

```
NotepadPlusPlus_MermaidPlugin/
├── src/
│   ├── MermaidPlugin.cs          # Main plugin class
│   ├── PluginInfrastructure/     # Notepad++ plugin framework
│   │   ├── PluginBase.cs         # Base plugin class
│   │   ├── UnManagedExports.cs   # DLL exports for Notepad++
│   │   ├── NotepadPlusPlusGateway.cs  # Notepad++ API wrapper
│   │   ├── ScintillaGateway.cs   # Scintilla editor wrapper
│   │   ├── Messages.cs           # Notepad++ messages
│   │   ├── Win32.cs              # Win32 API imports
│   │   └── ...
│   ├── Forms/                     # UI components
│   │   ├── MermaidPreviewForm.cs  # Preview panel
│   │   └── SettingsForm.cs        # Settings dialog
│   ├── Rendering/
│   │   └── MermaidRenderer.cs    # mmdr CLI wrapper
│   └── Properties/
│       └── AssemblyInfo.cs
├── tools/
│   └── mmdr/                      # mmdr executable
├── examples/                      # Sample .mmd files
└── docs/
    └── DEVELOPMENT.md            # This file
```

## Technical Decisions

### Why C#/.NET?

| Factor | C++ | C#/.NET | PythonScript |
|--------|-----|---------|-------------|
| UI Development | Complex | WinForms/WPF | Limited |
| Maintenance | High | Medium | Low |
| Performance | Best | Good | Poor |
| Plugin Ecosystem | Moderate | Large (PlantUmlViewer) | Limited |

The PlantUmlViewer plugin provided an excellent reference implementation for the C# approach.

### Why mmdr CLI instead of Rust DLL?

| Approach | Pros | Cons |
|----------|------|------|
| Rust DLL via P/Invoke | Fastest, no process | Complex FFI, ABI issues |
| mmdr CLI | Simple, portable | Process overhead (~3ms) |
| WebAssembly | Cross-platform | Notepad++ integration complex |

mmdr CLI was chosen for its simplicity and reliability. The 3ms cold-start overhead is negligible compared to the rendering speed gain.

## Building the Plugin

### Prerequisites

1. **Visual Studio 2022** with:
   - .NET desktop development
   - NuGet package manager

2. **.NET Framework 4.6.2** or higher

3. **Optional**: Rust toolchain for compiling mmdr

### Build Steps

1. Open `MermaidViewer.sln` in Visual Studio

2. Restore NuGet packages:
   ```bash
   cd src
   nuget restore packages.config
   ```

3. Build the solution:
   - Debug: `Build > Build Solution`
   - Release: `Build > Configuration Manager > Release`

4. The compiled DLL will be in:
   - `src/bin/Release/MermaidViewer.dll`

### Post-Build (for development)

The project is configured to automatically copy the DLL to Notepad++'s plugin directory during Debug/Release builds.

## Installing the Plugin

### Manual Installation

1. Copy the following files to `Notepad++/plugins/MermaidViewer/`:
   ```
   MermaidViewer.dll
   mmdr.exe (from tools/mmdr/)
   ```

2. Create a folder named `MermaidViewer` in the plugins directory:
   ```
   C:\Program Files\Notepad++\plugins\MermaidViewer\
   ```

3. Copy all files into this folder

4. Restart Notepad++

### For Plugin Distribution

Create a ZIP file with this structure:
```
MermaidViewer/
├── MermaidViewer.dll
├── mmdr.exe
├── README.md
└── examples/
    ├── flowchart.mmd
    ├── sequence.mmd
    └── ...
```

## Using the Plugin

### Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl+Shift+M | Toggle preview panel |
| Ctrl+F5 | Refresh preview |
| Ctrl+Mouse Wheel | Zoom in/out |
| Double-click | Reset view |

### Menu Options

- **Preview Mermaid**: Show/hide the preview panel
- **Refresh Preview**: Re-render the current diagram
- **Export as SVG**: Save diagram as SVG file
- **Export as PNG**: Save diagram as PNG file
- **Previous/Next Diagram**: Navigate between multiple diagrams
- **Zoom In/Out/Reset**: Control diagram zoom
- **Settings**: Configure plugin options

## Extending the Plugin

### Adding a New Menu Command

1. Open `MermaidPlugin.cs`

2. Add to `SetupMenu()`:
   ```csharp
   _menuItems.Add(MenuItemBase.Create(
       "Your Command",
       "Description shown in status bar",
       YourMethod,
       ctrl, alt, shift, keyCode
   ));
   ```

3. Implement the method:
   ```csharp
   private void YourMethod()
   {
       // Your code here
   }
   ```

### Adding a New Rendering Backend

1. Create a new class in `Rendering/`

2. Implement the interface:
   ```csharp
   public interface IMermaidRenderer
   {
       Task<string> RenderToSvgAsync(string source);
       Task<byte[]> RenderToPngAsync(string source);
       string LastError { get; }
   }
   ```

3. Update `MermaidRenderer.cs` to use the new backend

## Troubleshooting

### mmdr.exe Not Found

**Error**: "mmdr.exe not found"

**Solution**: 
1. Download mmdr from the releases
2. Place it in the plugin directory: `plugins/MermaidViewer/mmdr.exe`

### Rendering Fails

**Error**: "Error rendering diagram"

**Solutions**:
1. Check if the Mermaid syntax is valid
2. Verify mmdr.exe is working: run `mmdr.exe --version`
3. Check the status bar for error details

### Plugin Not Loading

**Error**: Plugin doesn't appear in Notepad++

**Solutions**:
1. Check Notepad++ plugins admin for errors
2. Verify the DLL is in `plugins/MermaidViewer/`
3. Check Windows Event Viewer for .NET errors
4. Ensure x86/x64 matches your Notepad++ version

## Performance Tips

1. **Use .mmd file extension** for automatic detection
2. **Disable auto-refresh** for large files
3. **Increase refresh delay** if typing feels sluggish
4. **Export to SVG** for fastest export

## API Reference

### NotepadPlusPlusGateway

```csharp
// Get current file
string path = _notepadGateway.GetFullCurrentPath();
string fileName = _notepadGateway.GetCurrentFileName();

// File operations
_notepadGateway.SaveCurrentFile();
_notepadGateway.OpenFile("path/to/file");

// Get editor
IntPtr scintilla = _notepadGateway.GetCurrentScintilla();
```

### ScintillaGateway

```csharp
// Get/Set text
string text = _scintillaGateway.GetText();
_scintillaGateway.SetText("new content");

// Selection
string selected = _scintillaGateway.GetSelText();
_scintillaGateway.SetSelection(start, end);

// Position
int pos = _scintillaGateway.GetCurrentPos();
int line = _scintillaGateway.GetCurrentLine();
```

## License

MIT License - See LICENSE file for details

## Credits

- **mermaid-rs-renderer**: https://github.com/1jehuang/mermaid-rs-renderer
- **NotepadPlusPlusPluginPack.Net**: https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
- **PlantUmlViewer**: https://github.com/Fruchtzwerg94/PlantUmlViewer
