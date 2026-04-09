using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MermaidViewer
{
    /// <summary>
    /// Base class for Notepad++ plugins
    /// </summary>
    public abstract class PluginBase
    {
        /// <summary>
        /// Plugin name - override in derived class
        /// </summary>
        public static string PluginName { get; protected set; }

        /// <summary>
        /// Notepad++ data structure
        /// </summary>
        public static NppData _nppData;

        /// <summary>
        /// Gateway to Notepad++ functions
        /// </summary>
        protected static NotepadPlusPlusGateway _notepadGateway;

        /// <summary>
        /// Gateway to Scintilla functions for the current editor
        /// </summary>
        protected static ScintillaGateway _scintillaGateway;

        /// <summary>
        /// Gateway to Scintilla functions for the main editor
        /// </summary>
        protected static ScintillaGateway _scintillaMainGateway;

        /// <summary>
        /// Gateway to Scintilla functions for the secondary editor
        /// </summary>
        protected static ScintillaGateway _scintillaSecondGateway;

        /// <summary>
        /// List of plugin commands
        /// </summary>
        protected List<MenuItemBase> _menuItems = new List<MenuItemBase>();

        /// <summary>
        /// Plugin initialization
        /// </summary>
        public virtual void Initialize()
        {
            // Initialize gateways
            _notepadGateway = new NotepadPlusPlusGateway(_nppData._nppHandle);
            _scintillaMainGateway = new ScintillaGateway(_nppData._scintillaMainHandle);
            _scintillaSecondGateway = new ScintillaGateway(_nppData._scintillaSecondHandle);

            // Get current scintilla
            IntPtr scintillaPtr = _notepadGateway.GetCurrentScintilla();
            _scintillaGateway = new ScintillaGateway(scintillaPtr);
        }

        /// <summary>
        /// Sets the plugin data from Notepad++
        /// </summary>
        public static void SetNppData(NppData nppData)
        {
            _nppData = nppData;
        }

        /// <summary>
        /// Gets the plugin's configuration directory
        /// </summary>
        public static string GetPluginsConfigDir()
        {
            string configDir = _notepadGateway?.GetPluginsConfigDir();
            if (string.IsNullOrEmpty(configDir))
            {
                // Fallback: construct path
                string nppPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                configDir = Path.Combine(nppPath, "Notepad++", "plugins", "config");
            }

            // Create plugin-specific config directory
            string pluginConfigDir = Path.Combine(configDir, PluginName);
            if (!Directory.Exists(pluginConfigDir))
            {
                Directory.CreateDirectory(pluginConfigDir);
            }

            return pluginConfigDir;
        }

        /// <summary>
        /// Called when the plugin is being cleaned up
        /// </summary>
        public virtual void Cleanup()
        {
        }

        /// <summary>
        /// Gets the command count
        /// </summary>
        public int GetCommandCount()
        {
            return _menuItems.Count;
        }

        /// <summary>
        /// Creates the FuncItem array for Notepad++
        /// </summary>
        public IntPtr CreateFuncArray()
        {
            int count = _menuItems.Count;
            IntPtr ptr = Marshal.AllocCoTaskMem(count * Marshal.SizeOf(typeof(FuncItem)));

            for (int i = 0; i < count; i++)
            {
                FuncItem item = _menuItems[i].CreateFuncItem();
                IntPtr itemPtr = new IntPtr(ptr.ToInt64() + i * Marshal.SizeOf(typeof(FuncItem)));
                Marshal.StructureToPtr(item, itemPtr, false);
            }

            return ptr;
        }

        /// <summary>
        /// Called when a command is executed
        /// </summary>
        public void Command(int index)
        {
            if (index >= 0 && index < _menuItems.Count)
            {
                _menuItems[index].OnCommand();
            }
        }

        /// <summary>
        /// Refreshes the scintilla gateway with the current scintilla
        /// </summary>
        public void RefreshScintillaGateway()
        {
            IntPtr scintillaPtr = _notepadGateway.GetCurrentScintilla();
            _scintillaGateway = new ScintillaGateway(scintillaPtr);
        }

        /// <summary>
        /// Checks if the current file is a Mermaid file
        /// </summary>
        public static bool IsMermaidFile()
        {
            string fileName = _notepadGateway?.GetCurrentFileName();
            if (string.IsNullOrEmpty(fileName))
                return false;

            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension == ".mmd" || extension == ".mermaid" || 
                   fileName.StartsWith("%%startmermaid", StringComparison.OrdinalIgnoreCase) ||
                   fileName.Contains("mermaid");
        }

        /// <summary>
        /// Gets the tool path from the plugin directory
        /// </summary>
        public static string GetToolPath(string toolName)
        {
            string pluginDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(pluginDir, toolName);
        }
    }
}
