using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MermaidViewer.Forms
{
    /// <summary>
    /// Settings dialog for Mermaid Viewer plugin
    /// </summary>
    public partial class SettingsForm : Form
    {
        private MermaidViewer.MermaidSettings _settings;

        /// <summary>
        /// Gets the modified settings
        /// </summary>
        public MermaidViewer.MermaidSettings Settings => _settings;

        public SettingsForm(MermaidViewer.MermaidSettings settings)
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
}
