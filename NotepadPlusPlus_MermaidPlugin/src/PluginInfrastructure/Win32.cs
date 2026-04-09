using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MermaidViewer
{
    /// <summary>
    /// Wrapper for Win32 API calls
    /// </summary>
    public static class Win32
    {
        // Window messages
        public const uint WM_CLOSE = 0x0010;
        public const uint WM_DESTROY = 0x0002;
        public const uint WM_SIZE = 0x0005;

        // Scintilla messages
        public const uint SCI_GETLENGTH = 2001;
        public const uint SCI_GETTEXT = 2002;
        public const uint SCI_GETLINE = 2003;
        public const uint SCI_GETCURRENTPOS = 2008;
        public const uint SCI_GETSELECTIONSTART = 2141;
        public const uint SCI_GETSELECTIONEND = 2142;
        public const uint SCI_GETSELTEXT = 2006;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
