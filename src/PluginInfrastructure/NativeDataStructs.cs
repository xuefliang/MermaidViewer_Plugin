using System;
using System.Runtime.InteropServices;

namespace MermaidViewer
{
    /// <summary>
    /// This static class inherits the PluginInfrastructure from the Notepad++ PluginPack.Net
    /// Source: https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    [Obsolete("Use NotepadPlusPlusGateway and ScintillaGateway instead")]
    public static class NotepadPP
    {
    }

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
    /// Struct used by NPPM_DMMVIEWOTHER and similar messages
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    /// <summary>
    /// Plugin command definition
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FuncItem
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string _itemName;
        public pFuncItemProc _pFuncItemProc;
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

        public ShortcutKey(bool isCtrl, bool isAlt, bool isShift, uint key)
        {
            _isCtrl = isCtrl;
            _isAlt = isAlt;
            _isShift = isShift;
            _key = key;
        }
    }

    /// <summary>
    /// Toolbar icon definition
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ToolbarIcon
    {
        public IntPtr _iconId;
    }

    /// <summary>
    /// NPPM_DMMREG External Plugin Interface
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NppPluginInfo
    {
        public int _funcItemCount;
        public IntPtr _funcItems;
        public IntPtr _pBitmapSet;
        public IntPtr _pIconSet;
        public int _docType;
        public IntPtr _pDLGTemplate;
        public IntPtr _pInfo;
    }

    /// <summary>
    /// Notification data structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NppNotification
    {
        public uint _hdrver;
        public uint _ctrlId;
        public IntPtr _hwndFrom;
        public IntPtr _idFrom;
        public uint _code;
        public uint _nmhdr_idFrom;
        public uint _nmhdr_code;
        public uint _nmhdr_hwndFrom;
        public IntPtr _nmhdr_ptr;
        public int _nmhdr_id;
        public uint _nmhdr_ctx;
        public uint _nmhdr_evt;
        public IntPtr _nmhdr_pnmhdr;
        public IntPtr _nmhdr_pinfo;
    }
}
