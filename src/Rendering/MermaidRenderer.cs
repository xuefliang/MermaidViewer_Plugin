using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace MermaidViewer.Rendering
{
    /// <summary>
    /// Renders Mermaid diagrams using the mmdr CLI tool
    /// mmdr (mermaid-rs-renderer) is 500-1000x faster than mermaid-cli
    /// </summary>
    public class MermaidRenderer : IDisposable
    {
        private readonly string _mmdrPath;
        private readonly int _timeoutMs;
        private string _lastError;

        /// <summary>
        /// Gets the last error message
        /// </summary>
        public string LastError => _lastError;

        /// <summary>
        /// Creates a new Mermaid renderer
        /// </summary>
        /// <param name="mmdrPath">Path to the mmdr.exe executable</param>
        /// <param name="timeoutMs">Timeout for rendering in milliseconds</param>
        public MermaidRenderer(string mmdrPath, int timeoutMs = 30000)
        {
            _mmdrPath = mmdrPath;
            _timeoutMs = timeoutMs;
        }

        /// <summary>
        /// Renders Mermaid source to SVG
        /// </summary>
        /// <param name="mermaidSource">The Mermaid diagram source code</param>
        /// <returns>SVG content or null if failed</returns>
        public async Task<string> RenderToSvgAsync(string mermaidSource)
        {
            if (string.IsNullOrWhiteSpace(mermaidSource))
            {
                _lastError = "Mermaid source is empty";
                return null;
            }

            return await Task.Run(() => RenderToSvg(mermaidSource));
        }

        /// <summary>
        /// Renders Mermaid source to SVG (synchronous)
        /// </summary>
        public string RenderToSvg(string mermaidSource)
        {
            _lastError = null;

            if (!File.Exists(_mmdrPath))
            {
                _lastError = $"mmdr.exe not found at: {_mmdrPath}";
                return null;
            }

            // Create temp file for input
            string tempInputPath = Path.GetTempFileName();
            string tempOutputPath = Path.GetTempFileName();

            try
            {
                // Write input
                File.WriteAllText(tempInputPath, mermaidSource);

                // Execute mmdr
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _mmdrPath,
                        Arguments = $"\"{tempInputPath}\" -o \"{tempOutputPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        StandardErrorEncoding = System.Text.Encoding.UTF8
                    }
                };

                process.Start();
                bool completed = process.WaitForExit(_timeoutMs);

                if (!completed)
                {
                    process.Kill();
                    _lastError = "Rendering timed out";
                    return null;
                }

                string stderr = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(stderr))
                {
                    _lastError = stderr;
                }

                if (process.ExitCode != 0)
                {
                    _lastError = $"mmdr exited with code {process.ExitCode}: {stderr}";
                    return null;
                }

                // Read output
                if (File.Exists(tempOutputPath))
                {
                    string svg = File.ReadAllText(tempOutputPath);
                    return svg;
                }
                else
                {
                    _lastError = "No output file generated";
                    return null;
                }
            }
            catch (Exception ex)
            {
                _lastError = $"Exception: {ex.Message}";
                return null;
            }
            finally
            {
                // Cleanup temp files
                try { File.Delete(tempInputPath); } catch { }
                try { File.Delete(tempOutputPath); } catch { }
            }
        }

        /// <summary>
        /// Renders Mermaid source to PNG
        /// </summary>
        /// <param name="mermaidSource">The Mermaid diagram source code</param>
        /// <param name="scale">Scale factor for PNG output (default 1.0)</param>
        /// <returns>PNG bytes or null if failed</returns>
        public async Task<byte[]> RenderToPngAsync(string mermaidSource, double scale = 1.0)
        {
            return await Task.Run(() => RenderToPng(mermaidSource, scale));
        }

        /// <summary>
        /// Renders Mermaid source to PNG (synchronous)
        /// </summary>
        public byte[] RenderToPng(string mermaidSource, double scale = 1.0)
        {
            _lastError = null;

            if (!File.Exists(_mmdrPath))
            {
                _lastError = $"mmdr.exe not found at: {_mmdrPath}";
                return null;
            }

            string tempInputPath = Path.GetTempFileName();
            string tempOutputPath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempInputPath, mermaidSource);

                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _mmdrPath,
                        Arguments = $"--format png --scale {scale} \"{tempInputPath}\" -o \"{tempOutputPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                bool completed = process.WaitForExit(_timeoutMs);

                if (!completed)
                {
                    process.Kill();
                    _lastError = "Rendering timed out";
                    return null;
                }

                if (File.Exists(tempOutputPath))
                {
                    return File.ReadAllBytes(tempOutputPath);
                }
                else
                {
                    _lastError = "No PNG output generated";
                    return null;
                }
            }
            catch (Exception ex)
            {
                _lastError = $"Exception: {ex.Message}";
                return null;
            }
            finally
            {
                try { File.Delete(tempInputPath); } catch { }
                try { File.Delete(tempOutputPath); } catch { }
            }
        }

        /// <summary>
        /// Validates Mermaid syntax
        /// </summary>
        public async Task<bool> ValidateAsync(string mermaidSource)
        {
            return await Task.Run(() => Validate(mermaidSource));
        }

        /// <summary>
        /// Validates Mermaid syntax (synchronous)
        /// </summary>
        public bool Validate(string mermaidSource)
        {
            _lastError = null;

            if (!File.Exists(_mmdrPath))
            {
                _lastError = $"mmdr.exe not found at: {_mmdrPath}";
                return false;
            }

            string tempInputPath = Path.GetTempFileName();

            try
            {
                File.WriteAllText(tempInputPath, mermaidSource);

                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _mmdrPath,
                        Arguments = $"--validate \"{tempInputPath}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                bool completed = process.WaitForExit(5000); // Shorter timeout for validation

                if (!completed)
                {
                    process.Kill();
                    return false;
                }

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                try { File.Delete(tempInputPath); } catch { }
            }
        }

        /// <summary>
        /// Extracts a single Mermaid diagram from source that may contain multiple diagrams
        /// </summary>
        public static string ExtractDiagram(string source, int diagramIndex = 0)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            // Find @startxxx / @endxxx blocks
            int startIdx = 0;
            int endIdx = source.Length;
            int currentDiagram = -1;

            int searchPos = 0;
            while (searchPos < source.Length)
            {
                // Look for @start
                int blockStart = source.IndexOf('@', searchPos);
                if (blockStart == -1) break;

                // Check if it's @startXXX pattern
                if (blockStart + 6 <= source.Length && source.Substring(blockStart, 5) == "@start")
                {
                    currentDiagram++;
                    if (currentDiagram == diagramIndex)
                    {
                        // Find the start of this diagram
                        int lineStart = source.LastIndexOf('\n', blockStart) + 1;
                        if (lineStart < blockStart) lineStart = blockStart;

                        // Find the matching @end
                        string diagramType = source.Substring(blockStart + 5, 3);
                        string endTag = "@end" + diagramType;
                        int blockEnd = source.IndexOf(endTag, blockStart);
                        if (blockEnd != -1)
                        {
                            endIdx = blockEnd;
                        }
                        else
                        {
                            endIdx = source.IndexOf("@enduml", blockStart);
                            if (endIdx == -1) endIdx = source.Length;
                        }
                        startIdx = lineStart;
                        break;
                    }
                }
                searchPos = blockStart + 1;
            }

            if (diagramIndex == 0 && currentDiagram == -1)
            {
                // No @start/@end blocks found, return the whole source
                return source.Trim();
            }

            return source.Substring(startIdx, endIdx - startIdx).Trim();
        }

        /// <summary>
        /// Counts the number of Mermaid diagrams in source
        /// </summary>
        public static int CountDiagrams(string source)
        {
            if (string.IsNullOrEmpty(source))
                return 0;

            int count = 0;
            int searchPos = 0;

            while (searchPos < source.Length)
            {
                int blockStart = source.IndexOf("@start", searchPos);
                if (blockStart == -1) break;
                count++;
                searchPos = blockStart + 6;
            }

            return count > 0 ? count : 1; // If no @start/@end found, there's one diagram
        }

        public void Dispose()
        {
            // Nothing to dispose for this implementation
        }
    }
}
