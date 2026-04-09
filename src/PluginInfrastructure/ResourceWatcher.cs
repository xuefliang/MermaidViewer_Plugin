using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MermaidViewer
{
    /// <summary>
    /// File system watcher for Notepad++ file changes
    /// </summary>
    public class ResourceWatcher : IDisposable
    {
        private FileSystemWatcher _watcher;
        private Timer _debounceTimer;
        private readonly string _filePath;
        private readonly Action _onChanged;
        private readonly int _debounceMs;

        /// <summary>
        /// Creates a new resource watcher
        /// </summary>
        /// <param name="filePath">The file to watch</param>
        /// <param name="onChanged">Callback when file changes</param>
        /// <param name="debounceMs">Debounce delay in milliseconds</param>
        public ResourceWatcher(string filePath, Action onChanged, int debounceMs = 500)
        {
            _filePath = filePath;
            _onChanged = onChanged;
            _debounceMs = debounceMs;

            InitializeWatcher();
        }

        private void InitializeWatcher()
        {
            if (!File.Exists(_filePath))
                return;

            string directory = Path.GetDirectoryName(_filePath);
            string fileName = Path.GetFileName(_filePath);

            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(fileName))
                return;

            _watcher = new FileSystemWatcher(directory, fileName)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            _watcher.Changed += OnFileChanged;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // Debounce to avoid multiple rapid events
            _debounceTimer?.Dispose();
            _debounceTimer = new Timer(_ =>
            {
                try
                {
                    _onChanged?.Invoke();
                }
                catch
                {
                    // Ignore exceptions in callback
                }
            }, null, _debounceMs, Timeout.Infinite);
        }

        /// <summary>
        /// Stops watching for changes
        /// </summary>
        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= OnFileChanged;
                _watcher.Dispose();
                _watcher = null;
            }

            _debounceTimer?.Dispose();
            _debounceTimer = null;
        }

        /// <summary>
        /// Restarts watching
        /// </summary>
        public void Restart()
        {
            Stop();
            InitializeWatcher();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
