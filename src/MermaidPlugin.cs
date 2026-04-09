using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MermaidViewer.Forms;

namespace MermaidViewer
{
    /// <summary>
    /// Main plugin class for Mermaid Viewer
    /// </summary>
    public class MermaidPlugin : PluginBase
    {
        /// <summary>
        /// Singleton instance of the plugin
        /// </summary>
        public static MermaidPlugin Plugin { get; private set; }

        /// <summary>
        /// Plugin version
        /// </summary>
        public const string VERSION = "1.0.0";

        private MermaidPreviewForm _previewForm;
        private MermaidSettings _settings;
        private ResourceWatcher _fileWatcher;
        private System.Windows.Forms.Timer _debounceTimer;
        private bool _isRefreshing = false;
        private IntPtr _previewWindowHandle;

        /// <summary>
        /// Plugin name
        /// </summary>
        public new static string PluginName => "Mermaid Viewer";

        /// <summary>
        /// Gets the current settings
        /// </summary>
        public MermaidSettings Settings => _settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public MermaidPlugin()
        {
            Plugin = this;
        }

        /// <summary>
        /// Gets the plugin version
        /// </summary>
        public string GetVersion() => VERSION;

        /// <summary>
        /// Initializes the plugin
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Load settings
            LoadSettings();

            // Setup menu items
            SetupMenu();

            // Create preview form
            _previewForm = new MermaidPreviewForm();
            _previewForm.RefreshRequested += OnRefreshRequested;
            _previewForm.ExportSvgRequested += OnExportSvgRequested;
            _previewForm.ExportPngRequested += OnExportPngRequested;
            _previewForm.IsDarkMode = ShouldUseDarkMode();
        }

        /// <summary>
        /// Loads plugin settings
        /// </summary>
        private void LoadSettings()
        {
            string configDir = GetPluginsConfigDir();
            string settingsPath = Path.Combine(configDir, "settings.xml");
            _settings = MermaidSettings.Load(settingsPath);

            // Set default mmdr path if not set
            if (string.IsNullOrEmpty(_settings.MmdrPath))
            {
                string mmdrPath = GetToolPath("mmdr.exe");
                if (File.Exists(mmdrPath))
                {
                    _settings.MmdrPath = mmdrPath;
                }
            }
        }

        /// <summary>
        /// Saves plugin settings
        /// </summary>
        private void SaveSettings()
        {
            string configDir = GetPluginsConfigDir();
            string settingsPath = Path.Combine(configDir, "settings.xml");
            _settings.Save(settingsPath);
        }

        /// <summary>
        /// Sets up the plugin menu items
        /// </summary>
        private void SetupMenu()
        {
            _menuItems.Clear();

            // Preview Mermaid
            _menuItems.Add(MenuItemBase.Create(
                "Preview Mermaid",
                "Show Mermaid preview panel",
                ShowPreview,
                true, true, false, 0x4D // Ctrl+Shift+M (M = 0x4D)
            ));

            // Refresh
            _menuItems.Add(MenuItemBase.Create(
                "Refresh Preview",
                "Refresh the Mermaid preview",
                RefreshPreview,
                true, false, false, 0x74 // Ctrl+F5 (F5 = 0x74)
            ));

            _menuItems.Add(new MenuItemBase()); // Separator

            // Export as SVG
            _menuItems.Add(MenuItemBase.Create(
                "Export as SVG...",
                "Export diagram as SVG file",
                ExportAsSvg
            ));

            // Export as PNG
            _menuItems.Add(MenuItemBase.Create(
                "Export as PNG...",
                "Export diagram as PNG file",
                ExportAsPng
            ));

            _menuItems.Add(new MenuItemBase()); // Separator

            // Previous diagram
            _menuItems.Add(MenuItemBase.Create(
                "Previous Diagram",
                "Show previous diagram",
                PreviousDiagram
            ));

            // Next diagram
            _menuItems.Add(MenuItemBase.Create(
                "Next Diagram",
                "Show next diagram",
                NextDiagram
            ));

            _menuItems.Add(new MenuItemBase()); // Separator

            // Zoom controls
            _menuItems.Add(MenuItemBase.Create(
                "Zoom In",
                "Zoom in",
                ZoomIn
            ));

            _menuItems.Add(MenuItemBase.Create(
                "Zoom Out",
                "Zoom out",
                ZoomOut
            ));

            _menuItems.Add(MenuItemBase.Create(
                "Reset View",
                "Reset zoom and pan",
                ResetView
            ));

            _menuItems.Add(new MenuItemBase()); // Separator

            // Settings
            _menuItems.Add(MenuItemBase.Create(
                "Settings...",
                "Open plugin settings",
                ShowSettings
            ));

            // About
            _menuItems.Add(MenuItemBase.Create(
                "About",
                "About Mermaid Viewer",
                ShowAbout
            ));
        }

        /// <summary>
        /// Checks if dark mode should be used
        /// </summary>
        private bool ShouldUseDarkMode()
        {
            if (_settings.FollowNotepadDarkMode)
            {
                // Try to detect Notepad++ dark mode
                try
                {
                    IntPtr nppHandle = _nppData._nppHandle;
                    if (nppHandle != IntPtr.Zero)
                    {
                        // Simple check: get background color
                        // This is a simplified check
                        return false; // Default to light mode unless detected otherwise
                    }
                }
                catch { }
            }
            return _settings.DarkMode;
        }

        /// <summary>
        /// Shows the preview panel
        /// </summary>
        public void ShowPreview()
        {
            if (_previewWindowHandle == IntPtr.Zero)
            {
                CreatePreviewWindow();
            }

            if (_previewWindowHandle != IntPtr.Zero)
            {
                ShowDockableWindow(_previewWindowHandle);
            }

            // Auto-refresh on show
            if (_settings.AutoRefresh)
            {
                RefreshPreview();
            }
        }

        /// <summary>
        /// Creates the dockable preview window
        /// </summary>
        private void CreatePreviewWindow()
        {
            try
            {
                // Get the native window handle of our preview form
                MethodInfo mi = typeof(Control).GetMethod("CreateControl", 
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                mi.Invoke(_previewForm, new object[] { true });

                _previewWindowHandle = _previewForm.Handle;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create preview window: {ex.Message}");
            }
        }

        /// <summary>
        /// Shows a dockable window
        /// </summary>
        private void ShowDockableWindow(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return;

            // Show the window
            Win32.ShowWindow(hwnd, 1); // SW_SHOW

            // Bring to front
            Win32.SetWindowPos(hwnd, new IntPtr(-1), 0, 0, 0, 0, 
                Win32.SWP_NOSIZE | Win32.SWP_NOMOVE);
        }

        /// <summary>
        /// Hides the preview panel
        /// </summary>
        public void HidePreview()
        {
            if (_previewWindowHandle != IntPtr.Zero)
            {
                Win32.ShowWindow(_previewWindowHandle, 0); // SW_HIDE
            }
        }

        /// <summary>
        /// Refreshes the preview
        /// </summary>
        public void RefreshPreview()
        {
            if (_isRefreshing || _previewForm == null)
                return;

            _isRefreshing = true;

            try
            {
                // Get current editor text
                string text = _scintillaGateway?.GetText() ?? "";

                if (string.IsNullOrWhiteSpace(text))
                {
                    _ = _previewForm.RenderAsync("");
                    return;
                }

                // Debounce if auto-refresh is enabled
                if (_settings.AutoRefresh)
                {
                    _debounceTimer?.Dispose();
                    _debounceTimer = new System.Windows.Forms.Timer();
                    _debounceTimer.Interval = _settings.RefreshDelayMs;
                    _debounceTimer.Tick += async (s, e) =>
                    {
                        _debounceTimer.Stop();
                        await _previewForm.RenderAsync(text);
                    };
                    _debounceTimer.Start();
                }
                else
                {
                    _ = _previewForm.RenderAsync(text);
                }
            }
            finally
            {
                _isRefreshing = false;
            }
        }

        private void OnRefreshRequested(object sender, EventArgs e)
        {
            RefreshPreview();
        }

        private void OnExportSvgRequested(object sender, EventArgs e)
        {
            ExportAsSvg();
        }

        private void OnExportPngRequested(object sender, EventArgs e)
        {
            ExportAsPng();
        }

        /// <summary>
        /// Exports as SVG
        /// </summary>
        private async void ExportAsSvg()
        {
            if (_previewForm == null || string.IsNullOrEmpty(_previewForm.CurrentSvg))
            {
                MessageBox.Show("No diagram to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
                dialog.DefaultExt = ".svg";

                // Get suggested filename
                string currentFile = _notepadGateway?.GetCurrentFileName();
                if (!string.IsNullOrEmpty(currentFile))
                {
                    string baseName = Path.GetFileNameWithoutExtension(currentFile);
                    dialog.FileName = baseName + ".svg";
                }

                if (!string.IsNullOrEmpty(_settings.DefaultExportPath) && Directory.Exists(_settings.DefaultExportPath))
                {
                    dialog.InitialDirectory = _settings.DefaultExportPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _previewForm.ExportToSvgAsync(dialog.FileName);
                        MessageBox.Show($"Exported successfully to:\n{dialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Exports as PNG
        /// </summary>
        private async void ExportAsPng()
        {
            if (_previewForm == null || string.IsNullOrEmpty(_previewForm.CurrentSvg))
            {
                MessageBox.Show("No diagram to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
                dialog.DefaultExt = ".png";

                string currentFile = _notepadGateway?.GetCurrentFileName();
                if (!string.IsNullOrEmpty(currentFile))
                {
                    string baseName = Path.GetFileNameWithoutExtension(currentFile);
                    dialog.FileName = baseName + ".png";
                }

                if (!string.IsNullOrEmpty(_settings.DefaultExportPath) && Directory.Exists(_settings.DefaultExportPath))
                {
                    dialog.InitialDirectory = _settings.DefaultExportPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _previewForm.ExportToPngAsync(dialog.FileName, _settings.DefaultPngScale);
                        MessageBox.Show($"Exported successfully to:\n{dialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Export failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Shows previous diagram
        /// </summary>
        public void PreviousDiagram()
        {
            _previewForm?.PreviousDiagram();
        }

        /// <summary>
        /// Shows next diagram
        /// </summary>
        public void NextDiagram()
        {
            _previewForm?.NextDiagram();
        }

        /// <summary>
        /// Zooms in
        /// </summary>
        public void ZoomIn()
        {
            _previewForm?.ZoomIn();
        }

        /// <summary>
        /// Zooms out
        /// </summary>
        public void ZoomOut()
        {
            _previewForm?.ZoomOut();
        }

        /// <summary>
        /// Resets the view
        /// </summary>
        public void ResetView()
        {
            _previewForm?.ResetView();
        }

        /// <summary>
        /// Shows the settings dialog
        /// </summary>
        private void ShowSettings()
        {
            using (SettingsForm form = new SettingsForm(_settings))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _settings = form.Settings;
                    SaveSettings();

                    // Apply dark mode change
                    if (_previewForm != null)
                    {
                        _previewForm.IsDarkMode = ShouldUseDarkMode();
                    }
                }
            }
        }

        /// <summary>
        /// Shows the about dialog
        /// </summary>
        private void ShowAbout()
        {
            MessageBox.Show(
                $"Mermaid Viewer Plugin\nVersion {VERSION}\n\n" +
                "A fast Mermaid diagram viewer for Notepad++\n\n" +
                "Powered by mermaid-rs-renderer\n" +
                "500-1000x faster than mermaid-cli\n\n" +
                "Features:\n" +
                "- Real-time preview\n" +
                "- SVG/PNG export\n" +
                "- Multiple diagram support\n" +
                "- Dark mode support\n",
                "About Mermaid Viewer",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Handles notifications from Notepad++
        /// </summary>
        public IntPtr HandleNotification(int messageType, IntPtr wParam, IntPtr lParam)
        {
            switch (messageType)
            {
                case NppMessages.NPPN_FILEOPENED:
                    OnFileOpened();
                    break;

                case NppMessages.NPPN_BUFFERACTIVATED:
                    OnBufferActivated();
                    break;

                case NppMessages.NPPN_FILESAVED:
                    OnFileSaved();
                    break;

                case NppMessages.NPPN_SHUTDOWN:
                    OnShutdown();
                    break;
            }

            return IntPtr.Zero;
        }

        private void OnFileOpened()
        {
            RefreshScintillaGateway();

            if (_settings.AutoRefresh && PluginBase.IsMermaidFile())
            {
                RefreshPreview();
            }

            // Setup file watcher
            SetupFileWatcher();
        }

        private void OnBufferActivated()
        {
            RefreshScintillaGateway();

            if (_settings.AutoRefresh && PluginBase.IsMermaidFile())
            {
                RefreshPreview();
            }
        }

        private void OnFileSaved()
        {
            if (_settings.AutoRefresh && PluginBase.IsMermaidFile())
            {
                RefreshPreview();
            }
        }

        private void OnShutdown()
        {
            SaveSettings();
            Cleanup();
        }

        private void SetupFileWatcher()
        {
            _fileWatcher?.Dispose();

            try
            {
                string currentFile = _notepadGateway?.GetFullCurrentPath();
                if (!string.IsNullOrEmpty(currentFile) && File.Exists(currentFile))
                {
                    _fileWatcher = new ResourceWatcher(currentFile, () =>
                    {
                        if (_settings.AutoRefresh && _previewForm != null)
                        {
                            _previewForm.Invoke(new Action(RefreshPreview));
                        }
                    });
                }
            }
            catch { }
        }

        /// <summary>
        /// Cleans up resources
        /// </summary>
        public override void Cleanup()
        {
            _fileWatcher?.Dispose();
            _debounceTimer?.Dispose();
            _previewForm?.Dispose();
            base.Cleanup();
        }
    }
}
