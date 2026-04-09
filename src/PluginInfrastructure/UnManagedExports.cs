using System;
using System.Runtime.InteropServices;

namespace MermaidViewer
{
    /// <summary>
    /// Plugin exports required by Notepad++
    /// This class contains the DLL entry points that Notepad++ uses to communicate with the plugin
    /// </summary>
    public static class UnmanagedExports
    {
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
            MermaidPlugin.Plugin.Initialize();
        }

        /// <summary>
        /// Gets the plugin name
        /// </summary>
        [DllExport]
        public static IntPtr GetName()
        {
            return Marshal.StringToHGlobalUni(MermaidPlugin.PluginName);
        }

        /// <summary>
        /// Gets the command count
        /// </summary>
        [DllExport]
        public static int GetFuncsCount(int nbConstants)
        {
            return MermaidPlugin.Plugin.GetCommandCount();
        }

        /// <summary>
        /// Gets the function array
        /// </summary>
        [DllExport]
        public static IntPtr GetFuncsArray(IntPtr nbConstants)
        {
            return MermaidPlugin.Plugin.CreateFuncArray();
        }

        /// <summary>
        /// Called when a menu item is clicked
        /// </summary>
        [DllExport]
        public static void RunCommand(int funcItemIndex, IntPtr reserved)
        {
            MermaidPlugin.Plugin.Command(funcItemIndex);
        }

        /// <summary>
        /// Handles notifications from Notepad++
        /// </summary>
        [DllExport]
        public static IntPtr MessageProc(int messageType, IntPtr wParam, IntPtr lParam)
        {
            return MermaidPlugin.Plugin.HandleNotification(messageType, wParam, lParam);
        }

        /// <summary>
        /// Gets the plugin version
        /// </summary>
        [DllExport]
        public static IntPtr GetPluginVersion()
        {
            return Marshal.StringToHGlobalUni(MermaidPlugin.Plugin.GetVersion());
        }
    }
}
