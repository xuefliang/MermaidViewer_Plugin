using System;
using System.Runtime.InteropServices;

namespace MermaidViewer
{
    /// <summary>
    /// Plugin exports required by Notepad++
    /// </summary>
    public static class UnmanagedExports
    {
        private static MermaidPlugin _plugin;

        /// <summary>
        /// Entry point for plugin initialization
        /// </summary>
        [DllExport]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static bool IsUnicode()
        {
            return true;
        }

        /// <summary>
        /// Sets the plugin data from Notepad++
        /// </summary>
        [DllExport]
        public static void SetInfo(NppData nppData)
        {
            PluginBase.SetNppData(nppData);
            _plugin = new MermaidPlugin();
            _plugin.Initialize();
        }

        /// <summary>
        /// Gets the plugin name
        /// </summary>
        [DllExport]
        public static IntPtr GetName()
        {
            return Marshal.StringToHGlobalUni(PluginBase.PluginName);
        }

        /// <summary>
        /// Gets the command count
        /// </summary>
        [DllExport]
        public static int GetFuncsCount(int nbConstants)
        {
            return _plugin != null ? _plugin.GetCommandCount() : 0;
        }

        /// <summary>
        /// Gets the function array
        /// </summary>
        [DllExport]
        public static IntPtr GetFuncsArray(IntPtr nbConstants)
        {
            return _plugin != null ? _plugin.CreateFuncArray() : IntPtr.Zero;
        }

        /// <summary>
        /// Called when a menu item is clicked
        /// </summary>
        [DllExport]
        public static void RunCommand(int funcItemIndex, IntPtr reserved)
        {
            if (_plugin != null)
            {
                _plugin.Command(funcItemIndex);
            }
        }

        /// <summary>
        /// Handles notifications from Notepad++
        /// </summary>
        [DllExport]
        public static IntPtr MessageProc(int messageType, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// Gets the plugin version
        /// </summary>
        [DllExport]
        public static IntPtr GetPluginVersion()
        {
            return Marshal.StringToHGlobalUni("1.0.0");
        }
    }
}
