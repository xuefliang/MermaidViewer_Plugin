using System;
using System.Runtime.InteropServices;

namespace MermaidViewer
{
    /// <summary>
    /// Delegate for plugin menu item callback
    /// </summary>
    public delegate void PFuncItemProc();

    /// <summary>
    /// Data structure for passing information from Notepad++ to a plugin
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NppData
    {
        public IntPtr _nppHandle;
        public IntPtr _scintillaMainHandle;
        public IntPtr _scintillaSecondHandle;
    }

    /// <summary>
    /// Plugin command definition
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FuncItem
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string _itemName;
        public PFuncItemProc _pFuncItemProc;
        public int _cmdID;
        public bool _init2Check;
        public IntPtr _pShKey;
    }

    /// <summary>
    /// Keyboard shortcut definition
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ShortcutKey
    {
        public bool _isCtrl;
        public bool _isAlt;
        public bool _isShift;
        public uint _key;
    }
}
