using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MermaidViewer
{
    /// <summary>
    /// Renders Mermaid diagrams using the mmdr CLI tool
    /// </summary>
    public static class MmdrRenderer
    {
        private static string _lastError = string.Empty;

        /// <summary>
        /// Gets the last error message
        /// </summary>
        public static string LastError => _lastError;

        /// <summary>
        /// Renders Mermaid source to SVG
        /// </summary>
        public static string Render(string mermaidCode)
        {
            _lastError = string.Empty;

            if (string.IsNullOrWhiteSpace(mermaidCode))
            {
                return GenerateErrorSvg("No Mermaid code to render");
            }

            string mmdrPath = GetMmdrPath();
            if (!File.Exists(mmdrPath))
            {
                return GenerateErrorSvg("mmdr.exe not found at: " + mmdrPath);
            }

            // Create temp files
            string tempInput = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mmd");
            string tempOutput = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".svg");

            try
            {
                // Write input file
                File.WriteAllText(tempInput, mermaidCode, Encoding.UTF8);

                // Execute mmdr
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = mmdrPath,
                    Arguments = string.Format("\"{0}\" -o \"{1}\"", tempInput, tempOutput),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (Process process = Process.Start(psi))
                {
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(30000); // 30 second timeout

                    if (!process.HasExited)
                    {
                        process.Kill();
                        return GenerateErrorSvg("Rendering timed out");
                    }

                    if (process.ExitCode != 0)
                    {
                        string errorMsg = string.IsNullOrEmpty(stderr) ? "Unknown error" : stderr;
                        return GenerateErrorSvg("mmdr error: " + errorMsg);
                    }
                }

                // Read output
                if (File.Exists(tempOutput))
                {
                    return File.ReadAllText(tempOutput, Encoding.UTF8);
                }
                else
                {
                    return GenerateErrorSvg("No output generated");
                }
            }
            catch (Exception ex)
            {
                return GenerateErrorSvg("Exception: " + ex.Message);
            }
            finally
            {
                // Cleanup temp files
                try { if (File.Exists(tempInput)) File.Delete(tempInput); } catch { }
                try { if (File.Exists(tempOutput)) File.Delete(tempOutput); } catch { }
            }
        }

        private static string GetMmdrPath()
        {
            // Try plugin directory
            string pluginDir = AppDomain.CurrentDomain.BaseDirectory;
            string mmdrPath = Path.Combine(pluginDir, "tools", "mmdr", "mmdr.exe");

            if (File.Exists(mmdrPath))
                return mmdrPath;

            // Try mmdr.exe directly in plugin directory
            mmdrPath = Path.Combine(pluginDir, "mmdr.exe");
            if (File.Exists(mmdrPath))
                return mmdrPath;

            // Try relative path
            mmdrPath = Path.Combine(pluginDir, "..", "..", "..", "tools", "mmdr", "mmdr.exe");
            if (File.Exists(mmdrPath))
                return mmdrPath;

            // Default to plugin directory (will be found at runtime)
            return Path.Combine(pluginDir, "tools", "mmdr", "mmdr.exe");
        }

        private static string GenerateErrorSvg(string message)
        {
            _lastError = message;
            return string.Format(
                @"<svg xmlns=""http://www.w3.org/2000/svg"" width=""400"" height=""100"">
                    <rect width=""100%"" height=""100%"" fill=""#ffeeee"" stroke=""#cc0000"" stroke-width=""2""/>
                    <text x=""10"" y=""40"" fill=""#cc0000"" font-family=""Arial"" font-size=""14"">Error</text>
                    <text x=""10"" y=""60"" fill=""#660000"" font-family=""Arial"" font-size=""12"">{0}</text>
                    <text x=""10"" y=""85"" fill=""#666666"" font-family=""Arial"" font-size=""10"">Check that mmdr.exe exists in the tools\mmdr\ folder</text>
                </svg>",
                EscapeXml(message)
            );
        }

        private static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }
    }
}
