# Mermaid Viewer for Notepad++

一款基于 mermaid-rs-renderer 的 Notepad++ Mermaid 图表渲染插件，超快速渲染（比 mermaid-cli 快 500-1000 倍）。

## 功能特点

✅ **实时预览** - 边写边渲染，输入即所见  
🚀 **极速渲染** - 使用 Rust 实现，比传统方案快 500-1000 倍  
📤 **多格式导出** - 支持 SVG 和 PNG 格式导出  
📊 **多图表支持** - 一个文件可包含多个 Mermaid 图表  
🌙 **深色模式** - 自动跟随 Notepad++ 深色主题  
⌨️ **快捷键支持** - 高效键盘操作  

## 支持的图表类型

| 类型 | 关键字 | 示例 |
|------|--------|------|
| 流程图 | flowchart, graph | `flowchart TD A-->B` |
| 时序图 | sequenceDiagram | `sequenceDiagram Alice->>Bob: Hello` |
| 类图 | classDiagram | `classDiagram class Animal` |
| 状态图 | stateDiagram | `stateDiagram [*]-->State1` |
| ER图 | erDiagram | `erDiagram CUSTOMER \|\|--o{ ORDER` |
| 饼图 | pie | `pie "Dogs" : 386 "Cats" : 85` |
| 甘特图 | gantt | `gantt title A Gantt Diagram` |
| 思维导图 | mindmap | `mindmap root((main))` |
| 以及更多... | | 共支持 25+ 种图表类型 |

## 系统要求

- **Notepad++** 8.0 或更高版本 (32位/64位)
- **.NET Framework** 4.6.2 或更高版本
- **Windows** 7/8/10/11

## 安装

### 方法一：手动安装

1. 下载最新版本的 [Release](https://github.com/your-repo/MermaidViewer/releases)

2. 解压到 Notepad++ 的 plugins 目录下：
   ```
   C:\Program Files\Notepad++\plugins\MermaidViewer\
   ```

3. 目录结构应包含：
   ```
   MermaidViewer/
   ├── MermaidViewer.dll    # 插件主文件
   ├── mmdr.exe             # 渲染引擎
   └── README.md
   ```

4. 重启 Notepad++

### 方法二：使用插件管理器

1. 打开 Notepad++
2. 进入 `插件 > 插件管理` (Plugins Admin)
3. 搜索 "Mermaid Viewer"
4. 点击安装

## 使用方法

### 快速开始

1. 打开或创建一个 `.mmd` 文件
2. 编写 Mermaid 代码：
   ```mermaid
   flowchart TD
       A[开始] --> B{决策}
       B -->|是| C[完成]
       B -->|否| D[重试]
       D --> B
   ```
3. 按 `Ctrl+Shift+M` 打开预览面板

### 快捷键

| 快捷键 | 功能 |
|--------|------|
| `Ctrl+Shift+M` | 显示/隐藏预览面板 |
| `Ctrl+F5` | 刷新预览 |
| `Ctrl+鼠标滚轮` | 缩放图表 |
| `双击` | 重置视图 |
| `右键拖拽` | 平移图表 |

### 导出图表

1. 在预览面板中右键点击
2. 选择 "Export as SVG" 或 "Export as PNG"
3. 选择保存位置

### 多图表导航

如果文件包含多个图表（使用 `@startxxx`/`@endxxx` 分隔）：

- 使用预览面板上的导航按钮
- 或使用菜单：`Previous Diagram` / `Next Diagram`

## 配置

### 打开设置

1. 进入 `插件 > Mermaid Viewer > Settings...`

### 可配置选项

| 选项 | 默认值 | 说明 |
|------|--------|------|
| 自动刷新 | 启用 | 编辑时自动重新渲染 |
| 深色模式 | 自动 | 跟随 Notepad++ 主题 |
| 刷新延迟 | 500ms | 触发渲染的延迟时间 |
| PNG 缩放 | 2.0x | 导出 PNG 时的缩放比例 |
| mmdr 路径 | 自动 | 渲染器可执行文件位置 |

## 示例文件

插件自带多个示例文件，位于 `examples/` 目录：

- `flowchart.mmd` - 流程图示例
- `sequence.mmd` - 时序图示例
- `class.mmd` - 类图示例
- `state.mmd` - 状态图示例
- `entity.mmd` - ER 图示例
- `pie.mmd` - 饼图示例

## 故障排除

### 提示 "mmdr.exe not found"

1. 确认 `mmdr.exe` 位于 `plugins/MermaidViewer/` 目录
2. 在设置中检查 mmdr 路径是否正确

### 渲染失败

1. 检查 Mermaid 语法是否正确
2. 查看预览面板底部的状态栏错误信息
3. 尝试在 [Mermaid Live Editor](https://mermaid.live/) 验证语法

### 插件无法加载

1. 确认 Notepad++ 版本 ≥ 8.0
2. 确认 .NET Framework 版本 ≥ 4.6.2
3. 检查插件目录结构是否正确

## 技术栈

- **插件框架**: NotepadPlusPlusPluginPack.Net
- **渲染引擎**: mermaid-rs-renderer (mmdr)
- **开发语言**: C# / .NET
- **UI**: WinForms

## 性能对比

| 渲染器 | 冷启动 | 渲染时间 | 内存占用 |
|--------|--------|----------|----------|
| mmdr (本插件) | ~3ms | ~5ms | ~15MB |
| mermaid-cli | ~2000ms | ~2000ms | ~300MB |

## 许可证

MIT License - 详见 [LICENSE](LICENSE)

## 参考项目

- [mermaid-rs-renderer](https://github.com/1jehuang/mermaid-rs-renderer) - Rust Mermaid 渲染器
- [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net) - Notepad++ 插件开发框架
- [PlantUmlViewer](https://github.com/Fruchtzwerg94/PlantUmlViewer) - 类似插件参考

## 贡献

欢迎提交 Issue 和 Pull Request！

## 版本历史

### v1.0.0 (2024)
- 初始版本
- 支持实时预览
- 支持 SVG/PNG 导出
- 支持深色模式
