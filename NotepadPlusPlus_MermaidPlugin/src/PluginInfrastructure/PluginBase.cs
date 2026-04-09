using System;

namespace MermaidViewer
{
    /// <summary>
    /// Base class for plugin data
    /// </summary>
    public static class PluginBase
    {
        /// <summary>
        /// Plugin name
        /// </summary>
        public static string PluginName => "Mermaid Viewer";

        /// <summary>
        /// Notepad++ data structure
        /// </summary>
        public static NppData NppData { get; private set; }

        /// <summary>
        /// Sets the plugin data from Notepad++
        /// </summary>
        public static void SetNppData(NppData nppData)
        {
            NppData = nppData;
        }
    }
}
