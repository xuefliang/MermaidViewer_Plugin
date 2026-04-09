# Notepad++ Mermaid Plugin

基于 mermaid-rs-renderer 的 Notepad++ Mermaid 图表渲染插件

## 技术选型决策

### 插件开发方式: C# + NotepadPlusPlusPluginPack.Net

| 方案 | 优势 | 劣势 | 选择理由 |
|------|------|------|----------|
| C++ 原生 | 性能最佳，Notepad++ 原生支持 | 开发周期长，UI 开发困难 | ❌ |
| C# .NET | 丰富的 UI 库，PlantUmlViewer 参考 | 需要 .NET Runtime | ✅ **采用** |
| PythonScript | 简单易用 | 性能差，无法打包 DLL | ❌ |

**选择理由**:
1. PlantUmlViewer 提供了完整的参考架构
2. C# 的 WinForms/WPF 提供更好的 UI 开发体验
3. 可以通过 P/Invoke 或进程调用 Rust DLL

### mermaid-rs-renderer 集成方式: 混合方案

| 方案 | 优势 | 劣势 | 选择理由 |
|------|------|------|----------|
| Rust DLL 直接调用 | 性能最佳 | 需要用户编译 Rust 代码 | ⚠️ 可选 |
| mmdr CLI 调用 | 简单，开箱即用 | 额外进程开销 | ✅ **默认采用** |
| WebAssembly | 跨平台 | Notepad++ 集成复杂 | ❌ |

**最终方案**: 
- 预编译 mmdr.exe 包含在插件中
- 提供 Rust 源码供高级用户自行编译 DLL
- 通过进程调用实现无缝集成

## 项目结构

```
NotepadPlusPlus_MermaidPlugin/
├── src/
│   ├── PluginInfrastructure/     # 插件基础设施 (来自 PluginPack.Net)
│   ├── Forms/                    # UI 表单
│   │   ├── MermaidPreviewForm.cs # 预览面板
│   │   └── SettingsForm.cs       # 设置对话框
│   ├── Rendering/                # 渲染引擎接口
│   │   └── MermaidRenderer.cs    # mmdr 调用封装
│   └── MermaidPlugin.cs         # 插件主类
├── tools/
│   └── mmdr/                     # mermaid-rs-renderer CLI
├── examples/                     # 示例 Mermaid 文件
├── docs/                         # 开发文档
└── README.md
```

## 支持的功能

- ✅ 实时预览面板（可停靠）
- ✅ 导出 SVG/PNG 格式
- ✅ 支持 23+ 种 Mermaid 图表类型
- ✅ 缩放和平移
- ✅ 深色模式支持
- ✅ 多图表支持
- ✅ 快捷键支持

## 快速开始

### 安装
1. 下载最新 Release
2. 解压到 `Notepad++/plugins/MermaidViewer/` 目录
3. 重启 Notepad++

### 使用
1. 打开或创建 `.mmd` 文件
2. 按 `Ctrl+Shift+M` 或点击插件菜单
3. 预览面板将显示渲染结果

## 编译

### 环境要求
- Visual Studio 2022+
- .NET Framework 4.6.2+
- Rust 工具链 (可选，用于编译 mmdr)

### 编译步骤
```bash
# 1. 克隆项目
git clone https://github.com/your-repo/NotepadPlusPlus_MermaidPlugin.git

# 2. 打开解决方案
cd NotepadPlusPlus_MermaidPlugin
start MermaidViewer.sln

# 3. 编译 Release 版本
# Visual Studio: 生成 -> 重新生成解决方案 (Release)
```

### mmdr.exe 编译 (可选)
```bash
# 如果需要重新编译 mmdr.exe
cd tools/mmdr
cargo build --release
copy target/release/mmdr.exe ../mmdr/
```

## 许可证

MIT License - 详见 LICENSE 文件
