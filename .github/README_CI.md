# 自动构建说明

## 使用方法

### 方式一：推送到 GitHub 自动构建

1. 在 GitHub 创建新仓库（比如 `notepadpp-mermaid-plugin`）

2. 推送代码：
   ```bash
   cd NotepadPlusPlus_MermaidPlugin
   git init
   git add .
   git commit -m "Initial commit: MermaidViewer plugin"
   git branch -M main
   git remote add origin https://github.com/你的用户名/notepadpp-mermaid-plugin.git
   git push -u origin main
   ```

3. GitHub Actions 会自动触发构建

4. 构建完成后，在 **Releases** 页面下载 `MermaidViewer-Plugin.zip`

### 方式二：手动触发构建

1. 进入你的 GitHub 仓库
2. 点击 **Actions** 标签
3. 选择 **Build MermaidViewer Plugin**
4. 点击 **Run workflow** 按钮
5. 等待构建完成，下载 Artifact

## 构建产物

构建完成后会生成：
```
MermaidViewer-Plugin.zip
├── MermaidViewer/
│   ├── MermaidViewer.dll    # 插件主文件
│   ├── tools/mmdr/mmdr.exe  # 渲染引擎
│   ├── README.md
│   ├── INSTALL.md
│   └── examples/            # 示例文件
```

## 安装到 Notepad++

1. 解压 `MermaidViewer-Plugin.zip`
2. 将 `MermaidViewer` 文件夹复制到 Notepad++ 插件目录：
   ```
   C:\Program Files\Notepad++\plugins\MermaidViewer\
   ```
3. 重启 Notepad++
4. 菜单中会出现 **Plugins → MermaidViewer**

## 注意事项

- 需要 Notepad++ 8.0 或更高版本
- 插件会自动下载 mmdr.exe，首次使用需联网
- 如果自动下载失败，手动下载 [mmdr.exe](https://github.com/1jehuang/mermaid-rs-renderer/releases) 放到 `tools/mmdr/` 目录
