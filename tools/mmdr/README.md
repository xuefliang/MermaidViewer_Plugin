# Mermaid Viewer mmdr Tool Source

This directory contains the Rust source code for the mmdr (Mermaid Rust Renderer) tool.
This is a minimal version that can be compiled separately.

## Prerequisites

- Rust 1.70+ installed
- Cargo

## Building

```bash
# Debug build
cargo build

# Release build (recommended)
cargo build --release

# The executable will be at:
# - Debug: target/debug/mmdr.exe
# - Release: target/release/mmdr.exe
```

## Note

This is a placeholder directory. The actual mmdr tool should be obtained from:
- https://github.com/1jehuang/mermaid-rs-renderer
- Or use the pre-compiled release from the plugin releases

For the plugin to work, copy mmdr.exe to the plugin directory.
