using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MermaidSettings = MermaidViewer.MermaidSettings;

namespace MermaidViewer.Forms
{
    /// <summary>
    /// Settings dialog for Mermaid Viewer plugin
    /// </summary>
    public partial class SettingsForm : Form
    {
        private MermaidSettings _settings;

        /// <summary>
        /// Gets the modified settings
        /// </summary>
        public MermaidSettings Settings => _settings;

        public SettingsForm(MermaidSettings settings)
        {
            InitializeComponent();
            _settings = settings.Clone();

            LoadSettings();
        }

        private void LoadSettings()
        {
            // General settings
            chkAutoRefresh.Checked = _settings.AutoRefresh;
            chkDarkMode.Checked = _settings.DarkMode;
            numRefreshDelay.Value = _settings.RefreshDelayMs;

            // Export settings
            numPngScale.Value = (decimal)_settings.DefaultPngScale;
            txtExportPath.Text = _settings.DefaultExportPath;

            // Renderer settings
            txtMmdrPath.Text = _settings.MmdrPath;
            chkFollowDarkMode.Checked = _settings.FollowNotepadDarkMode;
        }

        private void SaveSettings()
        {
            _settings.AutoRefresh = chkAutoRefresh.Checked;
            _settings.DarkMode = chkDarkMode.Checked;
            _settings.RefreshDelayMs = (int)numRefreshDelay.Value;
            _settings.DefaultPngScale = (double)numPngScale.Value;
            _settings.DefaultExportPath = txtExportPath.Text;
            _settings.MmdrPath = txtMmdrPath.Text;
            _settings.FollowNotepadDarkMode = chkFollowDarkMode.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowseMmdr_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                dialog.Title = "Select mmdr.exe";

                if (!string.IsNullOrEmpty(txtMmdrPath.Text) && File.Exists(txtMmdrPath.Text))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(txtMmdrPath.Text);
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtMmdrPath.Text = dialog.FileName;
                }
            }
        }

        private void btnBrowseExport_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select default export folder";

                if (!string.IsNullOrEmpty(txtExportPath.Text) && Directory.Exists(txtExportPath.Text))
                {
                    dialog.SelectedPath = txtExportPath.Text;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtExportPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnTestMmdr_Click(object sender, EventArgs e)
        {
            string mmdrPath = txtMmdrPath.Text;

            if (string.IsNullOrEmpty(mmdrPath))
            {
                MessageBox.Show("Please specify the path to mmdr.exe first.", "Test", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(mmdrPath))
            {
                MessageBox.Show($"mmdr.exe not found at:\n{mmdrPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = mmdrPath,
                        Arguments = "--version",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                bool completed = process.WaitForExit(5000);

                if (completed && process.ExitCode == 0)
                {
                    string version = process.StandardOutput.ReadToEnd().Trim();
                    MessageBox.Show($"mmdr.exe is working!\n\nVersion: {version}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string error = process.StandardError.ReadToEnd();
                    MessageBox.Show($"mmdr.exe test failed.\n\nError: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to test mmdr.exe:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Plugin settings
    /// </summary>
    [Serializable]
    public class MermaidSettings
    {
        public bool AutoRefresh { get; set; } = true;
        public bool DarkMode { get; set; } = false;
        public int RefreshDelayMs { get; set; } = 500;
        public double DefaultPngScale { get; set; } = 2.0;
        public string DefaultExportPath { get; set; } = "";
        public string MmdrPath { get; set; } = "";
        public bool FollowNotepadDarkMode { get; set; } = true;

        /// <summary>
        /// Creates a deep clone of the settings
        /// </summary>
        public MermaidSettings Clone()
        {
            return new MermaidSettings
            {
                AutoRefresh = AutoRefresh,
                DarkMode = DarkMode,
                RefreshDelayMs = RefreshDelayMs,
                DefaultPngScale = DefaultPngScale,
                DefaultExportPath = DefaultExportPath,
                MmdrPath = MmdrPath,
                FollowNotepadDarkMode = FollowNotepadDarkMode
            };
        }

        /// <summary>
        /// Saves settings to file
        /// </summary>
        public void Save(string filePath)
        {
            try
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MermaidSettings));
                using (var stream = File.Create(filePath))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads settings from file
        /// </summary>
        public static MermaidSettings Load(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MermaidSettings));
                    using (var stream = File.OpenRead(filePath))
                    {
                        return (MermaidSettings)serializer.Deserialize(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load settings: {ex.Message}");
            }

            return new MermaidSettings();
        }
    }
}
